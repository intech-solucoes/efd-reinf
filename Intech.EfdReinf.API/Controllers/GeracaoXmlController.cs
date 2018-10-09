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
        private const string _folderName = "Upload";

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
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, _folderName);

                new GeradorXml().GerarR1000(OidUsuario, oidContribuinte, tipoAmbiente, newPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerarR2010/{oidContribuinte}/{tipoOperacao}/{tipoAmbiente}/{dtaInicial}/{dtaFinal}")]
        [Authorize("Bearer")]
        public ActionResult GerarR2010(decimal oidContribuinte, string tipoOperacao, string tipoAmbiente, DateTime dtaInicial, DateTime dtaFinal)
        {
            try
            {                
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, _folderName);

                new GeradorXml().GerarR2010(OidUsuario, oidContribuinte, tipoOperacao, tipoAmbiente, dtaInicial, dtaFinal, newPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}