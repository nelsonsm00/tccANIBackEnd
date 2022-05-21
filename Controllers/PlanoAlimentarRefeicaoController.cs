using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Plano_Alimentar;
using ANI.Modulos.Plano_Alimentar;

namespace ANI.Controllers
{    
    [Route("PlanoAlimentar/Refeicao")]
    [ApiController]
    [Authorize]
    public class PlanoAlimentarRefeicaoController : ControllerBase
    {
        PlanoAlimentarRefeicaoModulo modulo = new PlanoAlimentarRefeicaoModulo();    

        /// <summary>
        /// Insere um vinculo entre o plano alimentar e a refeição
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("")]
        public ActionResult<int> Post(PlanoAlimentarRefeicaoModel pRegistro)
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
        /// Exclui o vinculo do da refeição ao plano alimentar.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("DELETE")]
        [Route("{PlanoAlimentar}/{Refeicao}")]
        public ActionResult<bool> Delete(int PlanoAlimentar, int Refeicao)
        {
            try
            {
                modulo.Delete(PlanoAlimentar, Refeicao);
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