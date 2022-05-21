using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Alimento;
using ANI.Models.Alimento.TCA;
using ANI.Modulos.Alimento;
using ANI.Modulos.Alimento.TCA;

namespace ANI.Controllers
{    
    [Route("Alimento/TBCA")]
    [ApiController]
    [Authorize]
    public class TBCAController : ControllerBase
    {
        AlimentoModulo modulo = new AlimentoModulo(new TBCA());

        /// <summary>
        /// Retorna os alimentos da tabela TBCA pesquisados.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AlimentoResumidoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista")]
        public ActionResult<List<AlimentoTBCAModel>> GetLista(string pProduto)
        {
            try
            {
                AlimentoTBCAFiltroModel filtro = new AlimentoTBCAFiltroModel()
                {
                    produto = pProduto
                };
                var result = modulo.GetListaTCA(filtro);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna os dados do alimento da tabela TBCA.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ComponenteAlimentoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Codigo}")]
        public ActionResult<List<ComponenteAlimentoModel>> Get(string Codigo)
        {
            try
            {
                var result = modulo.GetTCA(Codigo);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }
    }
}