#region Usings
using Dapper;
using Intech.Lib.Dapper;
using Intech.Lib.Web;
using Intech.EfdReinf.Entidades;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
#endregion

namespace Intech.EfdReinf.Dados.DAO
{   
    public abstract class R2098DAO : BaseDAO<R2098Entidade>
    {
        
		public virtual IEnumerable<R2098Entidade> BuscarPorOidContribuinte(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<R2098Entidade>("SELECT EFD_R2098.*,     EFD_ARQUIVO_UPLOAD.DTA_UPLOAD,     EFD_ARQUIVO_UPLOAD.IND_STATUS FROM EFD_R2098 INNER JOIN EFD_ARQUIVO_UPLOAD ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD = EFD_R2098.OID_ARQUIVO_UPLOAD WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<R2098Entidade>("SELECT EFD_R2098.*, EFD_ARQUIVO_UPLOAD.DTA_UPLOAD, EFD_ARQUIVO_UPLOAD.IND_STATUS FROM EFD_R2098 INNER  JOIN EFD_ARQUIVO_UPLOAD  ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD=EFD_R2098.OID_ARQUIVO_UPLOAD WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

    }
}
