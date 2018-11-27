using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("TBG_DOMINIO")]
    public class DominioEntidade
    {
		[Key]
		public decimal OID_DOMINIO { get; set; }
		public string COD_DOMINIO { get; set; }
		public string SIG_DOMINIO { get; set; }
		public string NOM_DOMINIO { get; set; }
        
    }
}
