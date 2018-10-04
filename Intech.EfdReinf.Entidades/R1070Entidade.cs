using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_R1070")]
    public class R1070Entidade
    {
		[Key]
		public decimal OID_R1070 { get; set; }
		public decimal OID_CONTRIBUINTE { get; set; }
		public decimal? OID_USUARIO_ENVIO { get; set; }
		public string IND_TIPO_PROCESSO { get; set; }
		public string NUM_PROCESSO { get; set; }
		public DateTime DTA_INICIO_VALIDADE { get; set; }
		public DateTime? DTA_FIM_VALIDADE { get; set; }
		public string IND_AUTORIA_JUDICIAL { get; set; }
		public string COD_SUSPENSAO { get; set; }
		public string IND_SUSPENSAO { get; set; }
		public DateTime? DTA_DECISAO { get; set; }
		public string IND_DEPOSITO_JUDICIAL { get; set; }
		public string COD_UF_VARA { get; set; }
		public string COD_MUNICIPIO_VARA { get; set; }
		public string COD_IDENTIFICACAO_VARA { get; set; }
		public string IND_OPERACAO_REGISTRO { get; set; }
		public string IND_SITUACAO_PROCESSAMENTO { get; set; }
		public string IND_AMBIENTE_ENVIO { get; set; }
		public string NUM_RECIBO_ENVIO { get; set; }
		public DateTime? DTA_ENVIO { get; set; }
        
    }
}
