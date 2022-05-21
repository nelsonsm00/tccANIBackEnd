using HtmlAgilityPack;
using ANI.Models.Alimento;
using ANI.Models.Alimento.TCA;

namespace ANI.Modulos.Alimento.TCA
{
    public interface TCAInterface
    {
        #region ETL Geral
        public string GetDataBase(string? compl = null, bool lista = true);

        public List<string[]> GetInfo(string url, string nodeInfo, HtmlDocument? doc = null, bool splitTd = true);

        public HtmlDocument LoadDocumento(string url);
        #endregion

        #region ETL Lista de alimentos
        public string GetNodeInfo();            

        public List<string[]> Extract(AlimentoTCAFiltroModel filtro);

        public List<AlimentoResumidoModel> Transform(List<string[]> data);

        public List<AlimentoResumidoModel> GetLista(AlimentoTCAFiltroModel filtro);
        #endregion

        #region ETL Alimento específico
        public string GetNodeInfoComponente();

        public List<string[]> ExtractAlimento(string pCodigo);

        public List<ComponenteAlimentoModel> TransformAlimento(List<string[]> data);

        public List<ComponenteAlimentoModel> Get(string pCodigo);
        #endregion
    }
}
