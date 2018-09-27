#region Usings
using Intech.EfdReinf.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System; 
#endregion

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DominioController : BaseController
    {
        [HttpGet("porCodigo/{codDominio}")]
        [Authorize("Bearer")]
        public IActionResult Listar(string codDominio)
        {
            try
            {
                return Json(new DominioProxy().BuscarPorCodigo(codDominio));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}