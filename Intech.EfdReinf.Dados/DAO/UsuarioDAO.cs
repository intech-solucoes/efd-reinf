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
    public abstract class UsuarioDAO : BaseDAO<UsuarioEntidade>
    {
        
		public virtual UsuarioEntidade BuscarPorEmail(string TXT_EMAIL)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.QuerySingleOrDefault<UsuarioEntidade>("SELECT * FROM EFD_USUARIO WHERE TXT_EMAIL = @TXT_EMAIL", new { TXT_EMAIL });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.QuerySingleOrDefault<UsuarioEntidade>("SELECT * FROM EFD_USUARIO WHERE TXT_EMAIL=:TXT_EMAIL", new { TXT_EMAIL });
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
