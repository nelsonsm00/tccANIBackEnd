using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Configuracao;
using ANI.Modulos.Configuracao;

namespace ANI.Controllers
{    
    [Route("Configuracao")]
    [ApiController]
    [Authorize]
    public class ConfiguracaoController : ControllerBase
    {
        ConfiguracaoModulo modulo = new ConfiguracaoModulo();
        
        /// <summary>
        /// Retorna os dados da configuração.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfiguracaoModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Nutricionista}")]
        public ActionResult<ConfiguracaoModel> Get(int Nutricionista)
        {
            try
            {
                var result = modulo.Get(Nutricionista);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna uma lista com os horários disponíveis.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Horario/Lista/{Nutricionista}")]
        public ActionResult<List<string>> GetHorarios(int Nutricionista)
        {
            try
            {
                var result = modulo.GetHorarios(Nutricionista);
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