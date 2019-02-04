using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.ComplexTypes
{
    public class FrameworkException:Exception
    {
        public FrameworkException(string message) : base(message)
        {

        }

        public FrameworkException(string message, object result) : base(message)
        {
            ResultValue = result;
        }

        public object ResultValue { get; private set; }
    }
}
