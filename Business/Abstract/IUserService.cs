using Core.Entities.Concrete;
using Core.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<User>> AddAsync(User user);
        Task<IDataResult<User>> GetByEmailAsync(string email);
        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);
        Task<IResult> DeleteUserAsync(int userId);
    }
}
