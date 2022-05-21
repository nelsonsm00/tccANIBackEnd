using ANI.Arquitetura;

namespace ANI.Models.Formulario
{
    public class FormularioRespostaModel : ModelBase
    {
        public int Tratamento { get; set; }
        public int FormularioItem { get; set; }
        public string? Valor { get; set; }
    }
}
