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
    public abstract class ContribuinteDAO : BaseDAO<ContribuinteEntidade>
    {
        
		public virtual IEnumerable<ContribuinteEntidade> BuscarAtivosPorOidUsuario(decimal OID_USUARIO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE = EFD_CONTRIBUINTE.OID_CONTRIBUINTE WHERE EFD_USUARIO_CONTRIBUINTE.OID_USUARIO = @OID_USUARIO   AND EFD_CONTRIBUINTE.IND_APROVADO = 'SIM';", new { OID_USUARIO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE=EFD_CONTRIBUINTE.OID_CONTRIBUINTE WHERE EFD_USUARIO_CONTRIBUINTE.OID_USUARIO=:OID_USUARIO AND EFD_CONTRIBUINTE.IND_APROVADO='SIM'", new { OID_USUARIO });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual ContribuinteEntidade BuscarPorCpfCnpj(string COD_CNPJ_CPF)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.QuerySingleOrDefault<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE WHERE COD_CNPJ_CPF = @COD_CNPJ_CPF", new { COD_CNPJ_CPF });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.QuerySingleOrDefault<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE WHERE COD_CNPJ_CPF=:COD_CNPJ_CPF", new { COD_CNPJ_CPF });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual ContribuinteEntidade BuscarPorCpfCnpjOidUsuario(string COD_CNPJ_CPF, decimal OID_USUARIO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.QuerySingleOrDefault<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE = EFD_CONTRIBUINTE.OID_CONTRIBUINTE WHERE EFD_CONTRIBUINTE.COD_CNPJ_CPF = @COD_CNPJ_CPF   AND EFD_USUARIO_CONTRIBUINTE.OID_USUARIO = @OID_USUARIO", new { COD_CNPJ_CPF, OID_USUARIO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.QuerySingleOrDefault<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE=EFD_CONTRIBUINTE.OID_CONTRIBUINTE WHERE EFD_CONTRIBUINTE.COD_CNPJ_CPF=:COD_CNPJ_CPF AND EFD_USUARIO_CONTRIBUINTE.OID_USUARIO=:OID_USUARIO", new { COD_CNPJ_CPF, OID_USUARIO });
				else
					throw new Exception("Provider não suportado!");
			}
			finally
			{
				Conexao.Close();
			}
		}

		public virtual IEnumerable<ContribuinteEntidade> BuscarPorOidUsuario(decimal OID_USUARIO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE INNER JOIN EFD_USUARIO_CONTRIBUINTE ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE = EFD_CONTRIBUINTE.OID_CONTRIBUINTE WHERE EFD_USUARIO_CONTRIBUINTE.OID_USUARIO = @OID_USUARIO", new { OID_USUARIO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<ContribuinteEntidade>("SELECT * FROM EFD_CONTRIBUINTE INNER  JOIN EFD_USUARIO_CONTRIBUINTE  ON EFD_USUARIO_CONTRIBUINTE.OID_CONTRIBUINTE=EFD_CONTRIBUINTE.OID_CONTRIBUINTE WHERE EFD_USUARIO_CONTRIBUINTE.OID_USUARIO=:OID_USUARIO", new { OID_USUARIO });
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
