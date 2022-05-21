using ANI.Arquitetura;

namespace ANI.Models.Orientacao
{
    public class OrientacaoModel : ModelBase
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string PublicoBD { get; set; }
        public bool Publico => PublicoBD == "S";
        public string PublicoDescricao => Publico ? "Sim" : "Não";
    }
}
