using HtmlAgilityPack;
using System.Net;
using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Alimento;
using ANI.Models.Alimento.TCA;
using ANI.Modulos.Alimento.TCA;

namespace ANI.Modulos.Alimento
{
    public class AlimentoModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_ALIMENTO";
        TCAInterface Tca;

        public AlimentoModulo(TCAInterface tca) : this(tca, null) { }

        public AlimentoModulo(TCAInterface tca, DataBase? db) : base(db) 
        {
            this.Tca = tca;
        }

        public List<AlimentoResumidoModel> GetListaTCA(AlimentoTCAFiltroModel filtro)
        {
            return this.Tca.GetLista(filtro);
        }

        public List<ComponenteAlimentoModel> GetTCA(string pCodigo)
        {

            return this.Tca.Get(pCodigo);
        }

        public AlimentoModel Get(string pCodigo)
        {
            return this.ExecuteQuery<AlimentoModel>($@" Select  ID Id,
                                                                DESCRICAO Descricao,
                                                                CODIGO Codigo
                                                        From ALIMENTO
                                                        Where CODIGO = '{pCodigo}'
                                                            and CONTA = {this.Conta}");
        }

        public int Post(AlimentoModel pRegistro)
        {
            AlimentoModel alimentoAux = this.Get(pRegistro.Codigo);
            if (alimentoAux != null)
            {
                //Se o alimento já existir, atualiza a descrição.
                alimentoAux.Descricao = pRegistro.Descricao;
                this.Put(alimentoAux);
                return alimentoAux.Id;
            }
            else
                pRegistro.Id = this.ExecuteSequence(SEQUENCE);

            this.ExecuteCommand<AlimentoModel>($@"Insert into ALIMENTO
                                                    (ID, CONTA, DESCRICAO, CODIGO)
                                                  Values
                                                    (@Id, {this.Conta}, UPPER(@Descricao), @Codigo)", pRegistro);
            return pRegistro.Id;
        }

        public void Put(AlimentoModel pRegistro)
        {
            this.ExecuteCommand<AlimentoModel>($@"Update ALIMENTO set DESCRICAO = UPPER(@Descricao)
                                                  Where ID = @Id", pRegistro);
        }
    }
}
