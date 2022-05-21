using HtmlAgilityPack;
using System.Net;
using ANI.Arquitetura;
using ANI.Models.Alimento;
using ANI.Models.Alimento.TCA;

namespace ANI.Modulos.Alimento.TCA
{
    public class TBCA : TCAInterface
    {
        private const int POSICAO_CODIGO = 0;
        private const int POSICAO_NOME = 1;
        private const int POSICAO_GRUPO = 4;

        private const int POSICAO_COMPONENTE = 0;
        private const int POSICAO_UNIDADE = 1;
        private const int POSICAO_VALOR = 2;

        private const string TCA = "TBCA";

        public TBCA() { }

        #region ETL Geral
        public string GetDataBase(string? compl = null, bool lista = true)
        {
            return $@"http://www.tbca.net.br/base-dados/{(lista ? "" : "int_")}composicao_alimentos.php?{compl ?? ""}";
        }

        public List<string[]> GetInfo(string url, string nodeInfo, HtmlDocument? doc = null, bool splitTd = true)
        {
            List<string[]> data = new List<string[]>();
            using (WebClient client = new WebClient())
            {
                if (doc == null) 
                {
                    doc = new HtmlDocument();
                    string html = client.DownloadString(url);
                    doc.LoadHtml(html);
                }

                var htmlNodes = doc.DocumentNode.SelectNodes(nodeInfo);
                if (htmlNodes != null)
                {
                    foreach (var node in htmlNodes)
                    {
                        if (node != null)
                        {
                            if (splitTd)
                                data.Add(GerenciadorHTML.splitTD(node));
                            else
                                data.Add(GerenciadorHTML.splitTH(node));
                        }
                    }
                }
            }
            return data;
        }

        public HtmlDocument LoadDocumento(string url)
        {
            HtmlDocument doc = new HtmlDocument();
            using (WebClient client = new WebClient())
            {
                string html = client.DownloadString(url);
                doc.LoadHtml(html);
            }
            return doc;
        }
        #endregion

        public string GetNodeInfo()
        {
            return "//html//body//div//main//div//table//tbody//tr";
        }

        
        

        public List<string[]> Extract(AlimentoTCAFiltroModel f)
        {
            AlimentoTBCAFiltroModel filtro = (AlimentoTBCAFiltroModel)f;
            filtro.guarda = "tomo1";

            //Primeira pagina
            List<string[]> data = this.GetInfo(this.GetDataBase(filtro.ToString()), this.GetNodeInfo());

            //Se encontrou resultado, procura nas próximas páginas
            if (data.Count > 0)
            {
                List<string[]> dataAux;
                int pagina = 1;
                int atualId = 1;
                do
                {                    
                    //Se já passou 10 páginas, aumenta o atualId para a próxima página
                    if (pagina % 10 == 0) atualId++;
                    filtro.atuald = Convert.ToString(atualId);
                    filtro.pagina = Convert.ToString(++pagina);

                    dataAux = this.GetInfo(this.GetDataBase(filtro.ToString()), this.GetNodeInfo());
                    data.AddRange(dataAux);
                } while (dataAux.Count > 0);
            }
            return data;
        }

        public List<AlimentoResumidoModel> Transform(List<string[]> data)
        {
            List<AlimentoResumidoModel> listaAlimento = new List<AlimentoResumidoModel>();
            foreach(string[] d in data)
            {
                if (d != null)
                {
                    listaAlimento.Add(new AlimentoResumidoModel()
                    {
                        Codigo = GerenciadorHTML.getInfoLink(d[POSICAO_CODIGO]),
                        Descricao = GerenciadorHTML.getInfoLink(d[POSICAO_NOME]),
                        Grupo = GerenciadorHTML.getInfoLink(d[POSICAO_GRUPO]),
                        TCA = TCA
                    });
                }
            }
            return listaAlimento;
        }

        public List<AlimentoResumidoModel> GetLista(AlimentoTCAFiltroModel filtro)
        {
            return this.Transform(this.Extract(filtro));
        }

        #region ETL Alimento específico
        public string GetNodeInfoUnidadeMedidaCaseira()
        {
            return "//html//body//div//main//div//table//thead//tr";
        }

        public string GetNodeInfoComponente()
        {
            return "//html//body//div//main//div//div//table//tbody//tr";
        }

        public List<string[]> ExtractAlimento(string pCodigo)
        {
            //Carrega o documento
            HtmlDocument doc = this.LoadDocumento(this.GetDataBase("cod_produto=" + pCodigo, false));

            //Recupera as unidades de medida caseira
            List<string[]> dataUnidadeMedidaCaseira = this.GetInfo(null, this.GetNodeInfoUnidadeMedidaCaseira(), doc, false);

            //Recupera as informações dos componentes
            List<string[]> data = this.GetInfo(null, this.GetNodeInfo(), doc);

            //Adiciona as unidades de medida caseira na primeira posicao da lista e retorna
            dataUnidadeMedidaCaseira.AddRange(data);
            return dataUnidadeMedidaCaseira;
        }

        public List<ComponenteAlimentoModel> TransformAlimento(List<string[]> data)
        {
            List<ComponenteAlimentoModel> componentes = new List<ComponenteAlimentoModel>();
            if (data != null && data.Count > 0)
            {                
                List<UnidadeMedidaCaseiraAlimentoModel> unidadesMedidasCaseira = new List<UnidadeMedidaCaseiraAlimentoModel>();
                //A primeira posicao são as unidades de medida caseira
                //A partir da posição 3 do array é que existe as unidades de medida caseira
                for (int i = 3; i < data[0].Length; i++)
                {
                    string unidade = data[0][i];

                    //Extrai a descricao da unidade
                    //)g 9( asar apos rehloc
                    string descricaoUnidade = new string(unidade.Reverse().ToArray());
                    int posParentesesPeso = descricaoUnidade.IndexOf('('); 
                    if (posParentesesPeso > -1)
                    {
                        descricaoUnidade = descricaoUnidade.Substring(posParentesesPeso + 2);
                    }
                    descricaoUnidade = new string(descricaoUnidade.Reverse().ToArray());

                    //(9 g)
                    string peso = unidade.Replace(descricaoUnidade, "");
                    if (peso[0] == ' ') peso = peso.Substring(1);
                    int posEspacoUnidadeMedida = peso.IndexOf(' ');
                    string unidadeMedida = peso.Substring(posEspacoUnidadeMedida + 1).Replace(')', ' ').Trim();
                    peso = peso.Substring(0, posEspacoUnidadeMedida).Replace('(', ' ').Trim();

                    UnidadeMedidaCaseiraAlimentoModel un = new UnidadeMedidaCaseiraAlimentoModel()
                    {
                        UnidadeMedidaCaseira = descricaoUnidade,
                        UnidadeMedida = unidadeMedida,
                        Peso = Convert.ToDecimal(peso)
                    };
                    unidadesMedidasCaseira.Add(un);
                }

                //Começa a verificar a partir da segunda posição, que contém os componentes
                for (int i = 1; i < data.Count; i++)
                {
                    string[] d = data[i];
                    if (d != null)
                    {
                        ComponenteAlimentoModel componente = new ComponenteAlimentoModel()
                        {
                            Componente = d[POSICAO_COMPONENTE],
                            UnidadeMedida = d[POSICAO_UNIDADE],
                            Valor = d[POSICAO_VALOR]
                        };
                        componente.UnidadeMedidaCaseira = new List<UnidadeMedidaCaseiraAlimentoModel>();
                        for(int j = 0; j < unidadesMedidasCaseira.Count; j++)
                        {
                            componente.UnidadeMedidaCaseira.Add(new UnidadeMedidaCaseiraAlimentoModel()
                            {
                                UnidadeMedidaCaseira = unidadesMedidasCaseira[j].UnidadeMedidaCaseira,
                                UnidadeMedida = unidadesMedidasCaseira[j].UnidadeMedida,
                                Peso = unidadesMedidasCaseira[j].Peso,
                                Valor = d[POSICAO_VALOR + j + 1]
                            });
                        }
                        componentes.Add(componente);
                    }
                }                
            }
            return componentes;
        }

        public List<ComponenteAlimentoModel> Get(string pCodigo)
        {
            return this.TransformAlimento(this.ExtractAlimento(pCodigo));
        }
        #endregion
    }
}

