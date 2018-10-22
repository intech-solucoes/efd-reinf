using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_R2098")]
    public class R2098Entidade
    {
		[Key]
		public decimal OID_R2098_ENVIO { get; set; }
		public decimal OID_CONTRIBUINTE { get; set; }
		public decimal OID_USUARIO_ENVIO { get; set; }
		public DateTime DTA_PERIODO_APURACAO { get; set; }
		public string IND_AMBIENTE_ENVIO { get; set; }
		public string NUM_RECIBO_ENVIO { get; set; }
		public DateTime? DTA_ENVIO { get; set; }
		public string IND_SITUACAO_PROCESSAMENTO_ { get; set; }
		public decimal OID_ARQUIVO_UPLOAD { get; set; }
		[Write(false)] public DateTime? DataGeracao { get; set; }
		[Write(false)] public string Status { get; set; }
        
    }
}
