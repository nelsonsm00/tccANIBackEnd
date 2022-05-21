using ANI.Arquitetura;
using ANI.Models.Plano_Alimentar;
using ANI.Modulos.Alimento;

namespace ANI.Modulos.Plano_Alimentar
{
    public class PlanoAlimentarRefeicaoAlimentoModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_PLANO_ALIMENTAR_REFEICAO_ALIMENTO";

        private AlimentoModulo alimentoModulo;

        public PlanoAlimentarRefeicaoAlimentoModulo() : this(null) { }

        public PlanoAlimentarRefeicaoAlimentoModulo(DataBase? db) : base(db) 
        { 
            this.alimentoModulo = new AlimentoModulo(null, this.getDataBase());
        }

        public List<PlanoAlimentarRefeicaoAlimentoDetalheModel> GetList(int pPlanoAlimentarRefeicao)
        {
            return
            this.ExecuteQueryList<PlanoAlimentarRefeicaoAlimentoDetalheModel>($@"   Select  PARA.ID Id,
                                                                                            PARA.PLANO_ALIMENTAR_REFEICAO PlanoAlimentarRefeicao,
                                                                                            A.DESCRICAO Descricao,
                                                                                            A.ID Alimento,
                                                                                            A.CODIGO Codigo,
                                                                                            PARA.QUANTIDADE Quantidade,
                                                                                            PARA.UNIDADE Unidade
                                                                                    From PLANO_ALIMENTAR_REFEICAO_ALIMENTO PARA
                                                                                        inner join ALIMENTO A on
                                                                                            (PARA.ALIMENTO = A.ID)
                                                                                    Where PARA.PLANO_ALIMENTAR_REFEICAO = {pPlanoAlimentarRefeicao}");
        }

        public int Post(PlanoAlimentarRefeicaoAlimentoModel pRegistro)
        {
            int alimentoId = this.alimentoModulo.Post(pRegistro.Alimento);
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            //Precisa fazer isso para não dar erro no banco
            pRegistro.Alimento = null;
            this.ExecuteCommand<PlanoAlimentarRefeicaoAlimentoModel>($@"
                    Insert into PLANO_ALIMENTAR_REFEICAO_ALIMENTO 
                        (ID, CONTA, PLANO_ALIMENTAR_REFEICAO, ALIMENTO, QUANTIDADE, UNIDADE)
                    Values
                        (@Id, {this.Conta}, @PlanoAlimentarRefeicao, {alimentoId}, @Quantidade, @Unidade)", pRegistro);
            return pRegistro.Id;
        }

        public int Post(PlanoAlimentarRefeicaoAlimentoDetalheModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<PlanoAlimentarRefeicaoAlimentoDetalheModel>($@"
                    Insert into PLANO_ALIMENTAR_REFEICAO_ALIMENTO 
                        (ID, CONTA, PLANO_ALIMENTAR_REFEICAO, ALIMENTO, QUANTIDADE, UNIDADE)
                    Values
                        (@Id, {this.Conta}, @PlanoAlimentarRefeicao, @Alimento, @Quantidade, @Unidade)", pRegistro);
            return pRegistro.Id;
        }

        public void Importa(int pPlanoAlimentarRefeicaoOrigem, int pPlanoAlimentarRefeicaoDestino)
        {
            List<PlanoAlimentarRefeicaoAlimentoDetalheModel> vinculos = this.GetList(pPlanoAlimentarRefeicaoOrigem);
            foreach (PlanoAlimentarRefeicaoAlimentoDetalheModel v in vinculos)
            {
                v.PlanoAlimentarRefeicao = pPlanoAlimentarRefeicaoDestino;
                v.Id = this.Post(v);
            }
        }

        public void Put(PlanoAlimentarRefeicaoAlimentoModel pRegistro)
        {
            this.alimentoModulo.Put(pRegistro.Alimento);
            //Precisa fazer isso para não dar erro no banco
            pRegistro.Alimento = null;
            this.ExecuteCommand<PlanoAlimentarRefeicaoAlimentoModel>($@"Update PLANO_ALIMENTAR_REFEICAO_ALIMENTO Set
                                                            QUANTIDADE = @Quantidade,
                                                            UNIDADE = @Unidade
                                                        Where ID = @Id", pRegistro);
        }

        public void Delete(int pId, bool deletaPorPlanoRefeicao = false)
        {
            string coluna = deletaPorPlanoRefeicao ? "PLANO_ALIMENTAR_REFEICAO = @PlanoAlimentarRefeicao"
                            : "ID = @Id";
            this.ExecuteCommand<PlanoAlimentarRefeicaoAlimentoModel>($@"Delete From PLANO_ALIMENTAR_REFEICAO_ALIMENTO
                                                                        Where {coluna}", new PlanoAlimentarRefeicaoAlimentoModel() { 
                                                                                            Id = pId,
                                                                                            PlanoAlimentarRefeicao = pId
                                                                                         });
        }
    }
}
