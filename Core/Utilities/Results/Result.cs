using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {

        public Result(bool success, string message):this(success) //2 parametre gönderilirse aşağıdaki metot da çalışır. buradaki this sınıfın kendisini kasteder yani result
        {
            //Success = success; bu metot aşağıdakini de kapsar. o yüzden burayı sileriz. 
            Message = message;
        }
        public Result(bool success)//mesaj vermek istemeyebilirim metotlarda- overloading
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
