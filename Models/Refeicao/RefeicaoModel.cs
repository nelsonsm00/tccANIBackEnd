using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Refeicao
{
    public class RefeicaoModel : ModelBase
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        [JsonIgnore]
        public DateTime? HorarioBD { get; set; }
        public string? Horario => HorarioBD.HasValue ? HorarioBD.Value.ToString("HH:mm") : null;
        [JsonIgnore]
        public string? SelecionadoBD { get; set; }
        public bool Selecionado => SelecionadoBD == "S";
    }
}
