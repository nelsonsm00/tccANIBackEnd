using ANI.Arquitetura;
using ANI.Models.Nutricionista;
using ANI.Modulos.Pessoa;

namespace ANI.Modulos.Nutricionista
{
    public class NutricionistaModulo : ModuloBase
    {
        private PessoaModulo pessoaModulo;

        public NutricionistaModulo() : this(null) { }

        public NutricionistaModulo(DataBase? db) : base(db)
        {
            this.pessoaModulo = new PessoaModulo(this.getDataBase());
        }

        public NutricionistaModel Get(int pId)
        {
            return this.ExecuteQuery<NutricionistaModel>($@"    Select {PessoaModulo.GetColunas("P")},
                                                                       N.CONTA Conta
                                                                From NUTRICIONISTA N 
                                                                    inner join PESSOA P on (N.ID = P.ID)
                                                                Where N.ID = {pId}");
        }

        public int Post(NutricionistaModel pRegistro)
        {
            pRegistro.Id = this.pessoaModulo.Post(pRegistro);
            this.ExecuteCommand<NutricionistaModel>($@" Insert into NUTRICIONISTA
                                                            (ID, CONTA)
                                                        Values
                                                            (@Id, {this.Conta})", pRegistro);
            return pRegistro.Id;

        }

        public void Put(NutricionistaModel pRegistro)
        {
            this.pessoaModulo.Put(pRegistro);
        }
    }
}
