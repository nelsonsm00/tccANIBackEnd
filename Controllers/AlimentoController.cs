using Microsoft.AspNetCore.Mvc;
using ANI.Arquitetura.Erro;
using ANI.Models.Erro;
using ANI.Models.Alimento;
using ANI.Modulos.Alimento;

namespace ANI.Controllers
{    
    [Route("Alimento")]
    [ApiController]
    public class AlimentoController : ControllerBase
    {
        AlimentoModulo modulo = new AlimentoModulo(null);
    }
}