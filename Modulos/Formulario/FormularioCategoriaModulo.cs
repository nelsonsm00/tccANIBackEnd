using ANI.Arquitetura;
using ANI.Models.Formulario;

namespace ANI.Modulos.Formulario
{
    public class FormularioCategoriaModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_FORMULARIO_CATEGORIA";

        private FormularioItemModulo formularioItemModulo;

        public FormularioCategoriaModulo() : this(null) { }

        public FormularioCategoriaModulo(DataBase? db) : base(db) 
        {
            this.formularioItemModulo = new FormularioItemModulo(this.getDataBase());
        }

        public FormularioCategoriaModel Get(int pId)
        {
            return this.ExecuteQuery<FormularioCategoriaModel>($@"  Select  ID Id,
                                                                            FORMULARIO Formulario,
                                                                            DESCRICAO Descricao,
                                                                            ATIVO AtivoBD
                                                                    From FORMULARIO_CATEGORIA
                                                                    Where ID = {pId}");
        }

        public List<FormularioCategoriaModel> GetList(int pFormulario)
        {
            return this.ExecuteQueryList<FormularioCategoriaModel>($@"  Select  ID Id,
                                                                                FORMULARIO Formulario,
                                                                                DESCRICAO Descricao,
                                                                                ATIVO AtivoBD
                                                                        From FORMULARIO_CATEGORIA
                                                                        Where FORMULARIO = {pFormulario}
                                                                        Order by ORDEM ASC");
        }

        public List<FormularioCategoriaCompletoModel> GetListCompleto(int pFormulario)
        {
            List<FormularioCategoriaCompletoModel> categorias =
                   this.ExecuteQueryList<FormularioCategoriaCompletoModel>($@"  Select  ID Id,
                                                                                        FORMULARIO Formulario,
                                                                                        DESCRICAO Descricao,
                                                                                        ATIVO AtivoBD
                                                                                From FORMULARIO_CATEGORIA
                                                                                Where FORMULARIO = {pFormulario}
                                                                                    and ATIVO = 'S'
                                                                                 Order by ORDEM ASC");

            if (categorias != null && categorias.Count > 0)
            {
                foreach(FormularioCategoriaCompletoModel c in categorias)
                {
                    c.Itens = this.formularioItemModulo.GetList(c.Id, true);
                }
            }
            return categorias;
        }

        public List<FormularioCategoriaCompletoRespondidoModel> GetListCompletoRespondido(int pFormulario, int pTratamento)
        {
            List<FormularioCategoriaCompletoRespondidoModel> categorias =
                   this.ExecuteQueryList<FormularioCategoriaCompletoRespondidoModel>($@"  Select  ID Id,
                                                                                        FORMULARIO Formulario,
                                                                                        DESCRICAO Descricao,
                                                                                        ATIVO AtivoBD
                                                                                From FORMULARIO_CATEGORIA
                                                                                Where FORMULARIO = {pFormulario}
                                                                                    and ATIVO = 'S'
                                                                                 Order by ORDEM ASC");

            if (categorias != null && categorias.Count > 0)
            {
                foreach (FormularioCategoriaCompletoRespondidoModel c in categorias)
                {
                    c.Itens = this.formularioItemModulo.GetListCompletoRespondido(c.Id, pTratamento);
                }
            }
            return categorias;
        }

        public int GetProximaOrdem(int pFormulario)
        {
            return this.ExecuteScalar<int>($@"Select MAX(ORDEM) From FORMULARIO_CATEGORIA Where FORMULARIO = {pFormulario} and ATIVO = 'S'") + 1;
        }

        public int Post(FormularioCategoriaModel pRegistro)
        {
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<FormularioCategoriaModel>($@"Insert into FORMULARIO_CATEGORIA
                                                                (ID, CONTA, FORMULARIO, DESCRICAO, ATIVO, ORDEM)
                                                            Values
                                                                (@Id, {this.Conta}, @Formulario, UPPER(@Descricao), 'S',
                                                                {this.GetProximaOrdem(pRegistro.Formulario)})", pRegistro);
            return pRegistro.Id;
        } 

        public void Put(FormularioCategoriaModel pRegistro)
        {
            this.ExecuteCommand<FormularioCategoriaModel>($@"Update FORMULARIO_CATEGORIA Set
                                                                DESCRICAO = UPPER(@Descricao)
                                                             Where ID = @Id", pRegistro);
        }

        public void AlteraOrdem(int pId, int pOrdem)
        {
            this.ExecuteCommand<FormularioCategoriaModel>($@"Update FORMULARIO_CATEGORIA Set
                                                                ORDEM = {pOrdem}
                                                            Where ID = @Id", new FormularioCategoriaModel() { Id = pId });
        }

        public void Delete(int pId)
        {
            FormularioCategoriaModel registro = this.Get(pId);
            if (registro != null) {
                if (this.ExisteRegistro("FORMULARIO_ITEM", $@"FORMULARIO_CATEGORIA = {pId}"))
                {
                    if (this.ExisteRegistro("TRATAMENTO_FORMULARIO", $@"FORMULARIO = {registro.Formulario}"))
                    {
                        this.ExecuteCommand<FormularioCategoriaModel>($@"Update FORMULARIO_CATEGORIA Set
                                                                            ATIVO = 'N'
                                                                         Where ID = @Id", registro);
                    }
                    else
                    {
                        this.formularioItemModulo.DeleteCascata(registro.Id);
                        this.ExecuteCommand<FormularioCategoriaModel>($@"Delete from FORMULARIO_CATEGORIA Where ID = @Id", registro);
                    }
                }
                else
                {
                    this.ExecuteCommand<FormularioCategoriaModel>($@"Delete from FORMULARIO_CATEGORIA Where ID = @Id", registro);
                }
            }
        }

        public void DeleteCascata(int pFormulario)
        {
            List<FormularioCategoriaModel> categorias = this.GetList(pFormulario);
            foreach (FormularioCategoriaModel categoria in categorias)
            {
                this.formularioItemModulo.DeleteCascata(categoria.Id);
            }
            this.ExecuteCommand<FormularioCategoriaModel>($@"Delete from FORMULARIO_CATEGORIA Where FORMULARIO = @Formulario", 
                                                            new FormularioCategoriaModel() { Formulario = pFormulario });
        }
    }
}
