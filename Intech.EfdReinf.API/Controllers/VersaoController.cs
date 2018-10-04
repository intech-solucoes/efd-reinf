using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class VersaoController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var version = Assembly.GetExecutingAssembly().GetName();
            return Json(version.Version.ToString(3));
        }

        [HttpGet("validarToken")]
        [Authorize("Bearer")]
        public IActionResult ValidarToken()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}