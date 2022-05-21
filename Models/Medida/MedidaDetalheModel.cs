using ANI.Arquitetura;

namespace ANI.Models.Medida
{
    public class MedidaDetalheModel : ModelBase
    {
        public int Consulta { get; set; }
        public int TipoMedida { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }

    public class MedidaPostModel 
    {
        public int Consulta { get; set; }
        public int TipoMedida { get; set; }
        public decimal Valor { get; set; }
    }

    public class MedidaDetalheHistoricoModel : ModelBase
    {
        public int Consulta { get; set; }
        public int TipoMedida { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataBD { get; set; }
        public string Data => DataBD.ToString("dd/MM/yyyy");
    }    
}
