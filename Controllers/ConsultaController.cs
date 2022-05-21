using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Consulta;
using ANI.Modulos.Consulta;

namespace ANI.Controllers
{    
    [Route("Consulta")]
    [ApiController]
    [Authorize]
    public class ConsultaController : ControllerBase
    {
        ConsultaModulo modulo = new ConsultaModulo();
        
        /// <summary>
        /// Retorna os dados da consulta.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConsultaDetalheModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Id}")]
        public ActionResult<ConsultaDetalheModel> Get(int Id)
        {
            try
            {
                var result = modulo.GetConsultaNaoRealizada(0, Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna os dados da consulta não realizada.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConsultaDetalheModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Atual/{Tratamento}")]
        public ActionResult<ConsultaDetalheModel> GetConsultaNaoRealizada(int Tratamento)
        {
            try
            {
                var result = modulo.GetConsultaNaoRealizada(Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna uma lista das consultas não realizadas.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ConsultaDetalheModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Atual/Lista/{Tratamento}")]
        public ActionResult<List<ConsultaDetalheModel>> GetListaConsultaNaoRealizada(int Tratamento)
        {
            try
            {
                var result = modulo.GetListaConsultaNaoRealizada(Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna os dados das consultas realizadas.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ConsultaResumoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Realizadas/{Tratamento}")]
        public ActionResult<List<ConsultaResumoModel>> GetListaConsultaRealizada(int Tratamento)
        {
            try
            {
                var result = modulo.GetListaConsultaRealizada(Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Insere uma consulta.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("")]
        public ActionResult<int> Post(ConsultaModel pRegistro)
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
        /// Indica que a consulta foi realizada, atualizando o peso e observação.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("Realiza")]
        public ActionResult<bool> Realiza(ConsultaRealizadaModel pRegistro)
        {
            try
            {
                modulo.Realiza(pRegistro);
                return Ok(true);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Reagenda uma consulta.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("Reagenda")]
        public ActionResult<bool> Reagenda(ConsultaReagendaModel pRegistro)
        {
            try
            {
                modulo.Reagenda(pRegistro);
                return Ok(true);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Deleta a consulta.
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