using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Formulario
{
    public class FormularioCategoriaModel : ModelBase
    {
        public int Id { get; set; }
        public int Formulario { get; set; }
        public string Descricao { get; set; }
        [JsonIgnore]
        public string? AtivoBD { get; set; }
        public bool Ativo => AtivoBD == "S";
    }

    public class FormularioCategoriaCompletoModel : FormularioCategoriaModel
    {
        public List<FormularioItemModel> Itens { get; set; }
    }

    public class FormularioCategoriaCompletoRespondidoModel : FormularioCategoriaModel
    {
        public List<FormularioItemCompletoRespondidoModel> Itens { get; set; }
    }
}
