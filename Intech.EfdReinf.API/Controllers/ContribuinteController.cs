#region Usings
using Intech.EfdReinf.Entidades;
using Intech.EfdReinf.Negocio.Proxy;
using Intech.Lib.Dominios;
using Intech.Lib.Dominios.EfdReinf;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize("Bearer")]
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

        [HttpGet("ativos")]
        [Authorize("Bearer")]
        public IActionResult ListarAtivos()
        {
            try
            {
                return Json(new ContribuinteProxy().BuscarAtivosPorOidUsuario(OidUsuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{oid}")]
        [Authorize("Bearer")]
        public IActionResult Buscar(decimal oid)
        {
            try
            {
                return Json(new ContribuinteProxy().BuscarPorChave(oid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize("Bearer")]
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

        [HttpPut]
        [Authorize("Bearer")]
        public IActionResult Atualizar([FromBody] ContribuinteEntidade contribuinte)
        {
            try
            {
                var proxyContribuinte = new ContribuinteProxy();
                proxyContribuinte.Atualizar(contribuinte);

                return Json(contribuinte);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("alterarAmbiente/{oidContribuinte}")]
        [Authorize("Bearer")]
        public IActionResult AlterarAmbiente(decimal oidContribuinte)
        {
            try
            {
                var contribuinteProxy = new ContribuinteProxy();
                var contribuinte = contribuinteProxy.BuscarPorChave(oidContribuinte);
                contribuinte.IND_TIPO_AMBIENTE = contribuinte.IND_TIPO_AMBIENTE == DMN_TIPO_AMBIENTE_EFD.PRODUCAO ? DMN_TIPO_AMBIENTE_EFD.PRODUCAO_RESTRITA : DMN_TIPO_AMBIENTE_EFD.PRODUCAO;

                contribuinteProxy.Atualizar(contribuinte);

                return Json(contribuinte);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}