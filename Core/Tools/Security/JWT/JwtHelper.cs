using Core.Entities.Concrete;
using Core.Extensions;
using Core.Results.Abstract;
using Core.Results.Concrete;
using Core.Tools.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tools.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private readonly TokenOptions _tokenOptions;

        public JwtHelper(IConfiguration configuration)
        {
            _tokenOptions=configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public IDataResult<AccessToken> CreateToken(User user, List<OperationClaim> claims)
        {
            if (_tokenOptions==null) return new ErrorDataResult<AccessToken>(null, message: "Create token error", internalServerError: true);

            var tokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialHelper.GetSigningCredentials(securityKey);
            var jwtSecurityToken=CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, claims);
            var token=(new JwtSecurityTokenHandler()).WriteToken(jwtSecurityToken);

            return new SuccessDataResult<AccessToken>(new AccessToken { Token=token, Expiration=tokenExpiration});
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            return new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration),
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
                ); 
        }

        private List<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName(user.FirstName);
            claims.AddLastName(user.LastName);
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }
    }
}
