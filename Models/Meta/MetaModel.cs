using Newtonsoft.Json;
using ANI.Arquitetura;
using ANI.Arquitetura.Enums;
using ANI.Arquitetura.Extensoes;

namespace ANI.Models.Meta
{
    public class MetaModel : ModelBase
    {
        public int Id { get; set; }
        public int Consulta { get; set; }
        public char TipoBD { get; set; }
        public TipoMetaEnum Tipo => TipoBD == TipoMetaEnum.Peso.ObterDescricao(true) ? TipoMetaEnum.Peso :
                                    TipoBD == TipoMetaEnum.Alimento.ObterDescricao(true) ? TipoMetaEnum.Alimento :
                                    TipoMetaEnum.Livre;
        public string TipoDescricao => TipoBD == TipoMetaEnum.Peso.ObterDescricao(true) ? "Peso" :
                                    TipoBD == TipoMetaEnum.Alimento.ObterDescricao(true) ? "Alimento" :
                                    "Livre";
        [JsonIgnore]
        public char ConcluidoBD { get; set; }
        public bool Concluido => ConcluidoBD == 'S';
        public decimal? ObjetivoValor { get; set; }
        public decimal? ResultadoValor { get; set; }
        public string? ObjetivoDescritivo { get; set; }
        public string? ResultadoDescritivo { get; set; }
        public char? AtingidoBD { get; set; }
        public bool? Atingido => AtingidoBD.HasValue ? AtingidoBD.Value  == 'S' : null;
        public override string ToString() 
        {
            return $@"Meta {TipoDescricao}: {Id} "; 
        }
    }
}
