using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Major;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly IMajorService _majorService;

        public MajorsController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        /// <summary>
        /// Get list majors
        /// </summary>
        /// <response code="200">
        /// Get list majors successfully
        /// </response>
        /// <response code="400">
        /// Get list majors fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Majors" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllMajor([FromQuery] MajorBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allMajor = await _majorService.GetAllMajor(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<MajorViewModelWithMajorGroup>>.OkWithDetail(allMajor, $"Đạt được thành công"));
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
        /// Get major by id
        /// </summary>
        /// <response code="200">
        /// Get major successfully
        /// </response>
        /// <response code="400">
        /// Get major fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Majors" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> GetMajorById(int id)
        {
            try
            {
                var major = await _majorService.GetMajorById(id);
                return Ok(MyResponse<MajorBaseViewModel>.OkWithDetail(major, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Create major
        /// </summary>
        /// <response code="200">
        /// Create major successfully
        /// </response>
        /// <response code="400">
        /// Create major fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Majors" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]")]
        public async Task<IActionResult> CreateMajor([FromBody] CreateMajorRequest createMajorRequest)
        {
            try
            {
                var majorId = await _majorService.CreateMajor(createMajorRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {majorId}, $"Tạo ngành thành công với id = {majorId}"));
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
        /// Update major
        /// </summary>
        /// <response code="200">
        /// Update major successfully
        /// </response>
        /// <response code="400">
        /// Update major fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin - Majors" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{id:int}")]
        public async Task<IActionResult> UpdateMajor(int id, [FromBody] UpdateMajorRequest updateMajorRequest)
        {
            try
            {
                await _majorService.UpdateMajor(id, updateMajorRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Delete major by id
        /// </summary>
        /// <response code="200">
        /// Delete major successfully
        /// </response>
        /// <response code="400">
        /// Delete major fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin - Majors" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{id:int}")]
        public async Task<IActionResult> DeleteMajorById(int id)
        {
            try
            {
                await _majorService.DeleteById(id);
                return Ok(MyResponse<object>.OkWithData("Xóa thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
    }
}