using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_CONTRIBUINTE")]
    public class ContribuinteEntidade
    {
		[Key]
		public decimal OID_CONTRIBUINTE { get; set; }
		public string NOM_RAZAO_SOCIAL { get; set; }
		public string IND_TIPO_INSCRICAO { get; set; }
		public string COD_CNPJ_CPF { get; set; }
		public DateTime DTA_INICIO_VALIDADE { get; set; }
		public DateTime? DTA_FIM_VALIDADE { get; set; }
		public string IND_CLASSIF_TRIBUT { get; set; }
		public string IND_OBRIGADA_ECD { get; set; }
		public string IND_DESONERACAO_CPRB { get; set; }
		public string IND_ISENCAO_MULTA { get; set; }
		public string IND_SITUACAO_PJ { get; set; }
		public string IND_EFR { get; set; }
		public string COD_CNPJ_EFR { get; set; }
		public string NOM_CONTATO { get; set; }
		public string COD_CPF_CONTATO { get; set; }
		public string COD_FONE_FIXO_CONTATO { get; set; }
		public string COD_FONE_CELULAR_CONTATO { get; set; }
		public string TXT_EMAIL_CONTATO { get; set; }
		public string IND_APROVADO { get; set; }
		public DateTime DTA_VALIDADE { get; set; }
		public string IND_TIPO_AMBIENTE { get; set; }
		[Write(false)] public List<UsuarioContribuinteEntidade> Usuarios { get; set; }
		[Write(false)] public decimal? OID_USUARIO_CONTRIBUINTE { get; set; }
        
    }
}
