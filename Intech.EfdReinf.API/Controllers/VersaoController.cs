using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/")]
    public class VersaoController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var version = Assembly.GetExecutingAssembly().GetName();
            return Json(version.Version.ToString(3));
        }
    }
}