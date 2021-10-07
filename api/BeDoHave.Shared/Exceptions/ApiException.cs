using System;

namespace BeDoHave.Shared.Exceptions
{
    public class ApiException : Exception
    {
        public int Code { get; }

        public ApiException(string message, int code)
            : base(message)
        {
            Code = code;
        }
    }
}
