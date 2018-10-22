using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_R2010")]
    public class R2010Entidade
    {
		[Key]
		public decimal OID_R2010 { get; set; }
		public decimal OID_CONTRIBUINTE { get; set; }
		public decimal? OID_USUARIO_ENVIO { get; set; }
		public DateTime DTA_APURACAO { get; set; }
		public string COD_INSC_ESTABELECIMENTO { get; set; }
		public string COD_CNPJ_CPF_OBRA { get; set; }
		public string IND_OBRA { get; set; }
		public string COD_CNPJ_PRESTADOR { get; set; }
		public decimal VAL_TOTAL_BRUTO { get; set; }
		public decimal VAL_BASE_RETENCAO { get; set; }
		public decimal VAL_TOTAL_RETENCAO { get; set; }
		public decimal? VAL_RETENCAO_ADICIONAL { get; set; }
		public decimal? VAL_RETENCAO_JUD_PRINC { get; set; }
		public decimal? VAL_RETENCAO_JUD_ADIC { get; set; }
		public decimal IND_CPRB { get; set; }
		public string COD_SERIE_NF { get; set; }
		public string NUM_DOCUMENTO_NF { get; set; }
		public DateTime DTA_EMISSAO_NF { get; set; }
		public decimal VAL_BRUTO_NF { get; set; }
		public string DES_OBSERVACAO_NF { get; set; }
		public string COD_TIPO_SERVICO { get; set; }
		public decimal VAL_BASE_RET_SERVICO { get; set; }
		public decimal VAL_RETENCAO_SERVICO { get; set; }
		public decimal? VAL_RET_SUBCONTRATO { get; set; }
		public decimal? VAL_RET_SERV_JUIZO { get; set; }
		public decimal? VAL_SERV_15_ANOS { get; set; }
		public decimal? VAL_SERV_20_ANOS { get; set; }
		public decimal? VAL_SERV_25_ANOS { get; set; }
		public decimal? VAL_RET_ADIC_ESPEC { get; set; }
		public decimal? VAL_RET_ADIC_JUIZO { get; set; }
		public string COD_PROCESSO_JUD { get; set; }
		public string NUM_PROCESSO_JUD { get; set; }
		public string COD_SUSPENSAO_PROCESSO { get; set; }
		public decimal? VAL_PRINCIPAL_PROCESSO { get; set; }
		public string COD_PROCESSO_JUD_ADIC { get; set; }
		public string NUM_PROCESSO_JUD_ADIC { get; set; }
		public string COD_SUSPENSAO_PROC_ADIC { get; set; }
		public decimal? VAL_ADICIONAL_PROCESSO { get; set; }
		public string IND_SITUACAO_PROCESSAMENTO { get; set; }
		public string IND_RETIFICACAO { get; set; }
		public string NUM_RECIBO_RETIFICADA { get; set; }
		public string IND_AMBIENTE_ENVIO { get; set; }
		public string NUM_RECIBO_ENVIO { get; set; }
		public DateTime? DTA_ENVIO { get; set; }
		public decimal OID_ARQUIVO_UPLOAD { get; set; }
		[Write(false)] public DateTime? DataGeracao { get; set; }
		[Write(false)] public string Status { get; set; }
        
    }
}
