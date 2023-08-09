using Core.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Results.Concrete
{
    public class Result : IResult
    {
        public Result(string message, bool isSuccess, bool internalServerError = false) : this(isSuccess: isSuccess, internalServerError: internalServerError)
        {
            Message = message;
        }

        public Result(bool isSuccess, bool internalServerError = false) : this(internalServerError: internalServerError)
        {
            IsSuccess = isSuccess;
        }

        public Result(bool internalServerError = false)
        {
            InternalServerError = internalServerError;
        }

        public string Message { get; }
        public bool IsSuccess { get; }
        public bool InternalServerError { get; }
    }
}
