using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceService _eventTypeService;
        
        public ProvincesController(IProvinceService eventTypeService)
        {
            _eventTypeService = eventTypeService;
            
        }
        
        /// <summary>
        /// Get a list provinces
        /// </summary>
        /// <response code="200">
        /// Get a list provinces successfully
        /// </response>
        /// <response code="400">
        /// Get a list provinces fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Provinces" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetListProvince([FromQuery] ProvinceBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var eventTypes = await _eventTypeService.GetAllProvinces(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<ProvinceBaseViewModel>>.OkWithDetail(eventTypes, $"Đạt được thành công"));
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