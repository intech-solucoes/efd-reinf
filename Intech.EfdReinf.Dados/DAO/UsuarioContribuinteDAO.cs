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
    public abstract class UsuarioContribuinteDAO : BaseDAO<UsuarioContribuinteEntidade>
    {
        
		public virtual IEnumerable<UsuarioContribuinteEntidade> BuscarPorOidContribuinte(decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<UsuarioContribuinteEntidade>("SELECT * FROM EFD_USUARIO_CONTRIBUINTE WHERE OID_CONTRIBUINTE = @OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<UsuarioContribuinteEntidade>("SELECT * FROM EFD_USUARIO_CONTRIBUINTE WHERE OID_CONTRIBUINTE=:OID_CONTRIBUINTE", new { OID_CONTRIBUINTE });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<UsuarioContribuinteEntidade> BuscarPorOidUsuario(decimal OID_USUARIO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<UsuarioContribuinteEntidade>("SELECT * FROM EFD_USUARIO_CONTRIBUINTE WHERE OID_USUARIO = @OID_USUARIO", new { OID_USUARIO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<UsuarioContribuinteEntidade>("SELECT * FROM EFD_USUARIO_CONTRIBUINTE WHERE OID_USUARIO=:OID_USUARIO", new { OID_USUARIO });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual UsuarioContribuinteEntidade BuscarPorOidUsuarioOidContribuinte(decimal OID_USUARIO, decimal OID_CONTRIBUINTE)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.QuerySingleOrDefault<UsuarioContribuinteEntidade>("SELECT * FROM EFD_USUARIO_CONTRIBUINTE WHERE OID_USUARIO = @OID_USUARIO   AND OID_CONTRIBUINTE = @OID_CONTRIBUINTE", new { OID_USUARIO, OID_CONTRIBUINTE });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.QuerySingleOrDefault<UsuarioContribuinteEntidade>("SELECT * FROM EFD_USUARIO_CONTRIBUINTE WHERE OID_USUARIO=:OID_USUARIO AND OID_CONTRIBUINTE=:OID_CONTRIBUINTE", new { OID_USUARIO, OID_CONTRIBUINTE });
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
