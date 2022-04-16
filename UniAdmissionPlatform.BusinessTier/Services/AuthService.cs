using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Entities;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IAuthService
    {
        int GetUserId(HttpContext httpContext);
        string GetUserRole(HttpContext httpContext);
    }
    public class AuthService : IAuthService
    {
        public int GetUserId(HttpContext httpContext)
        {
            var claims = (CustomClaims) httpContext.Items["claims"];
            return claims.UserId;
        }

        public string GetUserRole(HttpContext httpContext)
        {
            var claims = (CustomClaims) httpContext.Items["claims"];
            return claims.Role;
        }
    }
}