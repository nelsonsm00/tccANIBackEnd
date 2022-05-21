using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Paciente;
using ANI.Modulos.Paciente;

namespace ANI.Controllers
{    
    [Route("Paciente")]
    [ApiController]
    [Authorize]
    public class PacienteController : ControllerBase
    {
        PacienteModulo modulo = new PacienteModulo();
        
        /// <summary>
        /// Retorna os dados do paciente.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacienteModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("{Id}")]
        public ActionResult<PacienteModel> Get(int Id)
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
        /// Retorna os dados do paciente.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacienteModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Tratamento/{Tratamento}")]
        public ActionResult<PacienteModel> GetPeloTratamento(int Tratamento)
        {
            try
            {
                var result = modulo.Get(0, Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna os dados do paciente para a listagem.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PacienteListagemModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/{pNutricionista}")]
        public ActionResult<List<PacienteListagemModel>> GetList(int pNutricionista)
        {
            try
            {
                var result = modulo.GetList(pNutricionista);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Insere um paciente.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("")]
        public ActionResult<int> Post(PacienteModel pRegistro)
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
        /// Atualiza o nome e sobrenome de um paciente.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("")]
        public ActionResult<bool> Put(PacienteModel pRegistro)
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
        /// Deleta um paciente.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("DELETE")]
        [Route("{Id}")]
        public ActionResult<bool> Put(int Id)
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