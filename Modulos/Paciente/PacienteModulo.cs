using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Paciente;
using ANI.Models.Usuario;
using ANI.Modulos.Pessoa;
using ANI.Modulos.Tratamento;
using ANI.Modulos.Usuario;

namespace ANI.Modulos.Paciente
{
    public class PacienteModulo : ModuloBase
    {
        private PessoaModulo pessoaModulo;
        private TratamentoModulo tratamentoModulo;
        private UsuarioModulo usuarioModulo;

        public PacienteModulo() : this(null) { }

        public PacienteModulo(DataBase? db) : base(db)
        {
            this.pessoaModulo = new PessoaModulo(this.getDataBase());
            this.tratamentoModulo = new TratamentoModulo(this.getDataBase());
            this.usuarioModulo = new UsuarioModulo(this.getDataBase());
        }

        public PacienteModel Get(int pId, int? pTratamento = null)
        {
            string condicao = pTratamento != null ? $@"T.ID = {pTratamento.Value}" : $@"PA.ID = {pId}";
            PacienteModel paciente = this.ExecuteQuery<PacienteModel>(
                                                    $@"    Select {PessoaModulo.GetColunas("P")},
                                                                    U.EMAIL Email
                                                                From PACIENTE PA 
                                                                    inner join PESSOA P on (PA.ID = P.ID)
                                                                    left join USUARIO U on (P.ID = U.ID)
                                                                    left join TRATAMENTO T on (PA.ID = T.PACIENTE)
                                                                Where {condicao}");

            if (paciente != null)
                paciente.Idade = this.pessoaModulo.CalculaIdade(paciente.DataNascimentoBD);
            return paciente;
        }

        public List<PacienteListagemModel> GetList(int pNutricionista)
        {
            return this.ExecuteQueryList<PacienteListagemModel>($@"     Select  PAC.ID Id,
                                                                                P.NOME Nome,
                                                                                P.SOBRENOME Sobrenome,
                                                                                (Select MAX(C.DATA) 
                                                                                 From CONSULTA C 
                                                                                 Where T.ID = C.TRATAMENTO and C.REALIZADA = 'S') UltimaConsultaDataBD,
                                                                                (Select MIN(C.DATA) 
                                                                                 From CONSULTA C 
                                                                                 Where T.ID = C.TRATAMENTO and C.REALIZADA = 'N') ProximaConsultaDataBD,
                                                                                U.EMAIL Email
                                                                        From PACIENTE PAC 
                                                                            left join TRATAMENTO T on
                                                                                (PAC.ID = T.PACIENTE and T.ATIVO = 'S')
                                                                            inner join PESSOA P on
                                                                                (PAC.ID = P.ID)      
                                                                            left join USUARIO U on
                                                                                (P.ID = U.ID)
                                                                        Where T.NUTRICIONISTA = {pNutricionista} or PAC.CONTA = {this.Conta}");
        }

        public int Post(PacienteModel pRegistro)
        {
            pRegistro.Id = this.pessoaModulo.Post(pRegistro);
            this.usuarioModulo.Post(new UsuarioModel() { Id = pRegistro.Id, Email = pRegistro.Email });
            this.ExecuteCommand<PacienteModel>($@"  Insert into PACIENTE
                                                        (ID, CONTA)
                                                    Values
                                                        (@Id, {this.Conta})", pRegistro);
            return pRegistro.Id;

        }

        public void Put(PacienteModel pRegistro)
        {
            this.pessoaModulo.Put(pRegistro, true);
            this.usuarioModulo.PutEmail(pRegistro.Id, pRegistro.Email);
        }

        public void Delete(int pId)
        {
            if (this.tratamentoModulo.PacientePossuiTratamento(pId))
                throw new ANIException(Mensagens.e0081);

            this.ExecuteCommand<PacienteModel>($@"Delete from PACIENTE Where ID = @Id", new PacienteModel { Id = pId });
        }
    }
}
