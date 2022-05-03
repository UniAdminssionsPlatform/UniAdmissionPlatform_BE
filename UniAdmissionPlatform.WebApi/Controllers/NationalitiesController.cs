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
    public class NationalitiesController : ControllerBase
    {
        private readonly INationalityService _nationalityService;
        
        public NationalitiesController(INationalityService nationalityService)
        {
            _nationalityService = nationalityService;
        }
        
        /// <summary>
        /// Get list nationalities
        /// </summary>
        /// <response code="200">
        /// Get list nationalities successfully
        /// </response>
        /// <response code="400">
        /// Get list nationalities fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Nationalities" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetListTag([FromQuery] NationalityBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var tags = await _nationalityService.GetAllNationalities(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<NationalityBaseViewModel>>.OkWithDetail(tags, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server is error");
                }
            }
        }
    }
}