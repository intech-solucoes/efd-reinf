using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("{oidUsuarioContribuinte}"), DisableRequestSizeLimit]
        public IActionResult UploadFile(IFormFile file, decimal oidUsuarioContribuinte)
        {
            try
            {
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.ContentRootPath;
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
    }
}