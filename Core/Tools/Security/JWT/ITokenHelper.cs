using Core.Entities.Concrete;
using Core.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tools.Security.JWT
{
    public interface ITokenHelper
    {
        IDataResult<AccessToken> CreateToken(User user, List<OperationClaim> claims);
    }
}
