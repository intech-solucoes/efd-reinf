using Intech.EfdReinf.Dados.DAO;
using Intech.EfdReinf.Entidades;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Seguranca;
using System;

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
                throw new Exception("Credenciais Inválidas");

            return usuario;
        }

        public override long Inserir(UsuarioEntidade usuario)
        {
            usuario.PWD_USUARIO = Criptografia.Encriptar(usuario.PWD_USUARIO);

            return base.Inserir(usuario);
        }

        public decimal InserirNovoUsuario(UsuarioEntidade usuario)
        {
            usuario.DTA_CRIACAO = DateTime.Now;
            usuario.IND_BLOQUEADO = DMN_SN.NAO;
            usuario.IND_ATIVO = DMN_SN.SIM;
            usuario.IND_ADMINISTRADOR = DMN_SN.NAO;
            usuario.IND_EMAIL_VERIFICADO = DMN_SN.NAO;
            usuario.NUM_TENTATIVA = 0;

            decimal oidUsuario = Inserir(usuario);

            return oidUsuario;
        }
    }
}
