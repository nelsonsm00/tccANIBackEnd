using ANI.Arquitetura;
using ANI.Models.Medida;

namespace ANI.Modulos.Medida
{
    public class MedidaModulo : ModuloBase
    {
        public MedidaModulo() : this(null) { }

        public MedidaModulo(DataBase? db) : base(db) { }

        public List<MedidaDetalheModel> GetList(int pConsulta)
        {
            return this.ExecuteQueryList<MedidaDetalheModel>($@"    Select  CONSULTA Consulta,
                                                                            TIPO_MEDIDA TipoMedida,
                                                                            DESCRICAO Descricao,
                                                                            VALOR Valor
                                                                        From MEDIDA M
                                                                            inner join TIPO_MEDIDA TM on
                                                                                (M.TIPO_MEDIDA = TM.ID)
                                                                        Where M.CONSULTA = {pConsulta}");
        }

        public List<MedidaDetalheHistoricoModel> GetListHistorico(int pTratamento)
        {
            return this.ExecuteQueryList<MedidaDetalheHistoricoModel>($@"    Select  M.CONSULTA Consulta,
                                                                            M.TIPO_MEDIDA TipoMedida,
                                                                            TM.DESCRICAO Descricao,
                                                                            M.VALOR Valor,
                                                                            C.DATA DataBD
                                                                        From MEDIDA M
                                                                            inner join TIPO_MEDIDA TM on
                                                                                (M.TIPO_MEDIDA = TM.ID)
                                                                            inner join CONSULTA C on
                                                                                (M.CONSULTA = C.ID)
                                                                        Where C.TRATAMENTO = {pTratamento}");
        }

        public void Post(int pConsulta)
        {
            this.ExecuteCommand<MedidaDetalheModel>($@"Insert into MEDIDA 
                                                        (CONSULTA, TIPO_MEDIDA, CONTA, VALOR)
                                                       Select  @Consulta,
                                                               ID,
                                                               {this.Conta},
                                                               0
                                                       From TIPO_MEDIDA", new MedidaDetalheModel() { Consulta = pConsulta });
        }

        public void Put(List<MedidaPostModel> pRegistros)
        {
            foreach(MedidaPostModel m in pRegistros)
            {
                this.Put(m);
            }
        }

        private void Put(MedidaPostModel pRegistro)
        {
            this.ExecuteCommand<MedidaPostModel>($@"Update MEDIDA Set
                                                        VALOR = @Valor
                                                    Where CONSULTA = @Consulta and TIPO_MEDIDA = @TipoMedida", pRegistro);
        }

        public void Delete(int pConsulta)
        {
            this.ExecuteCommand<MedidaPostModel>($@"Delete from MEDIDA Where CONSULTA = @Consulta", new MedidaPostModel() { Consulta = pConsulta });
        }
    }
}
