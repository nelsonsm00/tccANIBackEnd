using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Refeicao;

namespace ANI.Modulos.Refeicao
{
    public class RefeicaoModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_REFEICAO";

        public RefeicaoModulo() : this(null) { }

        public RefeicaoModulo(DataBase? db) : base(db) { }

        public List<RefeicaoModel> GetList(int? pPlanoAlimentar)
        {
            string colunaSelecionado = "NULL";
            if (pPlanoAlimentar.HasValue)
            {
                colunaSelecionado = $@" (Select 'S' 
                                        From PLANO_ALIMENTAR_REFEICAO P 
                                        Where P.PLANO_ALIMENTAR = {pPlanoAlimentar.Value} and P.REFEICAO = R.ID)";
            }

            return this.ExecuteQueryList<RefeicaoModel>($@" Select  R.ID Id,
                                                                    R.DESCRICAO Descricao,
                                                                    R.HORARIO HorarioBD,
                                                                    COALESCE({colunaSelecionado}, 'N') SelecionadoBD
                                                            From REFEICAO R
                                                            Where R.CONTA = {this.Conta}
                                                            Order by R.HORARIO");
        }

        public int Post(RefeicaoModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<RefeicaoModel>($@"Insert into REFEICAO
                                                    (ID, CONTA, DESCRICAO, HORARIO)
                                                  Values
                                                    (@Id, {this.Conta}, UPPER(@Descricao), 
                                                    {(!pRegistro.HorarioBD.HasValue ? "NULL" : 
                                                    $@"CONVERT(DATETIME, '2022-01-01 {pRegistro.HorarioBD.Value.ToString("HH:mm")}:00.000')")})", 
                                              pRegistro);
            return pRegistro.Id;
        } 

        public void Put(RefeicaoModel pRegistro)
        {
            this.ExecuteCommand<RefeicaoModel>($@"Update REFEICAO Set
                                                    DESCRICAO = UPPER(@Descricao),
                                                    HORARIO = {(!pRegistro.HorarioBD.HasValue ? "NULL" :
                                                    $@"CONVERT(DATETIME, '2022-01-01 {pRegistro.HorarioBD.Value.ToString("HH:mm")}:00.000')")}
                                                  Where ID = @Id", pRegistro);
        }

        public void Delete(int pId)
        {
            if (this.ExisteRegistro("PLANO_ALIMENTAR_REFEICAO", $@"REFEICAO = {pId}"))
            {
                throw new ANIException(Mensagens.e0121);
            }
            else 
                this.ExecuteCommand<RefeicaoModel>($@"Delete From REFEICAO
                                                        Where ID = @Id", new RefeicaoModel() { Id = pId });
        }
    }
}
