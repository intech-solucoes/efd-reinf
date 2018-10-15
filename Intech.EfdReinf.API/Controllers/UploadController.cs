#region Usings
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Util.Relatorios;
using Intech.Lib.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
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

        [HttpGet("{oidUsuarioContribuinte}")]
        public IActionResult Buscar(decimal oidUsuarioContribuinte)
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

        [HttpGet("{oidUsuarioContribuinte}/{status}")]
        public IActionResult Buscar(decimal oidUsuarioContribuinte, string status)
        {
            try
            {
                return Json(new ArquivoUploadProxy().BuscarPorOidUsuarioContribuinteStatus(oidUsuarioContribuinte, status));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{oidUsuarioContribuinte}"), DisableRequestSizeLimit]
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

        [HttpGet("relatorio")]
        public IActionResult Relatorio(decimal oidArquivoUpload)
        {
            try
            {
                var parametros = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("OID_ARQUIVO_UPLOAD", 1)
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