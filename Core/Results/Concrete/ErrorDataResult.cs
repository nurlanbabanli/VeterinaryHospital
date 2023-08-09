using Core.Results.Abstract;

namespace Core.Results.Concrete
{
    public class ErrorDataResult<TData> : DataResult<TData>, IDataResult<TData>
    {

        public ErrorDataResult(TData data, string message, bool internalServerError = false) : base(data: data, message: message, isSuccess: false, internalServerError: internalServerError)
        {
        }

        public ErrorDataResult(TData data, bool internalServerError = false) : base(data: data, isSuccess: false, internalServerError: internalServerError)
        {
        }

    }
}
