using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_IMPORTACAO_CRITICA")]
    public class ImportacaoCriticaEntidade
    {
		[Key]
		public decimal OID_IMPORTACAO_CRITICA { get; set; }
		public decimal OID_ARQUIVO_UPLOAD { get; set; }
		public decimal NUM_LINHA_CRITICA { get; set; }
		public string DES_CRITICA { get; set; }
        
    }
}
