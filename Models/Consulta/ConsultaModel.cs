using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Consulta
{
    public class ConsultaModel : ModelBase
    {
        public int Id { get; set; }
        public int Tratamento { get; set; }
        public DateTime? DataBD { get; set; }
        public string Data => DataBD.HasValue ? DataBD.Value.ToString("dd/MM/yyyy") : null;
        public string Hora => DataBD.HasValue ? DataBD.Value.ToString("HH:mm") : null;
        public decimal? Peso { get; set; }
        [JsonIgnore]
        public char PrimeiraConsultaBD { get; set; }
        public bool PrimeiraConsulta => PrimeiraConsultaBD == 'S';
        public string? Observacao { get; set; }
        [JsonIgnore]
        public char RealizadaBD { get; set; }
        public bool Realizada => RealizadaBD == 'S';

    }

    public class ConsultaDetalheModel : ModelBase
    {
        public int Id { get; set; }
        public int Tratamento { get; set; }
        [JsonIgnore]
        public DateTime DataBD { get; set; }
        public string Data => DataBD.ToString("dd/MM/yyyy");
        public string Hora => DataBD.ToString("HH:mm");
        public decimal? Peso { get; set; }
        public decimal? PesoAnterior { get; set; }
        public string? Observacao { get; set; }
        public int? Altura { get; set; }
        public string? Sexo { get; set; }
        [JsonIgnore]
        public DateTime? DataNascimento { get; set; }
        public int? Idade { get; set; }

    }

    public class ConsultaResumoModel : ModelBase
    {
        public int Id { get; set; }
        public int Tratamento { get; set; }
        [JsonIgnore]
        public DateTime DataBD { get; set; }
        public string Data => DataBD.ToString("dd/MM/yyyy");
        public string Hora => DataBD.ToString("HH:mm");
        public decimal? Peso { get; set; }
        public string Observacao { get; set; }
    }

    public class ConsultaRealizadaModel : ModelBase
    {
        public int Id { get; set; }
        public string Observacao { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
    }

    public class ConsultaReagendaModel : ModelBase
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
    }
}
