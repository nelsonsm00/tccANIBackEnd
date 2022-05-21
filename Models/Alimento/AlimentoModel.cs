using ANI.Arquitetura;

namespace ANI.Models.Alimento
{
    public class AlimentoResumidoModel
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Grupo {get;set;}
        public string TCA { get; set; }
    }

    public class ComponenteAlimentoModel
    {
        public string Componente { get; set; }
        public string UnidadeMedida { get; set; }
        public string Valor { get; set; } //Precisa ser string, pois existem valores em forma de texto. Ex: NA, TR, ND...
        public List<UnidadeMedidaCaseiraAlimentoModel> UnidadeMedidaCaseira { get; set; }
    }

    public class UnidadeMedidaCaseiraAlimentoModel
    {
        public string UnidadeMedidaCaseira { get; set; }
        public decimal Peso { get; set; }
        public string UnidadeMedida { get; set; }
        public string Valor { get; set; } //Precisa ser string, pois existem valores em forma de texto. Ex: NA, TR, ND...
    }

    public class AlimentoModel : ModelBase
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
    }
}
