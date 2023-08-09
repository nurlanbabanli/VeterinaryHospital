using Core.Results.Abstract;
using Core.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Results
{
    public static class DataResultHandler
    {
        /// <summary>
        /// Result is null when check is successed
        /// </summary>
        /// <typeparam name="TData">DataResult to check</typeparam>
        /// <typeparam name="TResult">DataResult to return by caller method</typeparam>
        /// <param name="dataResult">DataResult to check</param>
        /// <returns></returns>
        public static IDataResult<TResult> FilterDataResult<TData, TResult>(IDataResult<TData> dataResult)
        {
            if (dataResult==null) return new ErrorDataResult<TResult>(data: default(TResult), message: "Get "+nameof(dataResult.Data)+" error", internalServerError: true);
            if (dataResult.InternalServerError) return new ErrorDataResult<TResult>(data: default(TResult), message: dataResult.Message, internalServerError: true);
            if (!dataResult.IsSuccess) return new ErrorDataResult<TResult>(data: default(TResult), message: dataResult.Message);

            return null;
        }

        /// <summary>
        /// Result is null when check is successed
        /// </summary>
        /// <typeparam name="TResult">DataResult to return by caller method</typeparam>
        /// <param name="result">Result to check</param>
        /// <returns></returns>
        public static IDataResult<TResult> FilterResult<TResult>(IResult result)
        {
            if (result==null) return new ErrorDataResult<TResult>(data: default(TResult), internalServerError: true);
            if (result.InternalServerError) return new ErrorDataResult<TResult>(data: default(TResult), message: result.Message, internalServerError: true);
            if (!result.IsSuccess) return new ErrorDataResult<TResult>(data: default(TResult), message: result.Message);

            return null;
        }
    }
}
