using ANI.Arquitetura;
using ANI.Models.Plano_Alimentar;

namespace ANI.Modulos.Plano_Alimentar
{
    public class PlanoAlimentarRefeicaoModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_PLANO_ALIMENTAR_REFEICAO";

        private PlanoAlimentarRefeicaoAlimentoModulo planoAlimentarRefeicaoAlimentoModulo;

        public PlanoAlimentarRefeicaoModulo() : this(null) { }

        public PlanoAlimentarRefeicaoModulo(DataBase? db) : base(db) 
        { 
            this.planoAlimentarRefeicaoAlimentoModulo = new PlanoAlimentarRefeicaoAlimentoModulo(this.getDataBase());
        }

        public PlanoAlimentarRefeicaoModel Get(int pPlanoAlimentar, int pRefeicao)
        {
            return this.ExecuteQuery<PlanoAlimentarRefeicaoModel>($@"Select ID Id,
                                                                            PLANO_ALIMENTAR PlanoAlimentar,
                                                                            REFEICAO Refeicao
                                                                     From PLANO_ALIMENTAR_REFEICAO
                                                                     Where PLANO_ALIMENTAR = {pPlanoAlimentar}
                                                                        and REFEICAO = {pRefeicao}");
        }

        public List<PlanoAlimentarRefeicaoModel> GetList(int pPlanoAlimentar)
        {
            return this.ExecuteQueryList<PlanoAlimentarRefeicaoModel>($@"    Select  ID Id,
                                                                                     REFEICAO Refeicao,
                                                                                     PLANO_ALIMENTAR PlanoAlimentar
                                                                                 From PLANO_ALIMENTAR_REFEICAO 
                                                                                 Where PLANO_ALIMENTAR = {pPlanoAlimentar}");
        }

        public List<PlanoAlimentarRefeicaoDetalheModel> GetList(int pPlanoAlimentar, bool buscarAlimento)
        {
            List<PlanoAlimentarRefeicaoDetalheModel> lista =
                    this.ExecuteQueryList<PlanoAlimentarRefeicaoDetalheModel>($@"    Select  PAR.ID Id,
                                                                                         PAR.REFEICAO Refeicao,
                                                                                         PAR.PLANO_ALIMENTAR PlanoAlimentar,
                                                                                         R.DESCRICAO DescricaoRefeicao
                                                                                 From PLANO_ALIMENTAR_REFEICAO PAR
                                                                                    inner join REFEICAO R on
                                                                                        (PAR.REFEICAO = R.ID)
                                                                                 Where PAR.PLANO_ALIMENTAR = {pPlanoAlimentar}
                                                                                 Order by R.HORARIO");
            if (lista != null && lista.Count > 0 && buscarAlimento)
            {
                foreach (PlanoAlimentarRefeicaoDetalheModel l in lista)
                {
                    l.Alimentos = this.planoAlimentarRefeicaoAlimentoModulo.GetList(l.Id);
                }
            }
            
            return lista;
        }

        public int Post(PlanoAlimentarRefeicaoModel pRegistro)
        {
            if (!this.ExisteRegistro("PLANO_ALIMENTAR_REFEICAO", 
                $@"PLANO_ALIMENTAR = {pRegistro.PlanoAlimentar} and REFEICAO = {pRegistro.Refeicao}")) 
            { 
                pRegistro.Id = this.ExecuteSequence(SEQUENCE);
                this.ExecuteCommand<PlanoAlimentarRefeicaoModel>($@"Insert into PLANO_ALIMENTAR_REFEICAO
                                                                        (ID, CONTA, PLANO_ALIMENTAR, REFEICAO)
                                                                    Values
                                                                        (@Id, {this.Conta}, @PlanoAlimentar, @Refeicao)", pRegistro);
                return pRegistro.Id;
            }
            return 0;
        }

        public void Importa(int pPlanoAlimentarOrigem, int pPlanoAlimentarDestino)
        {
            List<PlanoAlimentarRefeicaoModel> vinculos = this.GetList(pPlanoAlimentarOrigem);
            foreach(PlanoAlimentarRefeicaoModel v in vinculos)
            {
                v.PlanoAlimentar = pPlanoAlimentarDestino;
                int planoAlimentarRefeicaoOrigem = v.Id;
                int planoAlimentarRefeicaoDestino = this.Post(v);
                this.planoAlimentarRefeicaoAlimentoModulo.Importa(planoAlimentarRefeicaoOrigem, planoAlimentarRefeicaoDestino);
            }
        }

        public void Delete(int pPlanoAlimentar, int pRefeicao)
        {
            PlanoAlimentarRefeicaoModel paR = this.Get(pPlanoAlimentar, pRefeicao);
            if (paR != null)
            {
                this.planoAlimentarRefeicaoAlimentoModulo.Delete(paR.Id, true);
                this.ExecuteCommand<PlanoAlimentarRefeicaoModel>($@"Delete From PLANO_ALIMENTAR_REFEICAO
                                                                Where ID = @Id", paR);
            }
        }

        public void Delete(int pId)
        {
            this.planoAlimentarRefeicaoAlimentoModulo.Delete(pId, true);
            this.ExecuteCommand<PlanoAlimentarRefeicaoModel>($@"Delete From PLANO_ALIMENTAR_REFEICAO
                                                                Where ID = @Id", new PlanoAlimentarRefeicaoModel() { Id = pId });
        }
    }
}
