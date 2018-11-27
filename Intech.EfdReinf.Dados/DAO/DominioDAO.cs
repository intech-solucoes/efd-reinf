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
    public abstract class DominioDAO : BaseDAO<DominioEntidade>
    {
        
		public virtual IEnumerable<DominioEntidade> BuscarPorCodigo(string COD_DOMINIO)
		{
			try
			{
				if(AppSettings.IS_SQL_SERVER_PROVIDER)
					return Conexao.Query<DominioEntidade>("SELECT * FROM TBG_DOMINIO WHERE COD_DOMINIO = @COD_DOMINIO", new { COD_DOMINIO });
				else if(AppSettings.IS_ORACLE_PROVIDER)
					return Conexao.Query<DominioEntidade>("SELECT * FROM TBG_DOMINIO WHERE COD_DOMINIO=:COD_DOMINIO", new { COD_DOMINIO });
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
