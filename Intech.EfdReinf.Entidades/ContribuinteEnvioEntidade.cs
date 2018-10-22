using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_CONTRIBUINTE_ENVIO")]
    public class ContribuinteEnvioEntidade
    {
		[Key]
		public decimal OID_CONTRIBUINTE_ENVIO { get; set; }
		public decimal OID_CONTRIBUINTE { get; set; }
		public decimal OID_USUARIO_ENVIO { get; set; }
		public string IND_TIPO_AMBIENTE { get; set; }
		public string NUM_RECIBO_ENVIO { get; set; }
		public DateTime? DTA_ENVIO { get; set; }
		public string IND_TIPO_ENVIO { get; set; }
		public decimal OID_ARQUIVO_UPLOAD { get; set; }
		[Write(false)] public DateTime? DataGeracao { get; set; }
		[Write(false)] public string Status { get; set; }
        
    }
}
