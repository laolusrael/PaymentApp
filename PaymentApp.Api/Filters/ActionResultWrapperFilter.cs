using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApp.Api.Filters
{
    public class ApiResponseWrapper : ResultFilterAttribute
    {

        public override void OnResultExecuted(ResultExecutedContext context)
        {

            var response = context.HttpContext.Response;

            // crete a new 


            base.OnResultExecuted(context);
        }
    }
}
