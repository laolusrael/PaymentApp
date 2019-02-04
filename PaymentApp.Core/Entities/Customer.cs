using PaymentApp.Core.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Entities
{
    public class Customer
    {
        public Customer():base()
        {
            
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
