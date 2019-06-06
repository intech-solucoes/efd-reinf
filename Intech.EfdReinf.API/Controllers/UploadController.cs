#region Usings
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Relatorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
#endregion

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        private IHostingEnvironment HostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{oidArquivoUpload}")]
        [Authorize("Bearer")]
        public IActionResult Buscar(decimal oidArquivoUpload)
        {
            try
            {
                return Json(new ArquivoUploadProxy().BuscarPorChave(oidArquivoUpload));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("porOidUsuarioContribuinte/{oidUsuarioContribuinte}")]
        [Authorize("Bearer")]
        public IActionResult BuscarPorOidUsuarioContribuinte(decimal oidUsuarioContribuinte)
        {
            try
            {
                return Json(new ArquivoUploadProxy().BuscarPorOidUsuarioContribuinte(oidUsuarioContribuinte));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CsvPorOidUsuarioContribuinte/{oidUsuarioContribuinte}/{status}")]
        [Authorize("Bearer")]
        public IActionResult Buscar(decimal oidUsuarioContribuinte, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status) || status == "null")
                    status = null;

                var arquivosCSV = new ArquivoUploadProxy().BuscarPorOidUsuarioContribuinteStatus(oidUsuarioContribuinte, status)
                    .Where(x => x.NOM_EXT_ARQUIVO.ToUpper().Replace(".", "") == "CSV")
                    .ToList();

                return Json(arquivosCSV);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{oidUsuarioContribuinte}"), DisableRequestSizeLimit]
        [Authorize("Bearer")]
        public IActionResult UploadFile(IFormFile file, decimal oidUsuarioContribuinte)
        {
            try
            {
                string folderName = "Upload";
                string webRootPath = HostingEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                long oidArquivoUpload = 0;

                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var filePathArray = fileName.Split(".");
                    var ext = filePathArray[filePathArray.Length - 1];

                    if (ext.ToLower() != "csv")
                        throw new Exception("Formato de arquivo inválido. Apenas arquivos .csv são suportados.");

                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var arquivo = new ArquivoUploadEntidade
                    {
                        OID_USUARIO_CONTRIBUINTE = oidUsuarioContribuinte,
                        DTA_UPLOAD = DateTime.Now,
                        IND_STATUS = DMN_STATUS_EFD_UPLOAD.NAO_PROCESSADO,
                        NOM_ARQUIVO_LOCAL = string.Format(@"Upload\{0}", fileName),
                        NOM_ARQUIVO_ORIGINAL = fileName,
                        NOM_DIRETORIO_LOCAL = "Upload",
                        NOM_EXT_ARQUIVO = ext
                    };

                    oidArquivoUpload = new ArquivoUploadProxy().Inserir(arquivo);
                }

                return Json(oidArquivoUpload);
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        [HttpDelete("{oidArquivoUpload}")]
        [Authorize("Bearer")]
        public IActionResult Deletar(decimal oidArquivoUpload)
        {
            try
            {
                var proxy = new ArquivoUploadProxy();
                var arquivoUpload = proxy.BuscarPorChave(oidArquivoUpload);
                proxy.Deletar(arquivoUpload);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("relatorio/{oidArquivoUpload}")]
        [Authorize("Bearer")]
        public IActionResult Relatorio(decimal oidArquivoUpload)
        {
            try
            {
                var parametros = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("OID_ARQUIVO_UPLOAD", oidArquivoUpload)
                };

                var relatorio = GeradorRelatorio.Gerar("RelatorioCriticasImportacao", HostingEnvironment.ContentRootPath, parametros);

                return File(relatorio, "application/pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}