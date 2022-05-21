using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Tratamento
{
    public class TratamentoModel : ModelBase
    {
        public int Id { get; set; }
        public int Nutricionista { get; set; }
        public int Paciente { get; set; }
        public string? Observacao { get; set; }
        public string? Motivo { get; set; }
        public string? Objetivo { get; set; }
    }

    public class TratamentoListagemModel
    {
        public int Id { get; set; }
        public string PacienteNome { get; set; }
        [JsonIgnore]
        public DateTime? UltimaConsultaDataBD { get; set; }
        public string UltimaConsultaData => UltimaConsultaDataBD.HasValue ? UltimaConsultaDataBD.Value.ToString("dd/MM/yyyy HH:mm") : "";
        [JsonIgnore]
        public DateTime? ProximaConsultaDataBD { get; set; }
        public string ProximaConsultaData => ProximaConsultaDataBD.HasValue ? ProximaConsultaDataBD.Value.ToString("dd/MM/yyyy HH:mm") : "";
    }

    public class TratamentoPlanoAlimentarModel : ModelBase
    {
        public int Tratamento { get; set; }
        public int PlanoAlimentar { get; set; }
    }
}
