using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApp.Core.Shared.Types
{
    public class ApiErrorResponse
    {
        public string Version = "v1";
        public int Code;
        public string Message;
    }
}
