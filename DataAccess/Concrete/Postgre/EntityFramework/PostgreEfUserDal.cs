using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Tools.LocalLogger;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Postgre.EntityFramework
{
    public class PostgreEfUserDal : EfEntityRepositoryBase<User, PostgreDbContext>, IUserDal
    {
        public async Task<User> AddUserAsDoctorAsync(User user, Doctor doctor)
        {
            if (user == null || doctor==null) throw new ArgumentNullException("User is null");
            using (var context=new PostgreDbContext())
            {
                using (var transaction=context.Database.BeginTransaction())
                {
                    try
                    {
                        var addedUser=context.Entry(user);
                        addedUser.State= EntityState.Added;

                        var addedDoctor=context.Entry(doctor);
                        addedDoctor.State=EntityState.Added;
                        await context.SaveChangesAsync();

                        return addedUser.Entity;
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        LocalLogHandler.Log(exception.Message, LogLevel.Error);
                        return null;
                    }
                }
            }
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            if (user==null) return false;
            using (var context = new PostgreDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry<User>(user).State=EntityState.Deleted;

                        var userClaims = context.UserOperationsClaims.Where(x => x.UserId==user.Id);
                        context.UserOperationsClaims.RemoveRange(userClaims);

                        //this user is a doctor. delete record from Doctors table
                        if (user.UserType==1)
                        {
                            var doctor=context.Doctors.SingleOrDefault(x=>x.UserId==user.Id);
                            context.Entry<Doctor>(doctor).State=EntityState.Deleted;
                        }

                        var saveChangesResult = await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        LocalLogHandler.Log(exception.Message, LogLevel.Error);
                        return false;
                    }
                }
            }
        }

        public async Task<List<OperationClaim>> GetOperationClaimsAsync(User user)
        {
            using (var context = new PostgreDbContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationsClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name, };
                return await result.ToListAsync();
            }
        }
    }
}
