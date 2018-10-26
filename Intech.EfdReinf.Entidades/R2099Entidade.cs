using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_R2099")]
    public class R2099Entidade
    {
		[Key]
		public decimal OID_R2099 { get; set; }
		public decimal OID_CONTRIBUINTE { get; set; }
		public decimal OID_USUARIO_ENVIO { get; set; }
		public DateTime DTA_PERIODO_APURACAO { get; set; }
		public string IND_CONTRATACAO_SERV { get; set; }
		public string IND_PRESTACAO_SERV { get; set; }
		public string IND_ASSOCIACAO_DESPORTIVA { get; set; }
		public string IND_REPASSE_ASSOC_DESPORT { get; set; }
		public string IND_PRODUCAO_RURAL { get; set; }
		public string IND_CPRB { get; set; }
		public DateTime? DTA_COMPETENCIA_SEM_MOV { get; set; }
		public DateTime? DTA_ENVIO { get; set; }
		public string NUM_RECIBO_ENVIO { get; set; }
		public string IND_AMBIENTE_ENVIO { get; set; }
		public string IND_SITUACAO_PROCESSAMENTO { get; set; }
		public string IND_PAGAMENTOS_DIVERSOS { get; set; }
		public decimal OID_ARQUIVO_UPLOAD { get; set; }
		[Write(false)] public DateTime? DTA_UPLOAD { get; set; }
		[Write(false)] public string IND_STATUS { get; set; }
        
    }
}
