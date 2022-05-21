using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Formulario
{
    public class FormularioItemModel : ModelBase
    {
        public int Id { get; set; }
        public int FormularioCategoria { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        [JsonIgnore]
        public string? AtivoBD { get; set; }
        public bool Ativo => AtivoBD == "S";
        [JsonIgnore]
        public string? AlternativasBD { get; set; }
        public string[]? Alternativas => AlternativasBD != null ? AlternativasBD.Split(';') : null;
    }

    public class FormularioItemCompletoRespondidoModel : FormularioItemModel
    {
        public FormularioRespostaModel Resposta { get; set; }
    }
}
