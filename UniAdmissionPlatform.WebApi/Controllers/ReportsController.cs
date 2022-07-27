using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Entities;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Attributes;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IConfiguration _configuration;

        public ReportsController(IReportService reportService, IConfiguration configuration)
        {
            _reportService = reportService;
            _configuration = configuration;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Report" })]
        [Route("~/api/v{version:apiVersion}/get-student-record-setting")]
        public IActionResult GetSetting(int eventId, string token)
        {
            var reportSetting = _reportService.GetReportSetting(eventId, token);
            return Ok(reportSetting);
        }
        
        

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Report" })]
        [Route("~/api/v{version:apiVersion}/student-report")]
        public IActionResult GetStudentReport(int eventId, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SigningKey"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            // attach user to context on successful jwt validation
            var claims = CustomClaims.FromJwtSecurityToken(jwtToken);
            return Ok(_reportService.GetStudentReport(eventId, 
                claims.UniversityId!.Value));
        }
        
    }
}