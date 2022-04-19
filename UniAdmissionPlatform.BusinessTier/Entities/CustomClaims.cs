using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace UniAdmissionPlatform.BusinessTier.Entities
{
    public class CustomClaims
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public int? UniversityId { get; set; }
        public int? HighSchoolId { get; set; }
        public int? OrganizationId { get; set; }
        public long BufferTime { get; set; }
        public long Exp { get; set; }
        public string Iss { get; set; }
        public long Nbf { get; set; }

        public static CustomClaims FromJwtSecurityToken(JwtSecurityToken token)
        {
            var claims = token.Claims.ToList();

            var universityIdClaim = claims.FirstOrDefault(c => c.Type == "university_id");
            var highSchoolIdClaim = claims.FirstOrDefault(c => c.Type == "high_school_id");
            var organizationIdClaim = claims.FirstOrDefault(c => c.Type == "organization_id");

            return new CustomClaims
            {
                // Thieu buffer time
                UserId = int.Parse(claims.First(c => c.Type == "user_id").Value),
                Role = claims.First(c => c.Type == "role").Value,
                UniversityId = universityIdClaim == null ? null : int.Parse(universityIdClaim.Value),
                HighSchoolId = highSchoolIdClaim == null ? null : int.Parse(highSchoolIdClaim.Value),
                OrganizationId = organizationIdClaim == null ? null : int.Parse(organizationIdClaim.Value),
                Exp = long.Parse(claims.First(c => c.Type == "exp").Value),
                Iss = claims.First(c => c.Type == "iss").Value,
                Nbf = long.Parse(claims.First(c => c.Type == "nbf").Value)
            };
        }

        public IEnumerable<Claim> ToEnumerableClaims()
        {
            var identifyName = "auth_id";
            var identifyId = "non";

            if (UniversityId != null)
            {
                identifyName = "university_id";
                identifyId = UniversityId.ToString();
            }
            else if (HighSchoolId != null)
            {
                identifyName = "high_school_id";
                identifyId = HighSchoolId.ToString();
            }
            else if (OrganizationId != null)
            {
                identifyName = "organization_id";
                identifyId = OrganizationId.ToString();
            }

            return new[]
            {
                new Claim("user_id", UserId.ToString()),
                new Claim("role", Role ?? ""),
                new Claim(identifyName, identifyId),
                new Claim("buffer_time", BufferTime.ToString()),
                new Claim("exp", Exp.ToString()),
                new Claim("iss", Iss),
                new Claim("nbf", Nbf.ToString())
            };
        }
    }
}