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
    public abstract class R2010DAO : BaseDAO<R2010Entidade>
    {
        
		public virtual IEnumerable<DateTime> BuscarDatasEnviados(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<DateTime>("SELECT DTA_APURACAO FROM EFD_R2010 WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE   AND IND_SITUACAO_PROCESSAMENTO = 'ENV' ORDER BY DTA_APURACAO", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<DateTime>("SELECT DTA_APURACAO FROM EFD_R2010 WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND IND_SITUACAO_PROCESSAMENTO='ENV' ORDER BY DTA_APURACAO", new { OID_CONTRIBUINTE });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<R2010Entidade> BuscarPorOidContribuinte(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<R2010Entidade>("SELECT EFD_R2010.*,     EFD_ARQUIVO_UPLOAD.DTA_UPLOAD,     EFD_ARQUIVO_UPLOAD.IND_STATUS FROM EFD_R2010 INNER JOIN EFD_ARQUIVO_UPLOAD ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD = EFD_R2010.OID_ARQUIVO_UPLOAD WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<R2010Entidade>("SELECT EFD_R2010.*, EFD_ARQUIVO_UPLOAD.DTA_UPLOAD, EFD_ARQUIVO_UPLOAD.IND_STATUS FROM EFD_R2010 INNER  JOIN EFD_ARQUIVO_UPLOAD  ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD=EFD_R2010.OID_ARQUIVO_UPLOAD WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<R2010Entidade> BuscarPorOidContribuinteDtaInicioDtaFimIndSituacaoProcessamento(decimal OID_CONTRIBUINTE, DateTime DTA_INICIO, DateTime DTA_FIM, string IND_SITUACAO_PROCESSAMENTO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<R2010Entidade>("SELECT *  FROM EFD_R2010 WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE   AND DTA_APURACAO >= @DTA_INICIO   AND DTA_APURACAO <= @DTA_FIM   AND IND_SITUACAO_PROCESSAMENTO = @IND_SITUACAO_PROCESSAMENTO ORDER BY DTA_APURACAO, COD_CNPJ_PRESTADOR, COD_SERIE_NF, NUM_DOCUMENTO_NF, DTA_EMISSAO_NF", new { OID_CONTRIBUINTE, DTA_INICIO, DTA_FIM, IND_SITUACAO_PROCESSAMENTO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<R2010Entidade>("SELECT * FROM EFD_R2010 WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND DTA_APURACAO>=:DTA_INICIO AND DTA_APURACAO<=:DTA_FIM AND IND_SITUACAO_PROCESSAMENTO=:IND_SITUACAO_PROCESSAMENTO ORDER BY DTA_APURACAO, COD_CNPJ_PRESTADOR, COD_SERIE_NF, NUM_DOCUMENTO_NF, DTA_EMISSAO_NF", new { OID_CONTRIBUINTE, DTA_INICIO, DTA_FIM, IND_SITUACAO_PROCESSAMENTO });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<R2010Entidade> BuscarPorOidContribuinteMesEnvioAnoEnvio(decimal OID_CONTRIBUINTE, decimal MES_ENVIO, decimal ANO_ENVIO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<R2010Entidade>("SELECT EFD_R2010.*  FROM EFD_R2010 WHERE EFD_R2010.OID_CONTRIBUINTE = @OID_CONTRIBUINTE   AND EFD_R2010.DTA_ENVIO = ( SELECT MAX(A.DTA_ENVIO) 							  FROM EFD_R2010 AS A  							  WHERE ( A.OID_CONTRIBUINTE = @OID_CONTRIBUINTE ) 							    AND (MONTH(A.DTA_ENVIO) = @MES_ENVIO) 								AND (YEAR(A.DTA_ENVIO) = @ANO_ENVIO ) 								AND (A.NUM_RECIBO_ENVIO <> NULL)) ORDER BY EFD_R2010.DTA_APURACAO, EFD_R2010.COD_CNPJ_PRESTADOR, EFD_R2010.COD_SERIE_NF, EFD_R2010.NUM_DOCUMENTO_NF, EFD_R2010.DTA_EMISSAO_NF", new { OID_CONTRIBUINTE, MES_ENVIO, ANO_ENVIO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<R2010Entidade>("SELECT EFD_R2010.* FROM EFD_R2010 WHERE EFD_R2010.OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND EFD_R2010.DTA_ENVIO=(SELECT MAX(A.DTA_ENVIO) FROM EFD_R2010 A WHERE (A.OID_CONTRIBUINTE=:OID_CONTRIBUINTE) AND (TO_CHAR(A.DTA_ENVIO,'MM')=:MES_ENVIO) AND (TO_CHAR(A.DTA_ENVIO,'YYYY')=:ANO_ENVIO) AND (A.NUM_RECIBO_ENVIO<>NULL)) ORDER BY EFD_R2010.DTA_APURACAO, EFD_R2010.COD_CNPJ_PRESTADOR, EFD_R2010.COD_SERIE_NF, EFD_R2010.NUM_DOCUMENTO_NF, EFD_R2010.DTA_EMISSAO_NF", new { OID_CONTRIBUINTE, MES_ENVIO, ANO_ENVIO });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual R2010Entidade ExcluirPorOidContribuinteDtaApuracao(decimal OID_CONTRIBUINTE, DateTime DTA_APURACAO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.QuerySingleOrDefault<R2010Entidade>("DELETE FROM EFD_R2010 WHERE EFD_R2010.OID_CONTRIBUINTE = @OID_CONTRIBUINTE   AND EFD_R2010.DTA_APURACAO = @DTA_APURACAO   AND EFD_R2010.IND_SITUACAO_PROCESSAMENTO IN('IMP', 'PRO')", new { OID_CONTRIBUINTE, DTA_APURACAO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.QuerySingleOrDefault<R2010Entidade>("DELETE FROM EFD_R2010 WHERE EFD_R2010.OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND EFD_R2010.DTA_APURACAO=:DTA_APURACAO AND EFD_R2010.IND_SITUACAO_PROCESSAMENTO IN ('IMP', 'PRO')", new { OID_CONTRIBUINTE, DTA_APURACAO });
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
