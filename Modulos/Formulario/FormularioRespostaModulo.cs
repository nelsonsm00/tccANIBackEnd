using ANI.Arquitetura;
using ANI.Models.Formulario;

namespace ANI.Modulos.Formulario
{
    public class FormularioRespostaModulo : ModuloBase
    {
        public FormularioRespostaModulo() : this(null) { }

        public FormularioRespostaModulo(DataBase? db) : base(db) { }

        public FormularioRespostaModel Get(int pTratamento, int pFormularioItem)
        {
            return this.ExecuteQuery<FormularioRespostaModel>($@"   Select  TRATAMENTO Tratamento,
                                                                            FORMULARIO_ITEM FormularioItem,
                                                                            VALOR Valor
                                                                        From FORMULARIO_RESPOSTA
                                                                        Where TRATAMENTO = {pTratamento} and FORMULARIO_ITEM = {pFormularioItem}");
        }

        public void Post(FormularioRespostaModel pRegistro)
        {
            if (this.ExisteRegistro("FORMULARIO_RESPOSTA", $@"TRATAMENTO = {pRegistro.Tratamento} 
                                                                and FORMULARIO_ITEM = {pRegistro.FormularioItem}"))
            {
                this.Put(pRegistro);
            }
            else
            {
                this.ExecuteCommand<FormularioRespostaModel>($@"Insert into FORMULARIO_RESPOSTA
                                                                (TRATAMENTO, FORMULARIO_ITEM, CONTA, VALOR)
                                                            Values
                                                                (@Tratamento, @FormularioItem, {this.Conta}, @Valor)", pRegistro);
            }
        } 

        private void Put(FormularioRespostaModel pRegistro)
        {
            this.ExecuteCommand<FormularioRespostaModel>($@"Update FORMULARIO_RESPOSTA Set
                                                                VALOR = @Valor
                                                            Where TRATAMENTO = @Tratamento and FORMULARIO_ITEM = @FormularioItem", pRegistro);
        }
    }
}
