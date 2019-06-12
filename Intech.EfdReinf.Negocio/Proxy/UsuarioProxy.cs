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
        /// Realiza login do usu�rio.
        /// </summary>
        /// <param name="email">E-mail do usu�rio a ser logado</param>
        /// <param name="senhaSemCriptografia">Senha do usu�rio</param>
        /// <returns></returns>
        public UsuarioEntidade Login(string email, string senhaSemCriptografia)
        {
            var usuario = BuscarPorEmail(email);
            if (usuario == null)
                throw new Exception("E-mail n�o cadastrado.");

            if (usuario.IND_EMAIL_VERIFICADO == DMN_SN.NAO)
                throw new Exception("IND_EMAIL_VERIFICADO");

            if (usuario.IND_ATIVO == DMN_SN.NAO)
                throw new Exception("O E-mail cadastrado n�o est� ativo. Favor entrar em contato com a Intech.");

            if(usuario.IND_BLOQUEADO == DMN_SN.SIM)
                throw new Exception("O E-mail cadastrado est� bloqueado. Favor entrar em contato com a Intech.");
            
            var senhaEncriptada = Criptografia.Encriptar(senhaSemCriptografia);
            
            if (usuario.PWD_USUARIO != senhaEncriptada)
            {
                usuario.NUM_TENTATIVA++;

                if (usuario.NUM_TENTATIVA >= 5)
                {
                    usuario.IND_BLOQUEADO = DMN_SN.SIM;
                    base.Atualizar(usuario);
                    throw new Exception("N�mero de tentativas de acesso esgotado. O usu�rio est� bloqueado. Favor entrar em contato com a Intech");
                }
                else
                {
                    base.Atualizar(usuario);
                }

                throw new Exception("Credenciais Inv�lidas");
            }

            if (usuario.NUM_TENTATIVA > 0)
            {
                usuario.NUM_TENTATIVA = 0;
                base.Atualizar(usuario);
            }

            return usuario;
        }

        /// <summary>
        /// Insere um novo usu�rio.
        /// </summary>
        /// <param name="usuario">Usu�rio a ser inclu�do.</param>
        /// <returns></returns>
        public override long Inserir(UsuarioEntidade usuario)
        {
            try
            {
                // Valida e-mail
                if (!Validador.ValidarEmail(usuario.TXT_EMAIL))
                    throw new Exception("E-mail inv�lido.");

                // Valida CPF
                if (!Validador.ValidarCPF(usuario.COD_CPF))
                    throw new Exception("CPF inv�lido.");

                // Limpa m�scaras
                usuario.COD_CPF = usuario.COD_CPF.LimparMascara();
                usuario.COD_TELEFONE_CEL = usuario.COD_TELEFONE_CEL.LimparMascara();
                usuario.COD_TELEFONE_FIXO = usuario.COD_TELEFONE_FIXO.LimparMascara();

                // Valida telefones
                if (usuario.COD_TELEFONE_CEL.Length < 11)
                    throw new Exception("Telefone Celular inv�lido");

                if(usuario.COD_TELEFONE_FIXO.Length < 10)
                    throw new Exception("Telefone Fixo inv�lido");

                // Valida tamanho da senha
                if (usuario.PWD_USUARIO.Length < 6)
                    throw new Exception("A senha deve possuir no m�nimo 6 caracteres.");

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
        /// Inclui um novo usu�rio. Rotina mais utilizada pela tela de cadastro de novo usu�rio.
        /// </summary>
        /// <param name="usuario">Usu�rio a ser cadastrado</param>
        /// <returns>Retorna o OID do novo usu�rio</returns>
        public decimal InserirNovoUsuario(UsuarioEntidade usuario)
        {
            if (usuario.PWD_USUARIO.Length < 6)
                throw new Exception("A senha deve conter no m�nimo 6 caracteres.");

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
        /// Envia novo e-mail de confirma��o de cadastro do usu�rio.
        /// </summary>
        /// <param name="usuario">Usu�rio a ser enviado o e-mail</param>
        public void EnviarEmailConfirmacao(UsuarioEntidade usuario)
        {
            // Envia e-mail com nova senha de acesso
            var config = AppSettings.Get();

            var textoEmail = $"<h2>Bem-Vindo ao Intech EFD-Reinf</h2>" +
                $"Para confirmar seu cadastro, clique no link a seguir: <a href=\"{config.PublicacaoAPI}/usuario/confirmarEmail/{usuario.TXT_TOKEN}\">Confirmar e-mail</a>";

            EnvioEmail.EnviarMailKit(config.Email, usuario.TXT_EMAIL, $"EFD-Reinf - Confirma��o de Cadastro", textoEmail);
        }
        
        /// <summary>
        /// Utilizado para gerar uma nova senha aleat�ria de 6 d�gitos.
        /// No fim da atualiza��o, envia um e-mail para o usu�rio com a nova senha gerada.
        /// </summary>
        /// <param name="email">E-mail do usu�rio</param>
        public void RecuperarSenha(string email)
        {
            var usuario = BuscarPorEmail(email);

            if (usuario == null)
                throw new Exception("E-mail n�o cadastrado.");

            if (usuario.IND_ATIVO == DMN_SN.NAO)
                throw new Exception("O E-mail cadastrado n�o est� ativo. Favor entrar em contato com a Intech.");

            var novaSenha = new Random().Next(100000, 999999);

            usuario.PWD_USUARIO = Criptografia.Encriptar(novaSenha.ToString());
            usuario.IND_BLOQUEADO = DMN_SN.NAO;
            usuario.NUM_TENTATIVA = 0;

            Atualizar(usuario);

            var config = AppSettings.Get();

            var textoEmail = $"<h2>Bem-Vindo ao Intech EFD-Reinf</h2>" +
                $"A sua nova senha � {novaSenha}";

            EnvioEmail.EnviarMailKit(config.Email, usuario.TXT_EMAIL, $"EFD-Reinf - Recupera��o de Senha", textoEmail);
        }

        /// <summary>
        /// Troca a senha do usu�rio.
        /// </summary>
        /// <param name="email">E-mail do usu�rio a ser trocada a senha</param>
        /// <param name="senhaAtual">Senha atual do usu�rio</param>
        /// <param name="novaSenha">Nova senha do usu�rio</param>
        public void AlterarSenha(string email, string senhaAtual, string novaSenha)
        {
            var usuario = BuscarPorEmail(email);
            var senhaAntigaEncriptada = Criptografia.Encriptar(senhaAtual);

            if (usuario.PWD_USUARIO != senhaAntigaEncriptada)
                throw new Exception("A senha atual deve ser igual � senha cadastrada para o usu�rio logado.");

            usuario.PWD_USUARIO = Criptografia.Encriptar(novaSenha);

            Atualizar(usuario);
        }

        /// <summary>
        /// Atualiza os dados do usu�rio.
        /// </summary>
        /// <param name="usuario">Usu�rio a ser atualizado</param>
        /// <returns></returns>
        public override bool Atualizar(UsuarioEntidade usuario)
        {
            try
            {
                // Valida e-mail
                if (!Validador.ValidarEmail(usuario.TXT_EMAIL))
                    throw new Exception("E-mail inv�lido.");

                // Valida CPF
                if (!Validador.ValidarCPF(usuario.COD_CPF))
                    throw new Exception("CPF inv�lido.");

                // Limpa m�scaras
                usuario.COD_CPF = usuario.COD_CPF.LimparMascara();
                usuario.COD_TELEFONE_CEL = usuario.COD_TELEFONE_CEL.LimparMascara();
                usuario.COD_TELEFONE_FIXO = usuario.COD_TELEFONE_FIXO.LimparMascara();

                // Valida telefones
                if (usuario.COD_TELEFONE_CEL.Length < 11)
                    throw new Exception("Telefone Celular inv�lido");

                if (usuario.COD_TELEFONE_FIXO.Length < 10)
                    throw new Exception("Telefone Fixo inv�lido");

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