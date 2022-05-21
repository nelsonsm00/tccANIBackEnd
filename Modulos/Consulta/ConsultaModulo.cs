using ANI.Arquitetura;
using ANI.Arquitetura.Erro;
using ANI.Models.Consulta;
using ANI.Modulos.Medida;
using ANI.Modulos.Meta;
using ANI.Modulos.Pessoa;

namespace ANI.Modulos.Consulta
{
    public class ConsultaModulo : ModuloBase
    {
        private const string SEQUENCE = "SQ_CONSULTA";

        private MetaModulo metaModulo;
        private PessoaModulo pessoaModulo;
        private MedidaModulo medidaModulo;

        public ConsultaModulo() : this(null) { }

        public ConsultaModulo(DataBase? db) : base(db) 
        {
            this.metaModulo = new MetaModulo(this.getDataBase());
            this.pessoaModulo = new PessoaModulo(this.getDataBase());
            this.medidaModulo = new MedidaModulo(this.getDataBase());
        }

        public ConsultaModel Get(int pId)
        {
            return this.ExecuteQuery<ConsultaModel>($@" Select  ID Id,
                                                                TRATAMENTO Tratamento,
                                                                DATA DataBD,
                                                                PESO Peso,
                                                                PRIMEIRA_CONSULTA PrimeiraConsultaBD,
                                                                OBSERVACAO Observacao,
                                                                REALIZADA RealizadaBD
                                                        From CONSULTA 
                                                        Where ID = {pId}");
        }

        public ConsultaDetalheModel GetConsultaNaoRealizada(int pTratamento, int? pConsulta = null)
        {
            ConsultaDetalheModel consulta = this.ExecuteQuery<ConsultaDetalheModel>($@" Select  TOP 1 C.ID Id,
                                                                                                C.TRATAMENTO Tratamento,
                                                                                                C.DATA DataBD,
                                                                                                C.PESO Peso,
                                                                                                C.OBSERVACAO Observacao,
                                                                                                COALESCE(P.ALTURA, 0) Altura,
                                                                                                P.SEXO Sexo,
                                                                                                P.DATA_NASCIMENTO DataNascimento
                                                                                        From CONSULTA C 
                                                                                        inner join TRATAMENTO T on
                                                                                        (C.TRATAMENTO = T.ID)
                                                                                        inner join PACIENTE PA on
                                                                                        (T.PACIENTE = PA.ID)
                                                                                        inner join PESSOA P on
                                                                                        (PA.ID = P.ID)
                                                                                        Where {(pConsulta.HasValue 
                                                                                        ? $@"C.ID = {pConsulta.Value}"
                                                                                        : $@"C.TRATAMENTO = {pTratamento}")}
                                                                                            and C.REALIZADA <> 'S'
                                                                                        Order by C.DATA asc");
            if (consulta != null)
            {
                consulta.Idade = this.pessoaModulo.CalculaIdade(consulta.DataNascimento);
                ConsultaModel consultaAnterior = this.Get(this.ObterConsultaAnteior(consulta.Id, consulta.Tratamento, true));
                if (consultaAnterior != null)
                    consulta.PesoAnterior = consultaAnterior.Peso;
            }
            return consulta;
        }

        public List<ConsultaDetalheModel> GetListaConsultaNaoRealizada(int pTratamento)
        {
            ConsultaDetalheModel consultaAtual = this.GetConsultaNaoRealizada(pTratamento);
            if (consultaAtual != null)
            {
                return this.ExecuteQueryList<ConsultaDetalheModel>($@" Select  ID Id,
                                                                               TRATAMENTO Tratamento,
                                                                               DATA DataBD,
                                                                               PESO Peso                                                                                        
                                                                       From CONSULTA 
                                                                       Where TRATAMENTO = {pTratamento}
                                                                        and REALIZADA <> 'S'
                                                                        and ID <> {consultaAtual.Id}
                                                                       Order by DATA desc");
            }
            return null;
        }

        public List<ConsultaResumoModel> GetListaConsultaRealizada(int pTratamento)
        {
            return this.ExecuteQueryList<ConsultaResumoModel>($@" Select    ID Id,
                                                                            TRATAMENTO Tratamento,
                                                                            DATA DataBD,
                                                                            PESO Peso,
                                                                            OBSERVACAO Observacao
                                                                  From CONSULTA 
                                                                  Where TRATAMENTO = {pTratamento}
                                                                    and REALIZADA = 'S'
                                                                  Order by DATA desc");
        }

        public int ObterConsultaAnteior(int pId, int? pTratamento = null, bool realizada = false)
        {
            ConsultaModel consultaAtual = null;
            if (pTratamento == null)
                consultaAtual = this.Get(pId);

            string realizadaCondicao = realizada ? " and REALIZADA = 'S' " : string.Empty;
           
            return this.ExecuteScalar<int>($@" Select  TOP 1 ID
                                               From CONSULTA 
                                               Where TRATAMENTO = {(pTratamento == null ? consultaAtual.Tratamento : pTratamento.Value)}
                                                and ID <> {pId}
                                                {realizadaCondicao}
                                               Order by DATA desc");
        }

        public int Post(ConsultaModel pRegistro)
        {
            pRegistro.PrimeiraConsultaBD = this.ExisteRegistro("CONSULTA", " TRATAMENTO = " + pRegistro.Tratamento) ? 'S' : 'N';
            pRegistro.Id = this.ExecuteSequence(SEQUENCE);
            this.ExecuteCommand<ConsultaModel>($@" Insert into CONSULTA
                                                            (ID, CONTA, TRATAMENTO, DATA, PRIMEIRA_CONSULTA, OBSERVACAO)
                                                        Values
                                                            (@Id, {this.Conta}, @Tratamento, @DataBD, @PrimeiraConsultaBD, @Observacao)", pRegistro);
            this.medidaModulo.Post(pRegistro.Id);
            return pRegistro.Id;

        }

        private void Put(ConsultaModel pRegistro)
        {
            this.ExecuteCommand<ConsultaModel>($@"  Update  CONSULTA Set
                                                            PESO = @Peso,
                                                            OBSERVACAO = @Observacao
                                                    Where ID = @Id", pRegistro);
        }

        public void Delete(int pId)
        {
            ConsultaModel consulta = this.Get(pId);
            if (consulta != null)
            {
                if (consulta.Realizada)
                    throw new ANIException(Mensagens.e0141);

                //  Deleta os filhos se existir
                this.medidaModulo.Delete(pId);
                
                this.ExecuteCommand<ConsultaModel>($@"  Delete From CONSULTA Where ID = @Id", new ConsultaModel() { Id = pId });
            }
        }

        public void AtualizaPeso(int pId, decimal pPeso)
        {
            this.ExecuteCommand<ConsultaModel>($@" Update CONSULTA Set PESO = @Peso Where ID = @Id", new ConsultaModel() { Id = pId, Peso = pPeso });
            this.AtualizaMetas(pId, pPeso);
        }

        private void AtualizaMetas(int pId, decimal? pPeso = null, decimal? pAlimento = null, string? pDescricao = null, bool? pAtingido = null)
        {
            int consultaAlterior = this.ObterConsultaAnteior(pId);
            if (consultaAlterior > 0)
            {
                this.metaModulo.Atualiza(consultaAlterior, pPeso, pAlimento, pDescricao, pAtingido);
            }
        }

        public void Realiza(int pId)
        {
            this.ExecuteCommand<ConsultaModel>($@" Update CONSULTA Set REALIZADA = 'S' Where ID = @Id", new ConsultaModel() { Id = pId });
        }

        public void Reagenda(ConsultaReagendaModel pRegistro)
        {
            //REGRAS DE REAGENDAMENTO
            this.ExecuteCommand<ConsultaModel>($@" Update CONSULTA Set DATA = @DataBD Where ID = @Id", new ConsultaModel() { Id = pRegistro.Id, DataBD = pRegistro.Data });
        }

        public int GetPacienteId(int pId)
        {
            return this.ExecuteScalar<int>($@"Select T.PACIENTE From
                                                CONSULTA C inner join TRATAMENTO T on
                                                    (C.TRATAMENTO = T.ID)
                                                Where C.ID = {pId}");
        }

        public void Realiza(ConsultaRealizadaModel pRegistro)
        {
            this.Put(new ConsultaModel()
            {
                Id = pRegistro.Id,
                Peso = pRegistro.Peso,
                Observacao = pRegistro.Observacao
            });
            this.Realiza(pRegistro.Id);

            this.pessoaModulo.AtualizaAltura(this.GetPacienteId(pRegistro.Id), pRegistro.Altura);
        }
    }
}
