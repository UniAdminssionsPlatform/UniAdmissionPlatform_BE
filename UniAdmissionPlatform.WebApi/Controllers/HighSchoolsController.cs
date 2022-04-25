using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class HighSchoolsController : ControllerBase
    {
        private readonly IHighSchoolService _highSchoolService;

        public HighSchoolsController(IHighSchoolService highSchoolService)
        {
            _highSchoolService = highSchoolService;
        }
        
        
        
        /// <summary>
        /// Get a specific high school name by code
        /// </summary>
        /// <response code="200">
        /// Get a specific high school name by code successfully
        /// </response>
        /// <response code="400">
        /// Get a specific high school name by code fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/[controller]/get-by-code")]
        public async Task<IActionResult> GetHighSchoolByCode(string highSchoolCode)
        {
            try
            {
                var highSchools = await _highSchoolService.GetHighSchoolByCode(highSchoolCode);
                return Ok(MyResponse<HighSchoolCodeViewModel>.OkWithDetail(highSchools, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server ís error");
                }
            }
        }
    }
}