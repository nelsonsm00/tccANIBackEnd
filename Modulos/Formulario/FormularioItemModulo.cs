using ANI.Arquitetura;
using ANI.Models.Formulario;

namespace ANI.Modulos.Formulario
{
    public class FormularioItemModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_FORMULARIO_ITEM";

        private FormularioRespostaModulo formularioRespostaModulo;

        public FormularioItemModulo() : this(null) { }

        public FormularioItemModulo(DataBase? db) : base(db) 
        {
            this.formularioRespostaModulo = new FormularioRespostaModulo(this.getDataBase());
        }

        public List<FormularioItemModel> GetList(int pFormularioCategoria, bool pBuscaAtivo = false)
        {
            return this.ExecuteQueryList<FormularioItemModel>($@"   Select  ID Id,
                                                                            FORMULARIO_CATEGORIA FormularioCategoria,
                                                                            DESCRICAO Descricao,
                                                                            TIPO Tipo,
                                                                            ATIVO AtivoBD,
                                                                            ALTERNATIVAS AlternativasBD
                                                                        From FORMULARIO_ITEM
                                                                        Where FORMULARIO_CATEGORIA = {pFormularioCategoria}
                                                                            {(pBuscaAtivo ? " and ATIVO = 'S'" : "")}
                                                                        Order by ORDEM ASC");
        }

        public List<FormularioItemCompletoRespondidoModel> GetListCompletoRespondido(int pFormularioCategoria, int pTratamento)
        {
            List <FormularioItemCompletoRespondidoModel> itens =
                this.ExecuteQueryList<FormularioItemCompletoRespondidoModel>($@"    Select    ID Id,
                                                                                             FORMULARIO_CATEGORIA FormularioCategoria,
                                                                                             DESCRICAO Descricao,
                                                                                             TIPO Tipo,
                                                                                             ATIVO AtivoBD,
                                                                                             ALTERNATIVAS AlternativasBD
                                                                                     From FORMULARIO_ITEM
                                                                                     Where FORMULARIO_CATEGORIA = {pFormularioCategoria}
                                                                                     Order by ORDEM ASC");
            foreach(FormularioItemCompletoRespondidoModel item in itens)
            {
                item.Resposta = this.formularioRespostaModulo.Get(pTratamento, item.Id);
                if (item.Resposta == null)
                    item.Resposta = new FormularioRespostaModel();
            }
            return itens;
        }

        public int GetProximaOrdem(int pFormularioCategoria)
        {
            return this.ExecuteScalar<int>($@"Select MAX(ORDEM) From FORMULARIO_ITEM Where FORMULARIO_CATEGORIA = {pFormularioCategoria} and ATIVO = 'S'") + 1;
        }

        public int Post(FormularioItemModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<FormularioItemModel>($@"Insert into FORMULARIO_ITEM
                                                            (ID, CONTA, FORMULARIO_CATEGORIA, DESCRICAO, TIPO, ATIVO, ALTERNATIVAS, ORDEM)
                                                        Values
                                                            (@Id, {this.Conta}, @FormularioCategoria, UPPER(@Descricao), @Tipo, 'S', @AlternativasBD, 
                                                            {this.GetProximaOrdem(pRegistro.FormularioCategoria)})", pRegistro);
             return pRegistro.Id;
        } 

        public void Put(FormularioItemModel pRegistro)
        {
            this.ExecuteCommand<FormularioItemModel>($@"Update FORMULARIO_ITEM Set
                                                                DESCRICAO = UPPER(@Descricao),
                                                                ALTERNATIVAS = @AlternativasBD
                                                            Where ID = @Id", pRegistro);
        }

        public void AlteraOrdem(int pId, int pOrdem)
        {
            this.ExecuteCommand<FormularioItemModel>($@"Update FORMULARIO_ITEM Set
                                                                ORDEM = {pOrdem}
                                                            Where ID = @Id", new FormularioItemModel() { Id = pId });
        }

        public void Delete(int pId)
        {
            if (this.ExisteRegistro("FORMULARIO_RESPOSTA", $@"FORMULARIO_ITEM = {pId}"))
            {
                this.ExecuteCommand<FormularioItemModel>($@"Update FORMULARIO_ITEM Set
                                                                    ATIVO = 'N'
                                                                Where ID = @Id", new FormularioItemModel() { Id = pId});
            }
            else
            {
                this.ExecuteCommand<FormularioItemModel>($@"Delete from FORMULARIO_ITEM Where ID = @Id", new FormularioItemModel() { Id = pId });
            }
        }

        public void DeleteCascata(int pFormularioCategoria)
        {
            this.ExecuteCommand<FormularioItemModel>($@"Delete from FORMULARIO_ITEM 
                                                        Where FORMULARIO_CATEGORIA = @FormularioCategoria", 
                                                        new FormularioItemModel() { FormularioCategoria = pFormularioCategoria });
        }
    }
}
