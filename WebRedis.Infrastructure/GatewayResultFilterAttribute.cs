using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebRedis.Infrastructure
{
    public class GatewayResultFilterAttribute : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is EmptyResult)
            {
                context.Result = new JsonResult(new CodeResult());
            }
            else if (context.Result is ContentResult)
            {
            }
            else if (context.Result is ObjectResult)
            {
                var objresult = context.Result as ObjectResult;
                if ((objresult.Value as CodeResult) != null)
                {
                    return;
                }
                CodeResult<object> result = new CodeResult<object>(objresult.Value);
                context.Result = new JsonResult(result);
            }
        }
    }
}
