using AutoMapper;
using Business.Abstract;
using Business.Rules;
using Business.Validation.FluentValidation;
using Core.Aspect.Autofac.Exception;
using Core.Aspect.Autofac.Logging;
using Core.Aspect.Autofac.Validation;
using Core.Business;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Results;
using Core.Results.Abstract;
using Core.Results.Concrete;
using Core.Tools.Security.Hashing;
using Core.Tools.Security.JWT;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IMapper _autoMapper;
        private readonly ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, IMapper autoMapper, ITokenHelper tokenHelper)
        {
            _userService=userService;
            _autoMapper=autoMapper;
            _tokenHelper=tokenHelper;
        }

        [ExceptionLogAspect(typeof(MssqlLogger), skipValueLog: true, Priority = 3)]
        [LogAspects(typeof(MssqlLogger), skipValueLog: true, Priority = 4)]
        [ValidationAspect(typeof(UserLoginDtoValidator), Priority = 2)]
        public async Task<IDataResult<UserLoginResponseDto>> LoginAsync(UserLoginDto userLoginDto)
        {
            var userDataResult = await _userService.GetByEmailAsync(userLoginDto.Email);
            var filterResultUser = DataResultHandler.FilterDataResult<User, UserLoginResponseDto>(userDataResult);
            if (filterResultUser!=null) return filterResultUser;

            var ruleCheckResult = BusinessRules.RunRules(AuthRules.CheckUserIsActive(userDataResult.Data));
            var filterResultRuleCheck = DataResultHandler.FilterResult<UserLoginResponseDto>(ruleCheckResult);
            if (filterResultRuleCheck!=null) return filterResultRuleCheck;

            var passwordHashSalt = new PasswordHashSaltDto
            {
                PasswordSalt=userDataResult.Data.PasswordSalt,
                PasswordHash=userDataResult.Data.PasswordHash
            };
            var isPasswordVerified = HashingHelper.VerifyPasswordHash(userLoginDto.Password, passwordHashSalt);
            if (!isPasswordVerified) return new ErrorDataResult<UserLoginResponseDto>(null, "Password is not correct");

            var operationClaimsDataResult = await _userService.GetClaimsAsync(userDataResult.Data);
            var filterResultOperationClaims = DataResultHandler.FilterDataResult<List<OperationClaim>, UserLoginResponseDto>(operationClaimsDataResult);
            if (filterResultOperationClaims!=null) return filterResultOperationClaims;

            var accessTokenDataResult = _tokenHelper.CreateToken(userDataResult.Data, operationClaimsDataResult.Data);
            var filterResultToken = DataResultHandler.FilterDataResult<AccessToken, UserLoginResponseDto>(accessTokenDataResult);
            if (filterResultToken!=null) return filterResultToken;

            var userRolesDataResult = GetUserRoles(operationClaimsDataResult.Data);
            var filterResultUserRoles = DataResultHandler.FilterDataResult<List<string>, UserLoginResponseDto>(userRolesDataResult);
            if (filterResultUserRoles!=null) return filterResultUserRoles;

            var userLoginResponseDto = new UserLoginResponseDto
            {
                UserId=userDataResult.Data.Id,
                Token=accessTokenDataResult.Data.Token,
                TokenExpiration=accessTokenDataResult.Data.Expiration,
                Email=userDataResult.Data.Email,
                FirstName=userDataResult.Data.FirstName,
                LastName=userDataResult.Data.LastName,
                Roles=userRolesDataResult.Data
            };

            return new SuccessDataResult<UserLoginResponseDto>(userLoginResponseDto, "User login successed");
        }


        [ExceptionLogAspect(typeof(MssqlLogger), skipValueLog: true, Priority = 3)]
        [LogAspects(typeof(MssqlLogger), skipValueLog: true, Priority = 4)]
        [ValidationAspect(typeof(RegisterUserDtoValidator), Priority = 2)]
        public async Task<IDataResult<UserDto>> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var ruleCheckResult = BusinessRules.RunRules(await AuthRules.IsUserEmailExitsAsync(_userService, userRegisterDto),
    AuthRules.CheckPasswordLength(userRegisterDto));
            var filterResultRuleCheck = DataResultHandler.FilterResult<UserDto>(ruleCheckResult);
            if (filterResultRuleCheck!=null) return filterResultRuleCheck;

            var paswordHashSalt = HashingHelper.CreatePasswordHash(userRegisterDto.Password);

            var user = _autoMapper.Map<UserRegisterDto, User>(userRegisterDto);
            user.PasswordSalt = paswordHashSalt.PasswordSalt;
            user.PasswordHash=paswordHashSalt.PasswordHash;
            user.IsActive=false;

            var addUserResult = await _userService.AddAsync(user);
            var filterResultAddedUser = DataResultHandler.FilterDataResult<User, UserDto>(addUserResult);
            if (filterResultAddedUser!=null) return filterResultAddedUser;

            var userDtoResult = _autoMapper.Map<User, UserDto>(addUserResult.Data);

            return new SuccessDataResult<UserDto>(userDtoResult);
        }


        private IDataResult<List<string>> GetUserRoles(List<OperationClaim> operationClaims)
        {
            if (operationClaims==null) return new ErrorDataResult<List<string>>(null, "User roles not find");

            var userRoles = new List<string>();
            foreach (var claims in operationClaims)
            {
                userRoles.Add(claims.Name);
            }
            return new SuccessDataResult<List<string>>(userRoles);
        }
    }
}
