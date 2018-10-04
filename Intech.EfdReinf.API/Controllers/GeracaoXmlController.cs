using Intech.EfdReinf.Negocio;
using Intech.EfdReinf.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeracaoXmlController : BaseController
    {
        private IHostingEnvironment HostingEnvironment;

        public GeracaoXmlController(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        [HttpGet("gerarR1000/{oidContribuinte}/{tipoAmbiente}")]
        [Authorize("Bearer")]
        public ActionResult GerarR1000(decimal oidContribuinte, string tipoAmbiente)
        {
            try
            {
                string folderName = "Upload";
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                new GeradorXml().GerarR1000(OidUsuario, oidContribuinte, tipoAmbiente, newPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}