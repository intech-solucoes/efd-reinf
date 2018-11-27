using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_USUARIO")]
    public class UsuarioEntidade
    {
		[Key]
		public decimal OID_USUARIO { get; set; }
		public string TXT_EMAIL { get; set; }
		public string PWD_USUARIO { get; set; }
		public string NOM_USUARIO { get; set; }
		public string COD_CPF { get; set; }
		public string COD_TELEFONE_FIXO { get; set; }
		public string COD_TELEFONE_CEL { get; set; }
		public string IND_BLOQUEADO { get; set; }
		public decimal NUM_TENTATIVA { get; set; }
		public string IND_ADMINISTRADOR { get; set; }
		public string IND_ATIVO { get; set; }
		public string IND_EMAIL_VERIFICADO { get; set; }
		public string TXT_TOKEN { get; set; }
		public DateTime DTA_CRIACAO { get; set; }
        
    }
}
