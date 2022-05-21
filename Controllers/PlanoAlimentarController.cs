using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Plano_Alimentar;
using ANI.Modulos.Plano_Alimentar;

namespace ANI.Controllers
{    
    [Route("PlanoAlimentar")]
    [ApiController]
    [Authorize]
    public class PlanoAlimentarController : ControllerBase
    {
        PlanoAlimentarModulo modulo = new PlanoAlimentarModulo();
        
        /// <summary>
        /// Retorna os planos alimentares.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlanoAlimentarModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/{Tratamento}")]        
        public ActionResult<List<PlanoAlimentarModel>> GetLista(int Tratamento)
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
        /// Retorna os planos alimentares que não estão vinculados ao tratamento para serem importados.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlanoAlimentarModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/NaoVinculadas/{Tratamento}")]
        public ActionResult<List<PlanoAlimentarModel>> GetListaNaoVinculadas(int Tratamento)
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
        /// Retorna as refeições do plano alimentar em detalhes
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlanoAlimentarDetalheModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Id}")]
        public ActionResult<PlanoAlimentarDetalheModel> Get(int Id)
        {
            try
            {
                var result = modulo.GetDetalhe(Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Insere um plano alimentar vinculando ao tratamento.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("{Tratamento}")]
        public ActionResult<int> Post(PlanoAlimentarModel pRegistro, int Tratamento)
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
        /// Importa um plano alimentar
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("{Id}/{Tratamento}")]
        public ActionResult<bool> Post(int Id, int Tratamento)
        {
            try
            {
                modulo.Importa(Id, Tratamento);
                return Ok(true);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Atualiza a descrição de um plano alimentar.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("")]
        public ActionResult<bool> Put(PlanoAlimentarModel pRegistro)
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
        /// Exclui o plano alimentar.
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