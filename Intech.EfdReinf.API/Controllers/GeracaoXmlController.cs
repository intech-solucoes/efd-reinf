using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio;
using Intech.EfdReinf.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
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
        public ActionResult GerarR2010(decimal oidContribuinte, string tipoOperacao, string tipoAmbiente, string dtaInicial, string dtaFinal)
        {
            var dtaIni = DateTime.ParseExact(dtaInicial, "dd.MM.yyyy", new CultureInfo("pt-BR"));
            var dtaFim = DateTime.ParseExact(dtaFinal, "dd.MM.yyyy", new CultureInfo("pt-BR"));

            try
            {                
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, _folderName);

                new GeradorXml().GerarR2010(OidUsuario, oidContribuinte, tipoOperacao, tipoAmbiente, dtaIni, dtaFim, newPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerarR2098/{oidContribuinte}/{tipoAmbiente}/{ano}/{mes}")]
        [Authorize("Bearer")]
        public ActionResult GerarR2098(decimal oidContribuinte, string tipoAmbiente, int ano, int mes)
        {
            try
            {
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, _folderName);

                new GeradorXml().GerarR2098(OidUsuario, oidContribuinte, tipoAmbiente, ano, mes, newPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerarR2099/{oidContribuinte}/{r2099}")]
        [Authorize("Bearer")]
        public ActionResult GerarR2099(decimal oidContribuinte, R2099Entidade r2099)
        {
            try
            {
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, _folderName);

                new GeradorXml().GerarR2099(OidUsuario, oidContribuinte, r2099, newPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        
    }
}