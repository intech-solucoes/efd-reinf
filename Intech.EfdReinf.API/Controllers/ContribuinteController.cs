#region Usings
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Microsoft.AspNetCore.Mvc;
using System; 
#endregion

namespace Intech.EfdReinf.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContribuinteController : BaseController
    {
        [HttpGet]
        public IActionResult Listar()
        {
            try
            {
                return Json(new ContribuinteProxy().BuscarPorOidUsuario(OidUsuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Criar([FromBody] ContribuinteEntidade contribuinte)
        {
            try
            {
                var proxyContribuinte = new ContribuinteProxy();
                var oidContribuinteNovo = proxyContribuinte.Inserir(contribuinte, OidUsuario);
                var contribuinteNovo = proxyContribuinte.BuscarPorChave(oidContribuinteNovo);

                return Json(contribuinteNovo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}