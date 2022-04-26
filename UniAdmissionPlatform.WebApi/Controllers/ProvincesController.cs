using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IProvinceService _provinceTypeService;
        
        public ProvincesController(IProvinceService provinceTypeService)
        {
            _provinceTypeService = provinceTypeService;
            
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
                var provinces = await _provinceTypeService.GetAllProvinces(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<ProvinceBaseViewModel>>.OkWithDetail(provinces, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get province by iD
        /// </summary>
        /// <response code="200">
        /// Get province by iD successfully
        /// </response>
        /// <response code="400">
        /// Get province by iD fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Provinces" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{provinceId:int}")]
        public async Task<IActionResult> GetProvinceByID(int provinceId)
        {
            try
            {
                var provinces = await _provinceTypeService.GetProvinceByID(provinceId);
                return Ok(MyResponse<ProvinceBaseViewModel>.OkWithDetail(provinces, "Truy cập thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
    }
}