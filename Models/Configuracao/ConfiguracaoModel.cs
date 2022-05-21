using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Configuracao
{
    public class ConfiguracaoModel : ModelBase
    {
        public int Id { get; set; }
        public int Nutricionista { get; set; }
        [JsonIgnore]
        public DateTime? HorarioInicioBD { get; set; }
        public string HorarioInicio => HorarioInicioBD.HasValue ? HorarioInicioBD.Value.ToString("HH:mm") : "00:00";
        [JsonIgnore]
        public DateTime? HorarioFinalBD { get; set; }
        public string HorarioFinal => HorarioFinalBD.HasValue ? HorarioFinalBD.Value.ToString("HH:mm") : "23:59";
        public int? Duracao { get; set; }
    }

    public class HorarioModel
    {
        public string HorarioInicio { get; set; }
        public string HorarioFinal { get; set; }
    }
}
