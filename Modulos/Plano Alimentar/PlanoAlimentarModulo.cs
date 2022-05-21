using ANI.Arquitetura;
using ANI.Models.Plano_Alimentar;
using ANI.Models.Tratamento;
using ANI.Modulos.Alimento;
using ANI.Modulos.Tratamento;

namespace ANI.Modulos.Plano_Alimentar
{
    public class PlanoAlimentarModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_PLANO_ALIMENTAR";

        private TratamentoModulo tratamentoModulo;
        private PlanoAlimentarRefeicaoModulo planoAlimentarRefeicao;

        public PlanoAlimentarModulo() : this(null) { }

        public PlanoAlimentarModulo(DataBase? db) : base(db) 
        { 
            this.tratamentoModulo = new TratamentoModulo(this.getDataBase());
            this.planoAlimentarRefeicao = new PlanoAlimentarRefeicaoModulo(this.getDataBase());
        }

        public PlanoAlimentarModel Get(int pId)
        {
            return this.ExecuteQuery<PlanoAlimentarModel>($@"   Select  ID Id,
                                                                        DESCRICAO Descricao
                                                                From PLANO_ALIMENTAR
                                                                Where ID = {pId}");
        }

        public List<PlanoAlimentarModel> GetList(int pTratamento, bool pbuscaNaoVinculado = false)
        {
            return this.ExecuteQueryList<PlanoAlimentarModel>($@"   Select  PA.ID Id,
                                                                            PA.DESCRICAO Descricao
                                                                    From PLANO_ALIMENTAR PA
                                                                        left join TRATAMENTO_PLANO_ALIMENTAR TPA on
                                                                            (PA.ID = TPA.PLANO_ALIMENTAR)
                                                                    Where {(!pbuscaNaoVinculado
                                                                                ? $@"TPA.TRATAMENTO = {pTratamento}"
                                                                                : $@"PA.CONTA = {this.Conta} and 
                                                                                        (TPA.TRATAMENTO <> {pTratamento} or TPA.TRATAMENTO is null)")}");
        }

        public PlanoAlimentarDetalheModel GetDetalhe(int pPlanoAlimentar)
        {
            PlanoAlimentarDetalheModel planoAlimentar =
                this.ExecuteQuery<PlanoAlimentarDetalheModel>($@"   Select  PA.ID Id,
                                                                            PA.DESCRICAO Descricao,
                                                                            PE.NOME NomePaciente,
                                                                            PE.SOBRENOME SobrenomePaciente
                                                                    From PLANO_ALIMENTAR PA
                                                                        inner join TRATAMENTO_PLANO_ALIMENTAR TPA on
                                                                            (PA.ID = TPA.PLANO_ALIMENTAR)
                                                                        inner join TRATAMENTO T on
                                                                            (TPA.TRATAMENTO = T.ID)
                                                                        inner join PACIENTE P on
                                                                            (T.PACIENTE = P.ID) 
                                                                        inner join PESSOA PE on
                                                                            (P.ID = PE.ID)
                                                                    Where PA.ID = {pPlanoAlimentar}");
            if (planoAlimentar != null)
            {                
                planoAlimentar.Refeicoes = this.planoAlimentarRefeicao.GetList(pPlanoAlimentar, true);
            }
            return planoAlimentar;
        }

        private int Post(PlanoAlimentarModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<PlanoAlimentarModel>($@"Insert into PLANO_ALIMENTAR
                                                            (ID, CONTA, DESCRICAO)
                                                        Values
                                                            (@Id, {this.Conta}, UPPER(@Descricao))", pRegistro);
            return pRegistro.Id;
        }

        public int Post(PlanoAlimentarModel pRegistro, int pTratamento)
        {
            TratamentoPlanoAlimentarModel t = new TratamentoPlanoAlimentarModel();
            t.PlanoAlimentar = this.Post(pRegistro);
            t.Tratamento = pTratamento;
            this.tratamentoModulo.Post(t);
            return t.PlanoAlimentar;
        }      
        
        public void Importa(int pId, int pTratamento)
        {
            PlanoAlimentarModel plano = this.Get(pId);
            plano.Id = this.Post(plano, pTratamento);
            this.planoAlimentarRefeicao.Importa(pId, plano.Id);
        }

        public void Put(PlanoAlimentarModel pRegistro)
        {
            this.ExecuteCommand<PlanoAlimentarModel>($@"Update PLANO_ALIMENTAR Set
                                                            DESCRICAO = UPPER(@Descricao)
                                                        Where ID = @Id", pRegistro);
        }

        public void Delete(int pId)
        {
            List<PlanoAlimentarRefeicaoDetalheModel> pAR = this.planoAlimentarRefeicao.GetList(pId, false);
            foreach (PlanoAlimentarRefeicaoDetalheModel p in pAR)
                this.planoAlimentarRefeicao.Delete(p.Id);
            this.tratamentoModulo.DeleteTratamentoPlanoAlimentar(pId);
            this.ExecuteCommand<PlanoAlimentarModel>($@"Delete From PLANO_ALIMENTAR
                                                        Where ID = @Id", new PlanoAlimentarModel() { Id = pId });
        }
    }
}
