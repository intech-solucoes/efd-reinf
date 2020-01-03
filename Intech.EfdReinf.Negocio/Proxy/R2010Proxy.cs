using System;
using System.Collections.Generic;
using Intech.EfdReinf.Dados.DAO;

namespace Intech.EfdReinf.Negocio.Proxy
{
    public class R2010Proxy : R2010DAO
    {
        public override IEnumerable<string> BuscarPrestadores(decimal OID_CONTRIBUINTE)
        {
            var prestadores = base.BuscarPrestadores(OID_CONTRIBUINTE);
            var retorno = new List<string>();

            foreach (var prestador in prestadores)
                retorno.Add(prestador.AplicarMascara(Mascaras.CNPJ));

            return retorno;
        }
    }
}