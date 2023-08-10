using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDal:IEfEntityRepository<User>
    {
        /// <summary>
        /// Returns all claims of user
        /// </summary>
        /// <param name="user">User to get claims</param>
        /// <returns></returns>
        Task<List<OperationClaim>> GetOperationClaimsAsync(User user);
        /// <summary>
        /// This is transactional delete method of the user. All claims will delete of the user 
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(User user);
        Task<User> AddUserAsDoctorAsync(User user, Doctor doctor);
    }
}
