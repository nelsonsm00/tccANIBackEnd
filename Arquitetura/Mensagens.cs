namespace ANI.Arquitetura
{
    public abstract class Mensagens
    {
        //1 - 50 | METAS
        public const string a0001 = "A meta não será inserida pois não foi definido um objetivo maior que zero.";
        public const string a0002 = "A meta não será inserida pois não foi definido um objetivo.";

        //1 - 50 | METAS
        public const string e0001 = "O tipo da meta é inválido.";
        public const string e0002 = "O tipo da meta é inválido: [{0}]";

        //51 - 80 | TRATAMENTO
        public const string e0051 = "Paciente já possui um tratamento.";

        //81 - 100 | PACIENTE
        public const string e0081 = "Paciente não pode ser excluído, pois possui um tratamento.";

        //101 - 120 | ALIMENTO
        public const string e0101 = "TCA inválida.";

        //121 - 130 | REFEICAO
        public const string e0121 = "Esta refeição não pode ser excluída, pois está vinculada à planos alimentares.";

        //131 - 135 | USUARIO
        public const string e0131 = "É necessário informar o login ou o e-mail para cadastrar o usuário.";

        //136 - 140 | ORIENTACAO
        public const string e0136 = "Esta orientação não pode ser excluída, pois está vinculada à algum tratamento.";

        //141 - 170 | CONSULTA
        public const string e0141 = "Consulta não pode ser excluída, pois já foi realizada.";
    }
}
