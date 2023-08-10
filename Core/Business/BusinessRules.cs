using Core.Results.Abstract;
using Core.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class BusinessRules
    {
        public static IResult RunRules(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.IsSuccess)
                {
                    return logic;
                }
            }
            return new SuccessResult();
        }
    }
}
