using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictsController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        /// <summary>
        /// Get list districts
        /// </summary>
        /// <response code="200">
        /// Get list districts successfully
        /// </response>
        /// <response code="400">
        /// Get list districts fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "District" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllDistricts([FromQuery] DistrictViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allDistrict = await _districtService.GetAllDistrict(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<DistrictViewModel>>.OkWithData(allDistrict));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Get list district by id
        /// </summary>
        /// <response code="200">
        /// Get list district by id successfully
        /// </response>
        /// <response code="400">
        /// Get list district by id fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "District" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> GetDistrictById(int id)
        {
            try
            {
                var district = await _districtService.GetDistrictById(id);
                return Ok(MyResponse<DistrictViewModel>.OkWithDetail(district, "Đạt được thành công."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

    }
}