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
    public abstract class ContribuinteEnvioDAO : BaseDAO<ContribuinteEnvioEntidade>
    {
        
		public virtual IEnumerable<ContribuinteEnvioEntidade> BuscarPorOidContribuinte(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ContribuinteEnvioEntidade>("SELECT EFD_CONTRIBUINTE_ENVIO.*,     EFD_ARQUIVO_UPLOAD.DTA_UPLOAD,     EFD_ARQUIVO_UPLOAD.IND_STATUS FROM EFD_CONTRIBUINTE_ENVIO INNER JOIN EFD_ARQUIVO_UPLOAD ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD = EFD_CONTRIBUINTE_ENVIO.OID_ARQUIVO_UPLOAD WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ContribuinteEnvioEntidade>("SELECT EFD_CONTRIBUINTE_ENVIO.*, EFD_ARQUIVO_UPLOAD.DTA_UPLOAD, EFD_ARQUIVO_UPLOAD.IND_STATUS FROM EFD_CONTRIBUINTE_ENVIO INNER  JOIN EFD_ARQUIVO_UPLOAD  ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD=EFD_CONTRIBUINTE_ENVIO.OID_ARQUIVO_UPLOAD WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
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
