using System.Collections.Generic;
using Intech.EfdReinf.Dados.DAO;
using Intech.EfdReinf.Entidades;

namespace Intech.EfdReinf.Negocio.Proxy
{
    public class UsuarioContribuinteProxy : UsuarioContribuinteDAO
    {
        public override IEnumerable<UsuarioContribuinteEntidade> BuscarPorOidContribuinte(decimal OID_CONTRIBUINTE)
        {
            var lista = base.BuscarPorOidContribuinte(OID_CONTRIBUINTE);

            foreach(var usuarioContribuinte in lista)
            {
                usuarioContribuinte.Usuario = new UsuarioProxy().BuscarPorChave(usuarioContribuinte.OID_USUARIO);
            }

            return lista;
        }
    }
}
