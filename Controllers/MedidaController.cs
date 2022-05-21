using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Medida;
using ANI.Modulos.Medida;

namespace ANI.Controllers
{    
    [Route("Medida")]
    [ApiController]
    [Authorize]
    public class MedidaController : ControllerBase
    {
        MedidaModulo modulo = new MedidaModulo();
        
        /// <summary>
        /// Retorna os dados das medidas
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MedidaDetalheModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/{Consulta}")]
        public ActionResult<List<MedidaDetalheModel>> GetList(int Consulta)
        {
            try
            {
                var result = modulo.GetList(Consulta);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Retorna os dados do histórico das medidas
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MedidaDetalheHistoricoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("GET")]
        [Route("Lista/Historico/{Tratamento}")]
        public ActionResult<List<MedidaDetalheHistoricoModel>> GetListHistorico(int Tratamento)
        {
            try
            {
                var result = modulo.GetListHistorico(Tratamento);
                return Ok(result);
            }
            catch (Exception e)
            {
                var erro = GerenciadorErro.RetornaMensagemDeErro(e);
                return StatusCode(erro.StatusCode.GetValueOrDefault(500), erro.Value);
            }
        }

        /// <summary>
        /// Atualiza as medidas
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("PUT")]
        [Route("")]
        public ActionResult<bool> Put(List<MedidaPostModel> pRegistros)
        {
            try
            {
                modulo.Put(pRegistros);
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