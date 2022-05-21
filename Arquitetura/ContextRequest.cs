using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANI.Arquitetura
{
    public interface IFacadeRequest
    {
        IHttpContextAccessor ObterRequest();
    }


    public class ContextRequest
    {
        static IFacadeRequest _facadeRequest;

        public static void Init(IFacadeRequest facadeRequest)
        {
            _facadeRequest = facadeRequest;
        }

        public static string ContextConta()
        {
            var request = _facadeRequest.ObterRequest();
            var authHeader = request.HttpContext.Request.Headers.TryGetValue("Conta", out var ret);
            return ret;
        }      

        public bool StatusRequest()
        {
            try
            {
                var request = _facadeRequest.ObterRequest();

                if (request.HttpContext != null && request.HttpContext.Request != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


    }
}