namespace Hale.Core.Utils
{
    using System;
    using System.Net;
    using System.Net.Http;
    // using System.Web.Http.Filters;
    using Hale.Core.Model.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;

    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new ExceptionDTO
            {
                Message = context.Exception.Message,
#if DEBUG
                StackTrace = context.Exception.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
#endif
            })
            {
               StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
