using Business.Abstract;
using Business.Validation.FluentValidation;
using Core.Aspect.Autofac.Exception;
using Core.Aspect.Autofac.Logging;
using Core.Aspect.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Results.Abstract;
using Core.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal=userDal;
        }

        [ExceptionLogAspect(typeof(MssqlLogger), skipValueLog: true, Priority = 3)]
        [LogAspects(typeof(MssqlLogger), skipValueLog: true, Priority = 4)]
        [ValidationAspect(typeof(UserValidator), Priority = 2)]
        public async Task<IDataResult<User>> AddAsync(User user)
        {
            if (user.UserType==1)
            {
                var addedUser = await _userDal.AddAsync(user);
                if (addedUser==null) return new ErrorDataResult<User>(null, internalServerError: true);
                return new SuccessDataResult<User>(addedUser);
            }

            if(user.UserType==2)
            {

            }

            return new ErrorDataResult<User>(null, "Not implemented error", internalServerError: true);
        }

        [ExceptionLogAspect(typeof(MssqlLogger), skipValueLog: true, Priority = 3)]
        [LogAspects(typeof(MssqlLogger), skipValueLog: true, Priority = 4)]
        public async Task<IResult> DeleteUserAsync(int userId)
        {
            var userToDelete = await _userDal.GetAsync(x => x.Id==userId);
            if (userToDelete==null) return new ErrorResult("User not found");

            var userDeleteResult = await _userDal.DeleteUserAsync(userToDelete);

            return null;
        }

        [ExceptionLogAspect(typeof(MssqlLogger), skipValueLog: true, Priority = 3)]
        [LogAspects(typeof(MssqlLogger), skipValueLog: true, Priority = 4)]
        public async Task<IDataResult<User>> GetByEmailAsync(string email)
        {
            var userResult = await _userDal.GetAsync(x => x.Email==email);
            if (userResult==null) return new ErrorDataResult<User>(null, "User not found");

            return new SuccessDataResult<User>(userResult);
        }

        [ExceptionLogAspect(typeof(MssqlLogger), skipValueLog: true, Priority = 3)]
        [LogAspects(typeof(MssqlLogger), skipValueLog: true, Priority = 4)]
        [ValidationAspect(typeof(UserValidator), Priority = 2)]
        public async Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user)
        {
            var claims = await _userDal.GetOperationClaimsAsync(user);
            if (claims==null) return new ErrorDataResult<List<OperationClaim>>(null, internalServerError: true);

            return new SuccessDataResult<List<OperationClaim>>(claims);
        }
    }
}
