using Microsoft.AspNetCore.Mvc;

namespace ANI.Arquitetura.Erro
{
    public abstract class GerenciadorErro
    {       
        public GerenciadorErro()
        {
        }

        public static ObjectResult RetornaMensagemDeErro(Exception e)
        {

            if (e is ANIException)
            {
                ANIException aE = (ANIException) e;
                return new ObjectResult(aE.GetErrorObject())
                {
                    StatusCode = aE.GetStatusCode()
                };
            }

            string detailedErrorMessage = e.GetType().Name + ": " + e.ToString();
            string errorMessage = e.Message;

            var resultException = new
            {
                MensagemDeErro = errorMessage,
                DetalhesDoErro = detailedErrorMessage
            };
            var resposta = new ObjectResult(resultException)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            return resposta;
        }
    }
}
