using Core.Results.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<UserDto>>RegisterAsync(UserRegisterDto userRegisterDto);
        Task<IDataResult<UserLoginResponseDto>> LoginAsync(UserLoginDto userLoginDto);
    }
}
