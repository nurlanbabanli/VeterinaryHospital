using Core.Results.Abstract;

namespace Core.Results.Concrete
{
    public class SuccessResult : Result, IResult
    {
        public SuccessResult(string message) : base(message: message, isSuccess: true)
        {

        }

        public SuccessResult() : base(isSuccess: true)
        {

        }
    }
}
