using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tools.Security.Hashing
{
    public class HashingHelper
    {
        public static PasswordHashSaltDto CreatePasswordHash(string password)
        {
            var result=new PasswordHashSaltDto();
            using (var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                result.PasswordSalt=hmac.Key;
                result.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return result;
            }
        }

        public static bool VerifyPasswordHash(string password,PasswordHashSaltDto passwordHashSaltDto)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512(passwordHashSaltDto.PasswordSalt))
            {
                var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i]!=passwordHashSaltDto.PasswordHash[i]) return false;
                }
                hmac.Clear();
            }
            return true;
        }
    }
}
