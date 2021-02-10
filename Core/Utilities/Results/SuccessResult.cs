using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class SuccessResult:Result
    {
        public SuccessResult(string message):base(true,message) //base result burada. returnun 2 parametreli olanını çalıştırır.
        {

        }

        public SuccessResult() : base(true) //base'in(yani result'ın) tek parametereli olanını çalıştırır.
        {

        }
    }
}
