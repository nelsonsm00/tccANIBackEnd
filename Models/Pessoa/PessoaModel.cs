using Newtonsoft.Json;
using ANI.Arquitetura;

namespace ANI.Models.Pessoa
{
    public class PessoaModel : ModelBase
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public char? Sexo { get; set; }
        public string SexoDescricao => !Sexo.HasValue ? "Não definido" : Sexo == 'F' ? "Feminino" : Sexo == 'M' ? "Masculino" : "Outros";
        public string? Cpf { get; set; }
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Bairro { get; set; }
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public int? Ddd { get; set; }
        public int? Telefone { get; set; }
        [JsonIgnore]
        public DateTime? DataNascimentoBD {get; set; }
        public string? DataNascimento => DataNascimentoBD.HasValue ? DataNascimentoBD.Value.ToString("dd/MM/yyyy") : null;
        public int? Idade { get; set; }
        public string? Profissao { get; set; }
        public int? CargaHoraria { get; set; }
        public string? EstadoCivil { get; set; }
        public decimal? Altura { get; set; }
    }
}
