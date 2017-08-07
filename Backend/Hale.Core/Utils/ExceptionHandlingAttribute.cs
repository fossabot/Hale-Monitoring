using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Hale.Core.Utils
{
    class ExceptionHandlingAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                    new ExceptionDTO
                    {
                        Message = context.Exception.Message,
                        #if DEBUG
                            StackTrace = context.Exception.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                        #endif
                    }
                ))
            };
        }
    }

    public class ExceptionDTO
    {
        public string Message { get; set; }
        public string[] StackTrace { get; set; }
    }
}
