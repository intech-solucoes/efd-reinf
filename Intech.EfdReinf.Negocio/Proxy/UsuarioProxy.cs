#region Usings
using Intech.EfdReinf.Dados.DAO;
using Intech.EfdReinf.Entidades;
using Intech.Lib.Data.Erros;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Email;
using Intech.Lib.Util.Seguranca;
using Intech.Lib.Util.Validacoes;
using Intech.Lib.Web;
using System;
using System.Data.SqlClient; 
#endregion

namespace Intech.EfdReinf.Negocio.Proxy
{
    public class UsuarioProxy : UsuarioDAO
    {
        /// <summary>
        /// Realiza login do usuário.
        /// </summary>
        /// <param name="email">E-mail do usuário a ser logado</param>
        /// <param name="senhaSemCriptografia">Senha do usuário</param>
        /// <returns></returns>
        public UsuarioEntidade Login(string email, string senhaSemCriptografia)
        {
            var usuario = BuscarPorEmail(email);
            if (usuario == null)
                throw new Exception("E-mail não cadastrado.");

            if (usuario.IND_EMAIL_VERIFICADO == DMN_SN.NAO)
                throw new Exception("IND_EMAIL_VERIFICADO");

            if (usuario.IND_ATIVO == DMN_SN.NAO)
                throw new Exception("O E-mail cadastrado não está ativo. Favor entrar em contato com a Intech.");

            if(usuario.IND_BLOQUEADO == DMN_SN.SIM)
                throw new Exception("O E-mail cadastrado está bloqueado. Favor entrar em contato com a Intech.");
            
            var senhaEncriptada = Criptografia.Encriptar(senhaSemCriptografia);
            
            if (usuario.PWD_USUARIO != senhaEncriptada)
            {
                usuario.NUM_TENTATIVA++;

                if (usuario.NUM_TENTATIVA >= 5)
                {
                    usuario.IND_BLOQUEADO = DMN_SN.SIM;
                    base.Atualizar(usuario);
                    throw new Exception("Número de tentativas de acesso esgotado. O usuário está bloqueado. Favor entrar em contato com a Intech");
                }
                else
                {
                    base.Atualizar(usuario);
                }

                throw new Exception("Credenciais Inválidas");
            }

            if (usuario.NUM_TENTATIVA > 0)
            {
                usuario.NUM_TENTATIVA = 0;
                base.Atualizar(usuario);
            }

            return usuario;
        }

        /// <summary>
        /// Insere um novo usuário.
        /// </summary>
        /// <param name="usuario">Usuário a ser incluído.</param>
        /// <returns></returns>
        public override long Inserir(UsuarioEntidade usuario)
        {
            try
            {
                // Valida e-mail
                if (!Validador.ValidarEmail(usuario.TXT_EMAIL))
                    throw new Exception("E-mail inválido.");

                // Valida CPF
                if (!Validador.ValidarCPF(usuario.COD_CPF))
                    throw new Exception("CPF inválido.");

                // Limpa máscaras
                usuario.COD_CPF = usuario.COD_CPF.LimparMascara();
                usuario.COD_TELEFONE_CEL = usuario.COD_TELEFONE_CEL.LimparMascara();
                usuario.COD_TELEFONE_FIXO = usuario.COD_TELEFONE_FIXO.LimparMascara();

                // Valida telefones
                if (usuario.COD_TELEFONE_CEL.Length < 11)
                    throw new Exception("Telefone Celular inválido");

                if(usuario.COD_TELEFONE_FIXO.Length < 10)
                    throw new Exception("Telefone Fixo inválido");

                // Valida tamanho da senha
                if (usuario.PWD_USUARIO.Length < 6)
                    throw new Exception("A senha deve possuir no mínimo 6 caracteres.");

                // Encripta a senha
                usuario.PWD_USUARIO = Criptografia.Encriptar(usuario.PWD_USUARIO);

                return base.Inserir(usuario);
            }
            catch(SqlException ex)
            {
                throw new Exception(ErrosBanco.Traduzir(ex.Message, ex.Number));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inclui um novo usuário. Rotina mais utilizada pela tela de cadastro de novo usuário.
        /// </summary>
        /// <param name="usuario">Usuário a ser cadastrado</param>
        /// <returns>Retorna o OID do novo usuário</returns>
        public decimal InserirNovoUsuario(UsuarioEntidade usuario)
        {
            if (usuario.PWD_USUARIO.Length < 6)
                throw new Exception("A senha deve conter no mínimo 6 caracteres.");

            usuario.DTA_CRIACAO = DateTime.Now;
            usuario.IND_BLOQUEADO = DMN_SN.NAO;
            usuario.IND_ATIVO = DMN_SN.SIM;
            usuario.IND_ADMINISTRADOR = DMN_SN.NAO;
            usuario.IND_EMAIL_VERIFICADO = DMN_SN.NAO;
            usuario.NUM_TENTATIVA = 0;
            
            usuario.TXT_TOKEN = Guid.NewGuid().ToString();

            decimal oidUsuario = Inserir(usuario);

            EnviarEmailConfirmacao(usuario);

            return oidUsuario;
        }

        /// <summary>
        /// Envia novo e-mail de confirmação de cadastro do usuário.
        /// </summary>
        /// <param name="usuario">Usuário a ser enviado o e-mail</param>
        public void EnviarEmailConfirmacao(UsuarioEntidade usuario)
        {
            // Envia e-mail com nova senha de acesso
            var config = AppSettings.Get();

            var textoEmail = $"<h2>Bem-Vindo ao Intech EFD-Reinf</h2>" +
                $"Para confirmar seu cadastro, clique no link a seguir: <a href=\"{config.PublicacaoAPI}/usuario/confirmarEmail/{usuario.TXT_TOKEN}\">Confirmar e-mail</a>";

            EnvioEmail.EnviarMailKit(config.Email, usuario.TXT_EMAIL, $"EFD-Reinf - Confirmação de Cadastro", textoEmail);
        }
        
        /// <summary>
        /// Utilizado para gerar uma nova senha aleatória de 6 dígitos.
        /// No fim da atualização, envia um e-mail para o usuário com a nova senha gerada.
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        public void RecuperarSenha(string email)
        {
            var usuario = BuscarPorEmail(email);

            if (usuario == null)
                throw new Exception("E-mail não cadastrado.");

            if (usuario.IND_ATIVO == DMN_SN.NAO)
                throw new Exception("O E-mail cadastrado não está ativo. Favor entrar em contato com a Intech.");

            var novaSenha = new Random().Next(100000, 999999);

            usuario.PWD_USUARIO = Criptografia.Encriptar(novaSenha.ToString());
            usuario.IND_BLOQUEADO = DMN_SN.NAO;
            usuario.NUM_TENTATIVA = 0;

            Atualizar(usuario);

            var config = AppSettings.Get();

            var textoEmail = $"<h2>Bem-Vindo ao Intech EFD-Reinf</h2>" +
                $"A sua nova senha é {novaSenha}";

            EnvioEmail.EnviarMailKit(config.Email, usuario.TXT_EMAIL, $"EFD-Reinf - Recuperação de Senha", textoEmail);
        }

        /// <summary>
        /// Troca a senha do usuário.
        /// </summary>
        /// <param name="email">E-mail do usuário a ser trocada a senha</param>
        /// <param name="senhaAtual">Senha atual do usuário</param>
        /// <param name="novaSenha">Nova senha do usuário</param>
        public void AlterarSenha(string email, string senhaAtual, string novaSenha)
        {
            var usuario = BuscarPorEmail(email);
            var senhaAntigaEncriptada = Criptografia.Encriptar(senhaAtual);

            if (usuario.PWD_USUARIO != senhaAntigaEncriptada)
                throw new Exception("A senha atual deve ser igual à senha cadastrada para o usuário logado.");

            usuario.PWD_USUARIO = Criptografia.Encriptar(novaSenha);

            Atualizar(usuario);
        }

        /// <summary>
        /// Atualiza os dados do usuário.
        /// </summary>
        /// <param name="usuario">Usuário a ser atualizado</param>
        /// <returns></returns>
        public override bool Atualizar(UsuarioEntidade usuario)
        {
            try
            {
                // Valida e-mail
                if (!Validador.ValidarEmail(usuario.TXT_EMAIL))
                    throw new Exception("E-mail inválido.");

                // Valida CPF
                if (!Validador.ValidarCPF(usuario.COD_CPF))
                    throw new Exception("CPF inválido.");

                // Limpa máscaras
                usuario.COD_CPF = usuario.COD_CPF.LimparMascara();
                usuario.COD_TELEFONE_CEL = usuario.COD_TELEFONE_CEL.LimparMascara();
                usuario.COD_TELEFONE_FIXO = usuario.COD_TELEFONE_FIXO.LimparMascara();

                // Valida telefones
                if (usuario.COD_TELEFONE_CEL.Length < 11)
                    throw new Exception("Telefone Celular inválido");

                if (usuario.COD_TELEFONE_FIXO.Length < 10)
                    throw new Exception("Telefone Fixo inválido");

                return base.Atualizar(usuario);
            }
            catch (SqlException ex)
            {
                throw new Exception(ErrosBanco.Traduzir(ex.Message, ex.Number));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}