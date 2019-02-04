using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentApp.Core.Shared.Types;

namespace PaymentApp.Api.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {

        public BaseController()
        {

        }

        protected Task<ApiResponse> ApiResponseResult(object result, string message = "")
        {
            return Task.Run(() => new ApiResponse
            {
                Result = result,
                Code = StatusCodes.Status200OK,
                Message = message,
                Version = "v1"
            });
        }
    }
}