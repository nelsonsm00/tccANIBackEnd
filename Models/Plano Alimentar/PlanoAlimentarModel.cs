using ANI.Arquitetura;
using ANI.Models.Alimento;

namespace ANI.Models.Plano_Alimentar
{
    public class PlanoAlimentarModel : ModelBase
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }

    public class PlanoAlimentarRefeicaoModel : ModelBase
    {
        public int Id { get; set; }
        public int PlanoAlimentar { get; set; }
        public int Refeicao { get; set; }
    }

    public class PlanoAlimentarRefeicaoAlimentoModel : ModelBase
    {
        public int Id { get; set; }
        public int PlanoAlimentarRefeicao { get; set; }
        public string Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public AlimentoModel Alimento { get; set; }
    }

    public class PlanoAlimentarDetalheModel : PlanoAlimentarModel
    {
        public string NomePaciente { get; set; }
        public string SobrenomePaciente { get; set; }
        public List<PlanoAlimentarRefeicaoDetalheModel> Refeicoes { get; set; }
    }

    public class PlanoAlimentarRefeicaoDetalheModel
    {
        public int Id { get; set; }
        public int Refeicao { get; set; }
        public int PlanoAlimentar { get; set; }
        public string DescricaoRefeicao { get; set; }
        public List<PlanoAlimentarRefeicaoAlimentoDetalheModel> Alimentos { get; set; }
    }

    public class PlanoAlimentarRefeicaoAlimentoDetalheModel
    {
        public int Id { get; set; }
        public int PlanoAlimentarRefeicao { get; set; }
        public string Descricao { get; set; }
        public int Alimento { get; set; }
        public string Codigo { get; set; }
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; }
    }

}
