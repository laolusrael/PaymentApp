using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Interfaces
{
    public interface IValidatable
    {
        bool Validate();
        Dictionary<string, string> Error { get; }
    }
}
