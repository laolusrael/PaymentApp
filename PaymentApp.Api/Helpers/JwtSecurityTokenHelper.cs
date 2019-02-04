using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaymentApp.Api.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentApp.Api.Helpers
{
    public class JwtSecurityTokenHelper
    {
        public static TokenResponse CreateToken(IConfiguration configuration)
        {
            var key = configuration["Jwt:key"];
            var issuer = configuration["Jwt:issuer"];
            var audience = configuration["Jwt:audience"];
            var duration = Int32.Parse(configuration["Jwt:duration"]);

            if (duration == 0) duration = 1;

            var expireAt = DateTime.Now.AddHours(duration);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"sample")
            };

            var token = _JwtTokenBuilder(key, issuer, audience, "sample", expireAt, claims);

            return new TokenResponse
            {
                ExpireAt = expireAt,
                Status = System.Net.HttpStatusCode.OK,
                Token = token
            };
        }

    private static string _JwtTokenBuilder(string key, string issuer, string audience, string username, DateTime expireAt, List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    signingCredentials: credentials,
                    expires: expireAt,
                    claims: claims,
                    notBefore: DateTime.Now);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);


    }

    public static TokenValidationParameters GetTokenParameters(IConfiguration configuration)
    {
        return new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = configuration["Jwt:issuer"],
            ValidAudience = configuration["Jwt:audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
        };

    }
}}
