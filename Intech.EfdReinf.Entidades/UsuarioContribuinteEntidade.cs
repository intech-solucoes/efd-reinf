using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_USUARIO_CONTRIBUINTE")]
    public class UsuarioContribuinteEntidade
    {
		[Key]
		public decimal OID_USUARIO_CONTRIBUINTE { get; set; }
		public decimal OID_USUARIO { get; set; }
		public decimal OID_CONTRIBUINTE { get; set; }
		[Write(false)] public UsuarioEntidade Usuario { get; set; }
		[Write(false)] public ContribuinteEntidade Contribuinte { get; set; }
        
    }
}
