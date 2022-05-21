using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Plano_Alimentar;
using ANI.Modulos.Plano_Alimentar;

namespace ANI.Controllers
{    
    [Route("PlanoAlimentar/Refeicao/Alimento")]
    [ApiController]
    [Authorize]
    public class PlanoAlimentarRefeicaoAlimentoController : ControllerBase
    {
        PlanoAlimentarRefeicaoAlimentoModulo modulo = new PlanoAlimentarRefeicaoAlimentoModulo();    

        /// <summary>
        /// Insere um vinculo entre o alimento (se não existir na base ele cria) e a refeição/plano alimentar.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("")]
        public ActionResult<int> Post(PlanoAlimentarRefeicaoAlimentoModel pRegistro)
        {
            try
            {
                var result = modulo.Post(pRegistro);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Altera a quantidade e unidade de medida do alimento vinculado à refeição/plano alimentar.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("")]
        public ActionResult<bool> Put(PlanoAlimentarRefeicaoAlimentoModel pRegistro)
        {
            try
            {
                modulo.Put(pRegistro);
                return Ok(true);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Exclui o vinculo do alimento à refeição/plano alimentar.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("DELETE")]
        [Route("{Id}")]
        public ActionResult<bool> Delete(int Id)
        {
            try
            {
                modulo.Delete(Id);
                return Ok(true);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }
    }
}