using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Shared.Dtos
{
    public class DateConfirmationDto
    {
        public DateTime DateOfBirth { get; set; }
        public int OrderId { get; set; }
    }
}
