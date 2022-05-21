using ANI.Arquitetura;
using ANI.Arquitetura.Enums;
using ANI.Arquitetura.Erro;
using ANI.Arquitetura.Extensoes;
using ANI.Models.Meta;

namespace ANI.Modulos.Meta
{
    public class MetaModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_META";

        public MetaModulo() : this(null) { }

        public MetaModulo(DataBase? db) : base(db) { }

        public static string GetColunas(string? pAlias = null)
        {
            pAlias = pAlias == null ? "" : pAlias + ".";
            return $@"   {pAlias}ID Id,
                         {pAlias}CONSULTA Consulta,
                         {pAlias}TIPO TipoBD,
                         {pAlias}CONCLUIDA ConcluidaBD,
                         {pAlias}OBJETIVO_VALOR ObjetivoValor,
                         {pAlias}RESULTADO_VALOR ResultadoValor,
                         {pAlias}OBJETIVO_DESCRITIVO ObjetivoDescritivo,
                         {pAlias}RESULTADO_DESCRITIVO ResultadoDescritivo,
                         {pAlias}ATINGIU AtingiuBD";
        }

        public MetaModel Get(int pId)
        {
            return this.ExecuteQuery<MetaModel>($@" Select  {MetaModulo.GetColunas()}
                                                    From META 
                                                    Where ID = {pId}");
        }

        public List<MetaModel> GetList(int pConsulta, TipoMetaEnum? tipo = null)
        {
            string sLcCondicaoPorTipo = tipo == null ? string.Empty : $@" and TIPO = '{tipo.Value.ObterDescricao()}'";

            return this.ExecuteQueryList<MetaModel>($@"   Select  {MetaModulo.GetColunas()}
                                                            From META 
                                                            Where CONSULTA = {pConsulta}
                                                            {sLcCondicaoPorTipo}");
        }

        private int Post(MetaModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<MetaModel>($@" Insert into META
                                                (ID, CONTA, CONSULTA, TIPO, OBJETIVO_VALOR, OBJETIVO_DESCRITIVO)
                                               Values
                                                (@Id, {this.Conta}, @Consulta, @TipoBD, @ObjetivoValor, @ObjetivoDescritivo)", pRegistro);
            return pRegistro.Id;

        }

        private void Put(MetaModel pRegistro)
        {
            this.ExecuteCommand<MetaModel>($@" Update META Set
                                                RESULTADO_VALOR = @ResultadoValor, RESULTADO_DESCRITIVO = @ResultadoDescritivo, ATINGIDO = @AtingidoBD)
                                               Where ID = @Id", pRegistro);
        }

        private int InsereTipoPeso(MetaModel pRegistro)
        {
            if (pRegistro.ObjetivoValor.GetValueOrDefault(0) <= 0)
            {
                throw new ANIException(Mensagens.a0001, ANIException.CODIGO_CONDICAO_INVALIDA);
            }
            else
            {
                return this.Post(pRegistro);
            }
        }

        private int InsereTipoAlimento(MetaModel pRegistro)
        {
            if (pRegistro.ObjetivoValor.GetValueOrDefault(0) <= 0)
            {
                throw new ANIException(Mensagens.a0001, ANIException.CODIGO_CONDICAO_INVALIDA);
            }
            else
            {
                return this.Post(pRegistro);
            }
        }

        private int InsereTipoLivre(MetaModel pRegistro)
        {
            if (string.IsNullOrEmpty(pRegistro.ObjetivoDescritivo))
            {
                throw new ANIException(Mensagens.a0002, ANIException.CODIGO_CONDICAO_INVALIDA);
            }
            else
            {
                return this.Post(pRegistro);
            }
        }

        public int InserePorTipo(MetaModel pRegistro)
        {
            switch(pRegistro.Tipo)
            {
                case TipoMetaEnum.Peso:
                    return this.InsereTipoPeso(pRegistro);
                case TipoMetaEnum.Alimento:
                    return this.InsereTipoAlimento(pRegistro);
                case TipoMetaEnum.Livre:
                    return this.InsereTipoLivre(pRegistro);
                default:
                    throw new ANIException(Mensagens.e0001);
            }
        }

        private void AtualizaTipoPeso(MetaModel pRegistro, decimal pResultado)
        {
            pRegistro.ResultadoValor = pResultado;
            pRegistro.AtingidoBD = pRegistro.ResultadoValor >= pRegistro.ObjetivoValor ? 'S' : 'N';
            this.Put(pRegistro);            
        }

        private void AtualizaTipoAlimento(MetaModel pRegistro, decimal pResultado)
        {
            pRegistro.ResultadoValor = pResultado;
            pRegistro.AtingidoBD = pRegistro.ResultadoValor >= pRegistro.ObjetivoValor ? 'S' : 'N';
            this.Put(pRegistro);
        }

        private void AtualizaTipoLivre(MetaModel pRegistro, string pResultado, bool pAtingiu)
        {
            pRegistro.ResultadoDescritivo = pResultado;
            pRegistro.AtingidoBD = pAtingiu ? 'S' : 'N';
            this.Put(pRegistro);
        }

        public void Atualiza(int pConsulta, decimal? pResultadoValorPeso = null, decimal? pResultadoValorAlimento = null, string? pResultadoDescritivo = null, bool? pAtingiu = null)
        {
            TipoMetaEnum tipo = pResultadoValorPeso.HasValue ? TipoMetaEnum.Peso : pResultadoValorAlimento.HasValue ? TipoMetaEnum.Alimento : TipoMetaEnum.Livre;
            decimal? resultadoValor = tipo == TipoMetaEnum.Peso ? pResultadoValorPeso : pResultadoValorAlimento;
            List <MetaModel> listaMetas = this.GetList(pConsulta, tipo);
            foreach (MetaModel meta in listaMetas)
            {
                meta.ConcluidoBD = 'S';
                switch (meta.Tipo)
                {
                    case TipoMetaEnum.Peso:
                        this.AtualizaTipoPeso(meta, resultadoValor.GetValueOrDefault(0));
                        break;
                    case TipoMetaEnum.Alimento:
                        this.AtualizaTipoAlimento(meta, resultadoValor.GetValueOrDefault(0));
                        break;
                    case TipoMetaEnum.Livre:
                        this.AtualizaTipoLivre(meta, pResultadoDescritivo ?? "", pAtingiu ?? false);
                        break;
                    default:
                        throw new ANIException(string.Format(Mensagens.e0002, meta.ToString()));
                }
            }
        }

    }
}
