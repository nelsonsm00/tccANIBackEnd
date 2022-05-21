using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Orientacao;

namespace ANI.Modulos.Orientacao
{
    public class OrientacaoModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_ORIENTACAO";

        public OrientacaoModulo() : this(null) { }

        public OrientacaoModulo(DataBase? db) : base(db) { }

        public OrientacaoModel Get(int pId)
        {
            return this.ExecuteQuery<OrientacaoModel>($@"   Select  ID Id,
                                                                    TITULO Titulo,
                                                                    TEXTO Texto,
                                                                    PUBLICO PublicoBD
                                                            From ORIENTACAO
                                                            Where ID = {pId}");
        }

        public List<OrientacaoModel> GetList(int? pTratamento = null, bool buscaOrientacoesNaoVinculadas = false)
        {
            return this.ExecuteQueryList<OrientacaoModel>($@"   Select  O.ID Id,
                                                                        O.TITULO Titulo,
                                                                        O.TEXTO Texto,
                                                                        O.PUBLICO PublicoBD
                                                                From ORIENTACAO O
                                                                    Left Join TRATAMENTO_ORIENTACAO T on
                                                                        (O.ID = T.ORIENTACAO)
                                                                Where O.CONTA = {this.Conta}
                                                                    {(pTratamento.HasValue && !buscaOrientacoesNaoVinculadas 
                                                                    ? $@" and T.TRATAMENTO = {pTratamento.Value}" 
                                                                    : buscaOrientacoesNaoVinculadas 
                                                                        ? $@" and (T.TRATAMENTO is null or T.TRATAMENTO <> {pTratamento.Value})"
                                                                        : "")}");
        }

        public int Post(OrientacaoModel pRegistro, int? pTratamento)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<OrientacaoModel>($@"Insert into ORIENTACAO
                                                        (ID, CONTA, TITULO, TEXTO, PUBLICO)
                                                    Values
                                                        (@Id, {this.Conta}, @Titulo, @Texto, @PublicoBD)", pRegistro);

            if (pTratamento.HasValue)
                this.Vincular(pRegistro.Id,pTratamento.Value);

            return pRegistro.Id;
        }

        public void Vincular(int pId, int pTratamento)
        {
            this.ExecuteCommand<OrientacaoModel>($@"Insert into TRATAMENTO_ORIENTACAO
                                                        (TRATAMENTO, ORIENTACAO, CONTA)
                                                    Values
                                                        ({pTratamento}, @Id, {this.Conta})", new OrientacaoModel { Id = pId });
        }

        public void Put(OrientacaoModel pRegistro)
        {
            this.ExecuteCommand<OrientacaoModel>($@"Update ORIENTACAO Set
                                                        TITULO = @Titulo,
                                                        TEXTO = @Texto,
                                                        PUBLICO = @PublicoBD
                                                    Where ID = @Id", pRegistro);
        }

        public void Delete(int pId, int? pTratamento = null)
        {
            if (pTratamento.HasValue)
            {
                this.ExecuteCommand<OrientacaoModel>($@"Delete from TRATAMENTO_ORIENTACAO 
                                                        Where ORIENTACAO = @Id
                                                            and TRATAMENTO = {pTratamento.Value}", new OrientacaoModel { Id = pId });
            }
            else
            {
                if (this.ExisteRegistro("TRATAMENTO_ORIENTACAO", $@"ORIENTACAO = {pId}"))
                    throw new ANIException(Mensagens.e0136);

                this.ExecuteCommand<OrientacaoModel>($@"Delete from ORIENTACAO Where ID = @Id", new OrientacaoModel { Id = pId });
            }
        }
    }
}
