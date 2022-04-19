using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Entities;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IAuthService
    {
        int GetUserId(HttpContext httpContext);
        string GetUserRole(HttpContext httpContext);
        int GetHighSchoolId(HttpContext httpContext);
        int GetUniversityId(HttpContext httpContext);
        int GetOrganizationId(HttpContext httpContext);
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
        
        public int GetHighSchoolId(HttpContext httpContext)
        {
            var claims = (CustomClaims) httpContext.Items["claims"];
            return claims.HighSchoolId ?? 0;
        }
        
        public int GetUniversityId(HttpContext httpContext)
        {
            var claims = (CustomClaims) httpContext.Items["claims"];
            return claims.UniversityId ?? 0;
        }
        
        public int GetOrganizationId(HttpContext httpContext)
        {
            var claims = (CustomClaims) httpContext.Items["claims"];
            return claims.OrganizationId ?? 0;
        }
    }
}