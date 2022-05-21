using ANI.Models.Erro;

namespace ANI.Arquitetura.Erro
{
    public class ANIException : Exception
    {
        public const int CODIGO_ERRO = 500;
        public const int CODIGO_CONDICAO_INVALIDA = 412;

        private int statusCode;

        public ANIException(string pMensagem, int pStatusCode = CODIGO_ERRO) : base(pMensagem)
        {
            this.statusCode = pStatusCode;
        }

        public int GetStatusCode()
        {
            return this.statusCode;
        }

        public object GetErrorObject()
        {
            return new ErroModel()
            {
                Id = 0,
                Mensagem = this.Message
            };
        }
    }
}
