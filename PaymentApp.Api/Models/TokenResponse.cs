using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentApp.Api.Models
{
    public class TokenResponse
    {
        public DateTime ExpireAt { get; set; }
        public string Token { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
