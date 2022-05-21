using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Usuario;
using ANI.Modulos.Usuario;

namespace ANI.Controllers
{    
    [ApiController]
    public class Controller : ControllerBase
    {
        UsuarioModulo modulo = new UsuarioModulo();

        /// <summary>
        /// Realiza o login.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroModel))]
        [AcceptVerbs("POST")]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult<LoginResponseModel> Login(LoginRequestModel request)
        {
            try
            {
                var result = modulo.Login(request);
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