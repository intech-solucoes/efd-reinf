﻿#region Usings
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
					return Conexao.Query<ArquivoUploadEntidade>("SELECT EFD_ARQUIVO_UPLOAD.*, 	EFD_USUARIO.NOM_USUARIO FROM EFD_ARQUIVO_UPLOAD INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE = EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE INNER JOIN EFD_USUARIO ON EFD_USUARIO.OID_USUARIO = EFD_USUARIO_CONTRIBUINTE.OID_USUARIO WHERE EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE", new { OID_USUARIO_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT EFD_ARQUIVO_UPLOAD.*, EFD_USUARIO.NOM_USUARIO FROM EFD_ARQUIVO_UPLOAD INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE=EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE INNER  JOIN EFD_USUARIO  ON EFD_USUARIO.OID_USUARIO=EFD_USUARIO_CONTRIBUINTE.OID_USUARIO WHERE EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE=:OID_USUARIO_CONTRIBUINTE", new { OID_USUARIO_CONTRIBUINTE });
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
					return Conexao.Query<ArquivoUploadEntidade>("SELECT EFD_ARQUIVO_UPLOAD.*, 	EFD_USUARIO.NOM_USUARIO FROM EFD_ARQUIVO_UPLOAD INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE = EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE INNER JOIN EFD_USUARIO ON EFD_USUARIO.OID_USUARIO = EFD_USUARIO_CONTRIBUINTE.OID_USUARIO WHERE EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE = @OID_USUARIO_CONTRIBUINTE   AND (EFD_ARQUIVO_UPLOAD.IND_STATUS = @IND_STATUS OR @IND_STATUS IS NULL) ORDER BY EFD_ARQUIVO_UPLOAD.DTA_UPLOAD DESC", new { OID_USUARIO_CONTRIBUINTE, IND_STATUS });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT EFD_ARQUIVO_UPLOAD.*, EFD_USUARIO.NOM_USUARIO FROM EFD_ARQUIVO_UPLOAD INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE=EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE INNER  JOIN EFD_USUARIO  ON EFD_USUARIO.OID_USUARIO=EFD_USUARIO_CONTRIBUINTE.OID_USUARIO WHERE EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE=:OID_USUARIO_CONTRIBUINTE AND (EFD_ARQUIVO_UPLOAD.IND_STATUS=:IND_STATUS OR :IND_STATUS IS NULL ) ORDER BY EFD_ARQUIVO_UPLOAD.DTA_UPLOAD DESC", new { OID_USUARIO_CONTRIBUINTE, IND_STATUS });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<ArquivoUploadEntidade> BuscarR1070PorOidContribuinteNaoEnviado(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("select DISTINCT EFD_ARQUIVO_UPLOAD.*,                  EFD_R1070.IND_AMBIENTE_ENVIO,  				EFD_R1070.OID_USUARIO_ENVIO   from EFD_ARQUIVO_UPLOAD  INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE = EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE  INNER JOIN EFD_R1070 ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD = EFD_R1070.OID_ARQUIVO_UPLOAD  WHERE EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE = @OID_CONTRIBUINTE    AND IND_SITUACAO_PROCESSAMENTO <> 'ENV'", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT DISTINCT EFD_ARQUIVO_UPLOAD.*, EFD_R1070.IND_AMBIENTE_ENVIO, EFD_R1070.OID_USUARIO_ENVIO FROM EFD_ARQUIVO_UPLOAD INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE=EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE INNER  JOIN EFD_R1070  ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD=EFD_R1070.OID_ARQUIVO_UPLOAD WHERE EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND IND_SITUACAO_PROCESSAMENTO<>'ENV'", new { OID_CONTRIBUINTE });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<ArquivoUploadEntidade> BuscarR2010PorOidContribuinteNaoEnviado(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("select DISTINCT EFD_ARQUIVO_UPLOAD.*,                  EFD_R2010.IND_AMBIENTE_ENVIO,  				EFD_R2010.OID_USUARIO_ENVIO ,  				EFD_R2010.IND_SITUACAO_PROCESSAMENTO  from EFD_ARQUIVO_UPLOAD  INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE = EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE  INNER JOIN EFD_R2010 ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD = EFD_R2010.OID_ARQUIVO_UPLOAD  WHERE EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE = @OID_CONTRIBUINTE    AND IND_SITUACAO_PROCESSAMENTO <> 'ENV'", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ArquivoUploadEntidade>("SELECT DISTINCT EFD_ARQUIVO_UPLOAD.*, EFD_R2010.IND_AMBIENTE_ENVIO, EFD_R2010.OID_USUARIO_ENVIO, EFD_R2010.IND_SITUACAO_PROCESSAMENTO FROM EFD_ARQUIVO_UPLOAD INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_ARQUIVO_UPLOAD.OID_USUARIO_CONTRIBUINTE=EFD_USUARIO_CONTRIBUINTE.OID_USUARIO_CONTRIBUINTE INNER  JOIN EFD_R2010  ON EFD_ARQUIVO_UPLOAD.OID_ARQUIVO_UPLOAD=EFD_R2010.OID_ARQUIVO_UPLOAD WHERE EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE=:OID_CONTRIBUINTE AND IND_SITUACAO_PROCESSAMENTO<>'ENV'", new { OID_CONTRIBUINTE });
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
