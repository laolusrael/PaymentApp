using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApp.UI.Models.Interfaces
{
    public interface ICreditCardChecker
    {
        Task<bool> CheckCard(string cc);
        Task<CCResponse> GetCardDetails(string cc);
    }
}
