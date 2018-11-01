#region Usings
using Intech.EfdReinf.Dados.DAO;
using Intech.EfdReinf.Entidades;
using Intech.Lib.Data.Erros;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Email;
using Intech.Lib.Util.Validacoes;
using Intech.Lib.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
#endregion

namespace Intech.EfdReinf.Negocio.Proxy
{
    public class ContribuinteProxy : ContribuinteDAO
    {
        public override ContribuinteEntidade BuscarPorChave(object chave)
        {
            var contribuinte = base.BuscarPorChave(chave);

            contribuinte.Usuarios = new UsuarioContribuinteProxy().BuscarPorOidContribuinte((decimal)chave).ToList();

            return contribuinte;
        }

        /// <summary>
        /// Busca todos os contribuintes do usu�rio.
        /// Aplica m�scara do CNPJ ao retornar.
        /// </summary>
        /// <param name="OID_USUARIO">OID do usu�rio.</param>
        /// <returns></returns>
        public override IEnumerable<ContribuinteEntidade> BuscarPorOidUsuario(decimal OID_USUARIO)
        {
            var listaContribuintes = base.BuscarPorOidUsuario(OID_USUARIO).ToList();

            listaContribuintes.ForEach(contribuinte =>
            {
                var cpfCnpjComMascara =
                    contribuinte.IND_TIPO_INSCRICAO == DMN_TIPO_INSCRICAO_EFD.PESSOA_JURIDICA ?
                    contribuinte.COD_CNPJ_CPF.AplicarMascara(Mascaras.CNPJ) :
                    contribuinte.COD_CNPJ_CPF.AplicarMascara(Mascaras.CPF);

                contribuinte.COD_CNPJ_CPF = cpfCnpjComMascara;
                contribuinte.Usuarios = new UsuarioContribuinteProxy().BuscarPorOidUsuario(OID_USUARIO).ToList();
                contribuinte.OID_USUARIO_CONTRIBUINTE = contribuinte
                    .Usuarios
                    .First(x => x.OID_USUARIO == OID_USUARIO && 
                    x.OID_CONTRIBUINTE == contribuinte.OID_CONTRIBUINTE)
                    .OID_USUARIO_CONTRIBUINTE;
            });

            return listaContribuintes;
        }

        public decimal Inserir(ContribuinteEntidade contribuinte, decimal oidUsuario)
        {
            // Valida��es pessoa jur�dica
            if(contribuinte.IND_TIPO_INSCRICAO == DMN_TIPO_INSCRICAO_EFD.PESSOA_JURIDICA)
            {
                if (!Validador.ValidarCNPJ(contribuinte.COD_CNPJ_CPF))
                    throw new Exception("CNPJ inv�lido.");
            }

            // Valida��es pessoa f�sica
            else if (contribuinte.IND_TIPO_INSCRICAO == DMN_TIPO_INSCRICAO_EFD.PESSOA_FISICA)
            {
                if(!Validador.ValidarCPF(contribuinte.COD_CNPJ_CPF))
                    throw new Exception("CPF inv�lido.");

                if (contribuinte.IND_CLASSIF_TRIBUT != "21" || contribuinte.IND_CLASSIF_TRIBUT != "22")
                    throw new Exception("Os c�digos [21] \"Pessoa F�sica, exceto Segurado Especial\" e [22] \"Segurado Especial\" " +
                        "somente podem ser utilizados se o \"Tipo de Inscri��o\" for igual a \"PESSOA F�SICA\". Para os demais c�digos, " +
                        "o \"Tipo de Inscri��o\" deve ser igual a \"PESSOA JUR�DICA.\"");
            }

            if(!string.IsNullOrEmpty(contribuinte.TXT_EMAIL_CONTATO) && !Validador.ValidarEmail(contribuinte.TXT_EMAIL_CONTATO))
                throw new Exception("E-mail inv�lido.");

            if (contribuinte.IND_EFR == DMN_EFR_EFD.SIM && string.IsNullOrEmpty(contribuinte.COD_CNPJ_EFR))
                throw new Exception("CNPJ do Ente Federativo Respons�vel - EFR � obrigat�rio e exclusivo se EFR = Sim. Informa��o validada no cadastro do CNPJ da RFB.");
            
            if (contribuinte.DTA_FIM_VALIDADE != null && contribuinte.DTA_INICIO_VALIDADE > contribuinte.DTA_FIM_VALIDADE)
                throw new Exception("A data de T�rmino Validade deve ser maior que a data de In�cio Validade");

            contribuinte.COD_CNPJ_EFR = contribuinte.COD_CNPJ_EFR.LimparMascara();
            contribuinte.COD_CNPJ_CPF = contribuinte.COD_CNPJ_CPF.LimparMascara();
            contribuinte.COD_CPF_CONTATO = contribuinte.COD_CPF_CONTATO.LimparMascara();
            contribuinte.COD_FONE_CELULAR_CONTATO = contribuinte.COD_FONE_CELULAR_CONTATO.LimparMascara();
            contribuinte.COD_FONE_FIXO_CONTATO = contribuinte.COD_FONE_FIXO_CONTATO.LimparMascara();
            contribuinte.DTA_VALIDADE = DateTime.Now;
            contribuinte.IND_APROVADO = DMN_SN.NAO;

            var proxyContribuinte = new ContribuinteProxy();
            var proxyUsuarioContribuinte = new UsuarioContribuinteProxy();

            // Valida se usu�rio possui um contribuinte com o mesmo CNPJ cadastrado
            var contribuinteExistente = proxyContribuinte.BuscarPorCpfCnpjOidUsuario(contribuinte.COD_CNPJ_CPF, oidUsuario);

            if (contribuinteExistente != null)
                throw new Exception("J� existe um contribuinte com esse CNPJ vinculado ao usu�rio logado.");

            contribuinteExistente = proxyContribuinte.BuscarPorCpfCnpj(contribuinte.COD_CNPJ_CPF);

            if(contribuinteExistente != null)
                throw new Exception("J� existe um contribuinte com esse CNPJ vinculado a outro usu�rio. Favor entrar em contato com a Intech.");

            var oidContribuinte = base.Inserir(contribuinte);

            var oidUsuarioContribuinte = proxyUsuarioContribuinte.Inserir(new UsuarioContribuinteEntidade
            {
                OID_USUARIO = oidUsuario,
                OID_CONTRIBUINTE = oidContribuinte
            });

            // Envia e-mail com nova senha de acesso
            var config = AppSettings.Get();
            var usuario = new UsuarioProxy().BuscarPorChave(oidUsuario);

            var textoEmail = $"EFD-Reinf: Um novo contribuinte foi cadastrado pelo usu�rio {usuario.NOM_USUARIO}: {contribuinte.NOM_RAZAO_SOCIAL}.";

            EnvioEmail.EnviarMailKit(config.Email, config.EmailsCadastroContribuintes, $"[EFD-Reinf] - Novo Contribuinte", textoEmail);

            return oidContribuinte;
        }
    }
}
