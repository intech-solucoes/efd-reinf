using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

        [HttpGet("datas/{oidContribuinte}")]
        [Authorize("Bearer")]
        public IActionResult BuscarDatas(decimal oidContribuinte)
        {
            try
            {
                var datas = new R2010Proxy().BuscarDatasEnviados(oidContribuinte).ToList();

                var listaDatas = from data in datas
                                 group data by data.Year into g
                                 select new
                                 {
                                     Ano = g.Key,
                                     Meses = from mes in g
                                             group mes by mes.Month into g2
                                             select g2.Key
                                 };

                return Json(listaDatas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("arquivosGerados/{oidContribuinte}")]
        [Authorize("Bearer")]
        public IActionResult BuscarArquivos(decimal oidContribuinte)
        {
            try
            {
                var R1000 = new ContribuinteEnvioProxy().BuscarPorOidContribuinte(oidContribuinte);
                var R1070 = new R1070Proxy().BuscarPorOidContribuinte(oidContribuinte);
                var R2010 = new R2010Proxy().BuscarPorOidContribuinte(oidContribuinte);
                var R2098 = new R2098Proxy().BuscarPorOidContribuinte(oidContribuinte);
                var R2099 = new R2099Proxy().BuscarPorOidContribuinte(oidContribuinte);

                var listaArquivos = new List<ArquivoGerado>();

                // R-1000
                foreach(var item in R1000)
                {
                    listaArquivos.Add(new ArquivoGerado
                    {
                        Tipo = "R-1000",
                        DataGeracao = item.DTA_UPLOAD.Value,
                        Ambiente = item.IND_TIPO_AMBIENTE == DMN_TIPO_AMBIENTE_EFD.PRODUCAO ? "Produção" : "Pré-Produção",
                        Status = item.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO ? "Processado" : "Gerado",
                        Usuario = new UsuarioProxy().BuscarPorChave(item.OID_USUARIO_ENVIO).NOM_USUARIO,
                        OidArquivoUpload = item.OID_ARQUIVO_UPLOAD
                    });
                }

                // R-1070
                foreach(var item in R1070)
                {
                    listaArquivos.Add(new ArquivoGerado
                    {
                        Tipo = "R-1070",
                        DataGeracao = item.DTA_UPLOAD.Value,
                        Ambiente = item.IND_AMBIENTE_ENVIO == DMN_TIPO_AMBIENTE_EFD.PRODUCAO ? "Produção" : "Pré-Produção",
                        Status = item.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO ? "Processado" : "Gerado",
                        Usuario = new UsuarioProxy().BuscarPorChave(item.OID_USUARIO_ENVIO).NOM_USUARIO,
                        OidArquivoUpload = item.OID_ARQUIVO_UPLOAD
                    });
                }

                // R-2010
                foreach(var item in R2010)
                {
                    listaArquivos.Add(new ArquivoGerado
                    {
                        Tipo = "R-2010",
                        DataGeracao = item.DTA_UPLOAD.Value,
                        Ambiente = item.IND_AMBIENTE_ENVIO == DMN_TIPO_AMBIENTE_EFD.PRODUCAO ? "Produção" : "Pré-Produção",
                        Status = item.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO ? "Processado" : "Gerado",
                        Usuario = new UsuarioProxy().BuscarPorChave(item.OID_USUARIO_ENVIO).NOM_USUARIO,
                        OidArquivoUpload = item.OID_ARQUIVO_UPLOAD
                    });
                }

                // R-2098
                foreach (var item in R2098)
                {
                    listaArquivos.Add(new ArquivoGerado
                    {
                        Tipo = "R-2098",
                        DataGeracao = item.DTA_UPLOAD.Value,
                        Ambiente = item.IND_AMBIENTE_ENVIO == DMN_TIPO_AMBIENTE_EFD.PRODUCAO ? "Produção" : "Pré-Produção",
                        Status = item.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO ? "Processado" : "Gerado",
                        Usuario = new UsuarioProxy().BuscarPorChave(item.OID_USUARIO_ENVIO).NOM_USUARIO,
                        OidArquivoUpload = item.OID_ARQUIVO_UPLOAD
                    });
                }

                // R-2099
                foreach (var item in R2099)
                {
                    listaArquivos.Add(new ArquivoGerado
                    {
                        Tipo = "R-2099",
                        DataGeracao = item.DTA_UPLOAD.Value,
                        Ambiente = item.IND_AMBIENTE_ENVIO == DMN_TIPO_AMBIENTE_EFD.PRODUCAO ? "Produção" : "Pré-Produção",
                        Status = item.IND_STATUS == DMN_STATUS_EFD_UPLOAD.PROCESSADO ? "Processado" : "Gerado",
                        Usuario = new UsuarioProxy().BuscarPorChave(item.OID_USUARIO_ENVIO).NOM_USUARIO,
                        OidArquivoUpload = item.OID_ARQUIVO_UPLOAD
                    });
                }

                listaArquivos = listaArquivos.OrderByDescending(x => x.DataGeracao).ToList();

                return Json(listaArquivos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpGet("gerarR1070/{oidContribuinte}/{tipoAmbiente}")]
        [Authorize("Bearer")]
        public ActionResult GerarR1070(decimal oidContribuinte, string tipoAmbiente)
        {
            try
            {
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, _folderName);

                new GeradorXml().GerarR1070(OidUsuario, oidContribuinte, tipoAmbiente, newPath);

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

        [HttpPost("gerarR2099/{oidContribuinte}")]
        [Authorize("Bearer")]
        public ActionResult GerarR2099(decimal oidContribuinte, [FromBody] R2099Entidade r2099)
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

    public class ArquivoGerado
    {
        public string Tipo { get; set; }
        public DateTime DataGeracao { get; set; }
        public string Ambiente { get; set; }
        public string Status { get; set; }
        public string Usuario { get; set; }
        public decimal OidArquivoUpload { get; set; }
    }
}