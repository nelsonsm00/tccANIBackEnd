namespace ANI.Models.Alimento.TCA
{
    public class AlimentoTBCAFiltroModel : AlimentoTCAFiltroModel
    {
        public string? guarda { get; set; }
        public string? produto { get; set; }
        public string? cmb_grupo {get;set;}
        public string? cmb_tipo_alimento { get; set; }        
        public string? pagina { get; set; }
        public string? atuald { get; set; }
        override public string ToString()
        {
            return "guarda=" + this.guarda + "&produto=" + this.produto + "&cmb_grupo=" + this.cmb_grupo + "&cmb_tipo_alimento=" + this.cmb_tipo_alimento + 
                (pagina != null ? "&pagina=" + pagina + "&atuald=" + atuald : ""); 
        }
    }
}
