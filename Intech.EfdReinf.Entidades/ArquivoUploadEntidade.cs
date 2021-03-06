﻿using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Intech.EfdReinf.Entidades
{
    [Table("EFD_ARQUIVO_UPLOAD")]
    public class ArquivoUploadEntidade
    {
		[Key]
		public decimal OID_ARQUIVO_UPLOAD { get; set; }
		public decimal OID_USUARIO_CONTRIBUINTE { get; set; }
		public string NOM_ARQUIVO_ORIGINAL { get; set; }
		public string NOM_EXT_ARQUIVO { get; set; }
		public DateTime DTA_UPLOAD { get; set; }
		public string NOM_DIRETORIO_LOCAL { get; set; }
		public string NOM_ARQUIVO_LOCAL { get; set; }
		public string IND_STATUS { get; set; }
		[Write(false)] public string NOM_USUARIO { get; set; }
		[Write(false)] public string IND_AMBIENTE_ENVIO { get; set; }
		[Write(false)] public decimal? OID_USUARIO_ENVIO { get; set; }
		[Write(false)] public string IND_SITUACAO_PROCESSAMENTO { get; set; }
        
    }
}
