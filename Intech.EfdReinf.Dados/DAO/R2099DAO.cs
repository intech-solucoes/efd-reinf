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
    public abstract class R2099DAO : BaseDAO<R2099Entidade>
    {
        
		public virtual IEnumerable<R2099Entidade> BuscarPorOidContribuinteAnoMesIndSituacaoProcessamento(decimal OID_CONTRIBUINTE, int ANO, int MES, string IND_SITUACAO_PROCESSAMENTO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<R2099Entidade>("SELECT *  FROM EFD_R2099 WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE   AND YEAR(DTA_PERIODO_APURACAO) >= @ANO   AND MONTH(DTA_PERIODO_APURACAO) <= @MES   AND IND_SITUACAO_PROCESSAMENTO = @IND_SITUACAO_PROCESSAMENTO", new { OID_CONTRIBUINTE, ANO, MES, IND_SITUACAO_PROCESSAMENTO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<R2099Entidade>("SELECT * FROM EFD_R2099 WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND TO_CHAR(DTA_PERIODO_APURACAO,'YYYY')>=:ANO AND TO_CHAR(DTA_PERIODO_APURACAO,'MM')<=:MES AND IND_SITUACAO_PROCESSAMENTO=:IND_SITUACAO_PROCESSAMENTO", new { OID_CONTRIBUINTE, ANO, MES, IND_SITUACAO_PROCESSAMENTO });
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
