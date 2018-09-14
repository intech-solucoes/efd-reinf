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

namespace Intech.EfdReinf.Negocio.Proxy
{
    public class UsuarioProxy : UsuarioDAO
    {
        public UsuarioEntidade Login(string email, string senhaSemCriptografia)
        {
            var usuario = BuscarPorEmail(email);
            if (usuario == null)
                throw new Exception("E-mail não cadastrado.");

            if (usuario.IND_ATIVO == DMN_SN.NAO)
                throw new Exception("O E-mail cadastrado não está ativo. Favor entrar em contato com a Intech.");

            if(usuario.IND_BLOQUEADO == DMN_SN.SIM)
                throw new Exception("O E-mail cadastrado está bloqueado. Favor entrar em contato com a Intech.");
            
            var senhaEncriptada = Criptografia.Encriptar(senhaSemCriptografia);

            // TODO: Implementar tentativas
            if (usuario.PWD_USUARIO != senhaEncriptada)
            {
                usuario.NUM_TENTATIVA++;

                Atualizar(usuario);

                if (usuario.NUM_TENTATIVA >= 5)
                {
                    usuario.IND_BLOQUEADO = DMN_SN.SIM;
                    Atualizar(usuario);
                    throw new Exception("Número de tentativas de acesso esgotado. O usuário está bloqueado. Favor entrar em contato com a Intech");
                }

                throw new Exception("Credenciais Inválidas");
            }

            usuario.NUM_TENTATIVA = 0;

            Atualizar(usuario);

            return usuario;
        }

        public override long Inserir(UsuarioEntidade usuario)
        {
            try
            {
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

            // Envia e-mail com nova senha de acesso
            var config = AppSettings.Get();

            var textoEmail = $"<h2>Bem-Vindo ao Intech EFD-Reinf</h2>" +
                $"Para confirmar seu cadastro, clique no link a seguir: <a href=\"{config.PublicacaoAPI}/usuario/confirmarEmail/{usuario.TXT_TOKEN}\">Confirmar e-mail</a>";

            EnvioEmail.EnviarMailKit(config.Email, usuario.TXT_EMAIL, $"EFD-Reinf - Confirmação de Cadastro", textoEmail);

            return oidUsuario;
        }
    }
}