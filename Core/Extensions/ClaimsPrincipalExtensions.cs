using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result=claimsPrincipal?.FindAll(claimType)?.Select(claim => claim.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims("roles");
        }

        public static string GetClaimEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.Email).Value;
        }

        public static string GetClaimId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst("userId").Value;
        }
    }
}
