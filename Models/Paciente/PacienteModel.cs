using Newtonsoft.Json;
using ANI.Models.Pessoa;

namespace ANI.Models.Paciente
{
    public class PacienteModel : PessoaModel
    {
        public string? Email { get; set; } //Email do usuario
    }

    public class PacienteListagemModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string PacienteNome => Nome + " " + Sobrenome;
        [JsonIgnore]
        public DateTime? UltimaConsultaDataBD { get; set; }
        public string UltimaConsultaData => UltimaConsultaDataBD.HasValue ? UltimaConsultaDataBD.Value.ToString("dd/MM/yyyy HH:mm") : "";
        [JsonIgnore]
        public DateTime? ProximaConsultaDataBD { get; set; }
        public string ProximaConsultaData => ProximaConsultaDataBD.HasValue ? ProximaConsultaDataBD.Value.ToString("dd/MM/yyyy HH:mm") : "";
        public string? Email { get; set; } //Email do usuario
    }
}
