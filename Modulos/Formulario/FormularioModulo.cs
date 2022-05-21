using ANI.Arquitetura;
using ANI.Models.Formulario;

namespace ANI.Modulos.Formulario
{
    public class FormularioModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_FORMULARIO";

        private FormularioCategoriaModulo formularioCategoriaModulo;
        private FormularioItemModulo formularioItemModulo;
        private FormularioRespostaModulo formularioRespostaModulo;

        public FormularioModulo() : this(null) { }

        public FormularioModulo(DataBase? db) : base(db) 
        {
            this.formularioCategoriaModulo = new FormularioCategoriaModulo(this.getDataBase());
            this.formularioItemModulo = new FormularioItemModulo(this.getDataBase());
            this.formularioRespostaModulo = new FormularioRespostaModulo(this.getDataBase());
        }

        public FormularioCompletoModel GetFormularioCompleto(int pId)
        {
            FormularioCompletoModel formulario = this.ExecuteQuery<FormularioCompletoModel>($@"  Select ID Id,
                                                                                                            DESCRICAO Descricao,
                                                                                                            TIPO Tipo
                                                                                                     From FORMULARIO
                                                                                                     Where ID = {pId}");
            if (formulario != null)
            {
                formulario.Categorias = this.formularioCategoriaModulo.GetListCompleto(formulario.Id);
            }

            return formulario;
        }

        public FormularioCompletoRespondidoModel GetFormularioCompletoRespondido(int pId, int pTratamento)
        {
            FormularioCompletoRespondidoModel formulario = this.ExecuteQuery<FormularioCompletoRespondidoModel>($@"  
                                                                                                    Select  ID Id,
                                                                                                            DESCRICAO Descricao,
                                                                                                            TIPO Tipo
                                                                                                     From FORMULARIO
                                                                                                     Where ID = {pId}");
            if (formulario != null)
            {
                formulario.Categorias = this.formularioCategoriaModulo.GetListCompletoRespondido(formulario.Id, pTratamento);
            }

            return formulario;
        }

        public List<FormularioModel> GetList(string? tipo = null)
        {
            return this.ExecuteQueryList<FormularioModel>($@"  Select  ID Id,
                                                                       DESCRICAO Descricao,
                                                                       TIPO Tipo
                                                               From FORMULARIO
                                                               Where CONTA = {this.Conta}
                                                                and ATIVO = 'S'
                                                                {(tipo != null ? $@" and TIPO = '{tipo}'" : "")}");
        }

        public int Post(FormularioModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<FormularioModel>($@"Insert into FORMULARIO
                                                        (ID, CONTA, DESCRICAO, TIPO)
                                                            Values
                                                        (@Id, {this.Conta}, UPPER(@Descricao), @Tipo)", pRegistro);
            return pRegistro.Id;
        } 

        public void Put(FormularioModel pRegistro)
        {
            this.ExecuteCommand<FormularioModel>($@"Update FORMULARIO Set
                                                        DESCRICAO = UPPER(@Descricao)
                                                    Where ID = @Id", pRegistro);
        }

        public void AlteraOrdem(FormularioCompletoModel pRegistro)
        {
            for(int i = 0; i < pRegistro.Categorias.Count; i++)
            {
                this.formularioCategoriaModulo.AlteraOrdem(pRegistro.Categorias[i].Id, i);
                for(int j = 0; j < pRegistro.Categorias[i].Itens.Count; j++)
                {
                    this.formularioItemModulo.AlteraOrdem(pRegistro.Categorias[i].Itens[j].Id, j);
                }
            }
        }

        public void Responde(List<FormularioCategoriaCompletoRespondidoModel> pCategorias, int pTratamento, int pFormulario)
        {
            if (!this.ExisteRegistro("TRATAMENTO_FORMULARIO", $@"TRATAMENTO = {pTratamento} and FORMULARIO = {pFormulario}"))
                this.ExecuteCommand<FormularioModel>($@"Insert into TRATAMENTO_FORMULARIO 
                                                            (TRATAMENTO, FORMULARIO, CONTA)
                                                         Values
                                                            ({pTratamento}, @Id, {this.Conta})", new FormularioModel() { Id = pFormulario });
            
            foreach(FormularioCategoriaCompletoRespondidoModel categorias in pCategorias)
            {
                foreach(FormularioItemCompletoRespondidoModel item in categorias.Itens)
                {
                    item.Resposta.Tratamento = pTratamento;
                    item.Resposta.FormularioItem = item.Id;
                    this.formularioRespostaModulo.Post(item.Resposta);
                }
            }
        } 

        public void Delete(int pId)
        {
            if (this.ExisteRegistro("TRATAMENTO_FORMULARIO", $@"CONTA = {this.Conta} and FORMULARIO = {pId}"))
            {
                this.ExecuteCommand<FormularioModel>($@"Update FORMULARIO Set
                                                            ATIVO = 'N'
                                                        Where ID = @Id", new FormularioModel() { Id = pId });
            }
            else
            {
                if (this.ExisteRegistro("FORMULARIO_CATEGORIA", $@"FORMULARIO = {pId}"))
                {
                    this.formularioCategoriaModulo.DeleteCascata(pId);
                }
                this.ExecuteCommand<FormularioModel>($@"Delete from FORMULARIO Where ID = @Id", new FormularioModel() { Id = pId });
            }            
        }
    }
}
