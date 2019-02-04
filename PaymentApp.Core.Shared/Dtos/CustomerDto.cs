using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Shared.Dtos
{
    public class CustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Id { get; set; }
    }
}
