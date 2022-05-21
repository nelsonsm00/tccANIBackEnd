using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Usuario;
using ANI.Modulos.Usuario;

namespace ANI.Controllers
{    
    [Route("Usuario")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        
    }
}