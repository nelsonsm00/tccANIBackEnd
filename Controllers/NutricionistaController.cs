using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Nutricionista;
using ANI.Modulos.Nutricionista;

namespace ANI.Controllers
{    
    [Route("Nutricionista")]
    [ApiController]
    [Authorize]
    public class NutricionistaController : ControllerBase
    {
        NutricionistaModulo modulo = new NutricionistaModulo();
        
        /// <summary>
        /// Retorna os dados do nutricionista.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NutricionistaModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Id}")]        
        public ActionResult<NutricionistaModel> Get(int Id)
        {
            try
            {
                var result = modulo.Get(Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Insere um nutricionista.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("")]
        public ActionResult<int> Post(NutricionistaModel pRegistro)
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
        /// Atualiza um nutricionista.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("")]
        public ActionResult<bool> Put(NutricionistaModel pRegistro)
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
    }
}