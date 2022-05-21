using ANI.Arquitetura;
using ANI.Models.Configuracao;

namespace ANI.Modulos.Configuracao
{
    public class ConfiguracaoModulo : ModuloBase
    {
        public ConfiguracaoModulo() : this(null) { }

        public ConfiguracaoModulo(DataBase? db) : base(db) { }

        public  ConfiguracaoModel Get(int pNutricionista)
        {
            return this.ExecuteQuery<ConfiguracaoModel>($@" Select  NUTRICIONISTA Nutricionista,
                                                                    HORARIO_INICIO HorarioInicioBD,
                                                                    HORARIO_FINAL HorarioFinalBD,
                                                                    DURACAO Duracao
                                                            From CONFIGURACAO 
                                                            Where NUTRICIONISTA = {pNutricionista}");
        }

        public List<string> GetHorarios(int pNutricionista)
        {
            List<string> lista = new List<string>();
            ConfiguracaoModel configuracao = this.Get(pNutricionista);
            if (configuracao == null)
                configuracao = new ConfiguracaoModel();

            if (!configuracao.HorarioInicioBD.HasValue)
                configuracao.HorarioInicioBD = new DateTime(2020, 1, 1, 0, 0, 0);

            if (!configuracao.HorarioFinalBD.HasValue)
                configuracao.HorarioFinalBD = new DateTime(2020, 1, 1, 23, 59, 0);

            if (!configuracao.Duracao.HasValue)
                configuracao.Duracao = 15;

            DateTime hora = configuracao.HorarioInicioBD.Value;
            while(hora.CompareTo(configuracao.HorarioFinalBD) < 0)
            {
                lista.Add(hora.ToString("HH:mm"));
                hora = hora.AddMinutes(configuracao.Duracao.Value);
            }
            return lista;
        }
    }
}
