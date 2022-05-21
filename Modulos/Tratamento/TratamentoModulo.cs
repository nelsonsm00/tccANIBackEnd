using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Pessoa;
using ANI.Models.Tratamento;
using ANI.Modulos.Pessoa;

namespace ANI.Modulos.Tratamento
{
    public class TratamentoModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_TRATAMENTO";

        private PessoaModulo pessoaModulo;

        public TratamentoModulo() : this(null) { }

        public TratamentoModulo(DataBase? db) : base(db) 
        {
            this.pessoaModulo = new PessoaModulo(this.getDataBase());
        }

        public TratamentoModel Get(int pId)
        {
            return this.ExecuteQuery<TratamentoModel>($@"   Select  ID Id,
                                                                    NUTRICIONISTA Nutricionista,
                                                                    PACIENTE Paciente,
                                                                    OBSERVACAO Observacao,
                                                                    MOTIVO Motivo,
                                                                    OBJETIVO Objetivo
                                                            From TRATAMENTO
                                                            Where ID = {pId}");
        }

        public List<TratamentoListagemModel> GetList(int pNutricionista)
        {
            return this.ExecuteQueryList<TratamentoListagemModel>($@"   Select  T.ID Id,
                                                                                P.NOME + ' ' + P.SOBRENOME PacienteNome,
                                                                                (Select MAX(C.DATA) 
                                                                                 From CONSULTA C 
                                                                                 Where T.ID = C.TRATAMENTO and C.REALIZADA = 'S') UltimaConsultaDataBD,
                                                                                (Select MIN(C.DATA) 
                                                                                 From CONSULTA C 
                                                                                 Where T.ID = C.TRATAMENTO and C.REALIZADA = 'N') ProximaConsultaDataBD
                                                                        From TRATAMENTO T
                                                                            inner join PACIENTE PAC on
                                                                                (T.PACIENTE = PAC.ID)
                                                                            inner join PESSOA P on
                                                                                (PAC.ID = P.ID)                                                                            
                                                                        Where T.NUTRICIONISTA = {pNutricionista}
                                                                            and T.ATIVO = 'S'");
        }

        public string GetNome(int pId)
        {
            TratamentoModel tratamento = this.Get(pId);
            if (tratamento != null)
            {
                PessoaModel p = this.pessoaModulo.Get(tratamento.Paciente);
                return p.Nome + " " + p.Sobrenome; 
            }
            return null;
        }

        public int? AnamnesePreenchida(int pId)
        {
            int formulario =
                this.ExecuteScalar<int>($@"Select FORMULARIO From TRATAMENTO_FORMULARIO
                                            Where TRATAMENTO = {pId} 
                                                and FORMULARIO IN (Select ID 
                                                                   From FORMULARIO
                                                                   Where TIPO = 'A' and CONTA = {this.Conta})");
            return formulario > 0 ? formulario: null;
        }

        public int Post(TratamentoModel pRegistro)
        {
            if (this.PacientePossuiTratamento(pRegistro.Paciente, pRegistro.Nutricionista))
                throw new ANIException(Mensagens.e0051);

            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<TratamentoModel>($@" Insert into TRATAMENTO
                                                            (ID, CONTA, NUTRICIONISTA, PACIENTE, OBSERVACAO, MOTIVO, OBJETIVO)
                                                        Values
                                                            (@Id, {this.Conta}, @Nutricionista, @Paciente, @Observacao, @Motivo, @Objetivo)", pRegistro);
            return pRegistro.Id;

        }

        public void Post(TratamentoPlanoAlimentarModel pRegistro)
        {
            this.ExecuteCommand<TratamentoPlanoAlimentarModel>($@" Insert into TRATAMENTO_PLANO_ALIMENTAR
                                                            (TRATAMENTO, PLANO_ALIMENTAR, CONTA)
                                                        Values
                                                            (@Tratamento, @PlanoAlimentar, {this.Conta})", pRegistro);
        }

        public void Put(TratamentoModel pRegistro)
        {
            this.ExecuteCommand<TratamentoModel>($@" Update TRATAMENTO Set
                                                            OBSERVACAO = @Observacao, MOTIVO = @Motivo, OBJETIVO = @Objetivo
                                                     Where ID = @Id", pRegistro);
        }      
        
        public void Inativar(int pId)
        {
            this.ExecuteCommand<TratamentoModel>($@" Update TRATAMENTO Set
                                                            ATIVO = 'N'
                                                     Where ID = @Id", new TratamentoModel() { Id = pId });
        }

        public bool PacientePossuiTratamento(int pPaciente, int? pNutricionista = null)
        {
            return this.ExisteRegistro("TRATAMENTO", $@"PACIENTE = {pPaciente} {(pNutricionista.HasValue ? $@"and NUTRICIONISTA= {pNutricionista} and ATIVO = 'S'" : "")}");
        }

        public void DeleteTratamentoPlanoAlimentar(int pPlanoAlimentar)
        {
            this.ExecuteCommand<TratamentoPlanoAlimentarModel>($@"Delete From TRATAMENTO_PLANO_ALIMENTAR
                                                                  Where PLANO_ALIMENTAR = @PlanoAlimentar", 
                                                                  new TratamentoPlanoAlimentarModel() { PlanoAlimentar = pPlanoAlimentar });
        }
    }
}
