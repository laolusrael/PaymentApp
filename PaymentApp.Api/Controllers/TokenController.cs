using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using PaymentApp.Api.Helpers.Security;
using PaymentApp.Api.Models.Types;
using PaymentApp.Core.Dtos.Security;
using PaymentApp.Core.Enums;
using PaymentApp.Core.Interfaces.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PaymentApp.Api.Controllers
{
    /// <summary>
    /// Token Controller allows you to generate a token for registered users on the platform
    /// </summary>
    [Produces("application/json")]
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private readonly IConfiguration Configuration;
        private readonly IIdentityService _identityService;

        public TokenController(IConfiguration config, IIdentityService idsvc)
        {
            Configuration = config;
            _identityService = idsvc;
        }

        /// <summary>
        /// Create a token for given user that can be later used to access the application services.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenResponse> CreateToken([FromBody]TokenRequestDto model)
        {
            // Validate user credentials and return token
            IActionResult response = Unauthorized();

            if (model == null)
            {
                throw new Exception("request details required");
            }

            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Username))
            {
                throw new Exception("Incomplete credentials");
            }

            var result = await _identityService.GetUserAccountAsync(model);

            if (result.Status == AuthStatus.Authenticated) {

                // return Json(JwtSecurityTokenHelper.CreateToken(Configuration, result.UserDetails));
                return JwtSecurityTokenHelper.CreateToken(Configuration, result.UserDetails);

                // return await ApiResponseResult(JwtSecurityTokenHelper.CreateToken(Configuration, result.UserDetails), "Token created");


            }

            return new TokenResponse
            {
                Token = "",
                Status = System.Net.HttpStatusCode.OK,
                UserDetails = result.UserDetails
            };
        }
    }
}