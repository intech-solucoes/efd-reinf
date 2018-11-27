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
    public abstract class ImportacaoCriticaDAO : BaseDAO<ImportacaoCriticaEntidade>
    {
        
		public virtual ImportacaoCriticaEntidade ExcluirPorOidArquivoUpload(decimal OID_ARQUIVO_UPLOAD)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.QuerySingleOrDefault<ImportacaoCriticaEntidade>("DELETE FROM EFD_IMPORTACAO_CRITICA WHERE OID_ARQUIVO_UPLOAD = @OID_ARQUIVO_UPLOAD", new { OID_ARQUIVO_UPLOAD });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.QuerySingleOrDefault<ImportacaoCriticaEntidade>("DELETE FROM EFD_IMPORTACAO_CRITICA WHERE OID_ARQUIVO_UPLOAD=:OID_ARQUIVO_UPLOAD", new { OID_ARQUIVO_UPLOAD });
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
