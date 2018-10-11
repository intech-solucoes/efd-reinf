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
    public abstract class ArquivoUploadDAO : BaseDAO<ArquivoUploadEntidade>
    {
        
		public virtual IEnumerable<ArquivoUploadEntidade> BuscarPorOidUsuarioContribuinte(decimal OID_USUARIO_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT * FROM EFD_ARQUIVO_UPLOAD WHERE OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE", new { OID_USUARIO_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT * FROM EFD_ARQUIVO_UPLOAD WHERE OID_USUARIO_CONTRIBUINTE=:OID_USUARIO_CONTRIBUINTE", new { OID_USUARIO_CONTRIBUINTE });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<ArquivoUploadEntidade> BuscarPorOidUsuarioContribuinteStatus(decimal OID_USUARIO_CONTRIBUINTE, string IND_STATUS)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT * FROM EFD_ARQUIVO_UPLOAD WHERE OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE   AND IND_STATUS = @IND_STATUS", new { OID_USUARIO_CONTRIBUINTE, IND_STATUS });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT * FROM EFD_ARQUIVO_UPLOAD WHERE OID_USUARIO_CONTRIBUINTE=:OID_USUARIO_CONTRIBUINTE AND IND_STATUS=:IND_STATUS", new { OID_USUARIO_CONTRIBUINTE, IND_STATUS });
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
