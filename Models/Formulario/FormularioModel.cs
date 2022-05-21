using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Formulario
{
    public class FormularioModel : ModelBase
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        [JsonIgnore]
        public string Tipo { get; set; }
        public string TipoDescricao => Tipo == "A" ? "Anamnese" : "Outros";
    }

    public class FormularioCompletoModel : FormularioModel
    {
        public List<FormularioCategoriaCompletoModel> Categorias { get; set; }
    }

    public class FormularioCompletoRespondidoModel : FormularioModel
    {
        public List<FormularioCategoriaCompletoRespondidoModel> Categorias { get; set; }
    }
}
