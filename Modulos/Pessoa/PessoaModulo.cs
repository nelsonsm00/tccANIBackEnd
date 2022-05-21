using ANI.Arquitetura;
using ANI.Models.Pessoa;

namespace ANI.Modulos.Pessoa
{
    public class PessoaModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_PESSOA";

        public PessoaModulo() : this(null) { }

        public PessoaModulo(DataBase? db) : base(db) { }

        public static string GetColunas(string? pAlias = null)
        {
            pAlias = pAlias == null ? "" : pAlias + "."; 
            return $@"   {pAlias}ID Id,
                         {pAlias}NOME Nome,
                         {pAlias}SOBRENOME Sobrenome,
                         {pAlias}SEXO Sexo,
                         {pAlias}CPF Cpf,
                         {pAlias}UF Uf,
                         {pAlias}CIDADE Cidade,
                         {pAlias}BAIRRO Bairro,
                         {pAlias}RUA Rua,
                         {pAlias}NUMERO Numero,
                         {pAlias}COMPLEMENTO Complemento,
                         {pAlias}DDD Ddd,
                         {pAlias}TELEFONE Telefone,
                         {pAlias}DATA_NASCIMENTO DataNascimentoBD,
                         {pAlias}PROFISSAO Profissao,
                         {pAlias}CARGA_HORARIA CargaHoraria,
                         {pAlias}ESTADO_CIVIL EstadoCivil";
        }

        public PessoaModel Get(int pId)
        {
            return this.ExecuteQuery<PessoaModel>($@"Select {PessoaModulo.GetColunas()} From PESSOA Where ID = {pId}");
        }

        public PessoaModel Get(string pCpf)
        {
            return this.ExecuteQuery<PessoaModel>($@"Select {PessoaModulo.GetColunas()} From PESSOA Where CPF = '{pCpf}'");
        }

        public int Post(PessoaModel pRegistro)
        {
            PessoaModel p = this.Get(pRegistro.Cpf);
            if (p != null)
                return p.Id;
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<PessoaModel>($@"Insert into PESSOA
                                                    (ID, NOME, SOBRENOME, SEXO, CPF)
                                                Values
                                                    (@Id, @Nome, @Sobrenome, @Sexo, @Cpf)", pRegistro);
            return pRegistro.Id;
        }

        public void Put(PessoaModel pRegistro, bool alterarSomenteNome = false)
        {
            this.ExecuteCommand<PessoaModel>($@"Update PESSOA Set
                                                    NOME = @Nome,
                                                    SOBRENOME = @Sobrenome{(alterarSomenteNome ? "" : $@",
                                                    SEXO = @Sexo,
                                                    CPF = @Cpf")}
                                                Where ID = @Id", pRegistro);
        }

        public void AtualizaAltura(int pId, decimal pAltura)
        {
            this.ExecuteCommand<PessoaModel>($@"Update PESSOA Set
                                                    ALTURA = @Altura
                                                Where ID = @Id", new PessoaModel() { Id = pId, Altura = pAltura});
        }

        public int? CalculaIdade(DateTime? pDataNascimento)
        {
            if (!pDataNascimento.HasValue)
                return null;
            else if (pDataNascimento.Value >= DateTime.Now)
            {
                return null;
            }
            else
            {
                int idade = DateTime.Now.Year - pDataNascimento.Value.Year;
                if (DateTime.Now.DayOfYear < pDataNascimento.Value.DayOfYear)
                    --idade;
                return idade;
            }
        }
    }
}
