using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Shared.Dtos
{
    public class PaymentDto
    {
        public string CreditCardNumber { get; set; }
        public int OrderId { get; set; }
    }
}
