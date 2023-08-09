using Core.Results.Abstract;

namespace Core.Results.Concrete
{
    public class DataResult<TData> : Result, IDataResult<TData>
    {
        public DataResult(TData data, bool internalServerError = false) : this(data: data, message: string.Empty, internalServerError: internalServerError)
        {

        }

        public DataResult(TData data, string message, bool internalServerError = false) : this(data: data, message: message, isSuccess: false, internalServerError: internalServerError)
        {

        }

        public DataResult(TData data, bool isSuccess, bool internalServerError = false) : this(data: data, message: string.Empty, isSuccess: isSuccess, internalServerError: internalServerError)
        {

        }

        public DataResult(TData data, string message, bool isSuccess, bool internalServerError = false) : base(message: message, isSuccess: isSuccess, internalServerError: internalServerError)
        {
            Data = data;
        }

        public TData Data { get; }
    }
}
