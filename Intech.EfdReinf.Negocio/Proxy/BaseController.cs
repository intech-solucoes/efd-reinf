using Microsoft.AspNetCore.Mvc;
using System;

namespace Intech.EfdReinf.Negocio.Proxy
{
    public class BaseController : Controller
    {
        public decimal OidUsuario => Convert.ToDecimal(User.Claims.GetValue("Oid"));
        public string EmailUsuario => User.Claims.GetValue("Email");
    }
}
