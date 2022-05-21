using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Orientacao;
using ANI.Modulos.Orientacao;

namespace ANI.Controllers
{    
    [Route("Orientacao")]
    [ApiController]
    [Authorize]
    public class OrientacaoController : ControllerBase
    {
        OrientacaoModulo modulo = new OrientacaoModulo();

        /// <summary>
        /// Retorna a orientação.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrientacaoModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Id}")]
        public ActionResult<OrientacaoModel> Get(int Id)
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
        /// Retorna as orientações.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrientacaoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/{Tratamento?}")]        
        public ActionResult<List<OrientacaoModel>> GetLista(int? Tratamento = null)
        {
            try
            {
                var result = modulo.GetList(Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna as orientações que não estão vinculadas ao tratamento.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrientacaoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/NaoVinculadas/{Tratamento}")]
        public ActionResult<List<OrientacaoModel>> GetListaNaoVinculadas(int Tratamento)
        {
            try
            {
                var result = modulo.GetList(Tratamento, true);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Insere uma orientação.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("{Tratamento?}")]
        public ActionResult<int> Post(OrientacaoModel pRegistro, int? Tratamento = null)
        {
            try
            {
                var result = modulo.Post(pRegistro, Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Insere uma orientação.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("Vincular/{Id}/{Tratamento}")]
        public ActionResult<bool> Vincular(int Id, int Tratamento)
        {
            try
            {
                modulo.Vincular(Id, Tratamento);
                return Ok(true);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Atualiza uma orientação.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("")]
        public ActionResult<bool> Put(OrientacaoModel pRegistro)
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
        /// Exclui uma orientação.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("DELETE")]
        [Route("{Id}/{Tratamento?}")]
        public ActionResult<bool> Delete(int Id, int? Tratamento = null)
        {
            try
            {
                modulo.Delete(Id, Tratamento);
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