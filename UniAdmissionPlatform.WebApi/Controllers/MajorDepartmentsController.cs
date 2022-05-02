using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.MajorDepartment;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class MajorDepartmentsController : ControllerBase
    {
        private readonly IMajorDepartmentService _majorDepartmentService;
        private readonly IAuthService _authService;
        

        public MajorDepartmentsController(IMajorDepartmentService majorDepartmentService, IAuthService authService)
        {
            _majorDepartmentService = majorDepartmentService;
            _authService = authService;
        }

        /// <summary>
        /// Get list major departments
        /// </summary>
        /// <response code="200">
        /// Get list major departments successfully
        /// </response>
        /// <response code="400">
        /// Get list major departments fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin University - Major Department" })]
        [Route("~/api/v{version:apiVersion}/admin-university/major-department")]
        public async Task<IActionResult> GetAllMajorDepartment([FromQuery] MajorDepartmentBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allMajorDepartment = await _majorDepartmentService.GetAllMajorDepartment(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<MajorDepartmentBaseViewModel>>.OkWithDetail(allMajorDepartment, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Lấy thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Lấy thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        

        /// <summary>
        /// Create a new major departments
        /// </summary>
        /// <response code="200">
        /// Create a new major departments successfully
        /// </response>
        /// <response code="400">
        /// Create a new major departments fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin University - Major Department" })]
        [Route("~/api/v{version:apiVersion}/admin-university/major-department")]
        public async Task<IActionResult> CreateMajorDepartment([FromBody] CreateMajorDepartmentRequest createMajorDepartmentRequest)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                var majorDepartmentId = await _majorDepartmentService.CreateMajorDepartment(universityId, createMajorDepartmentRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {majorDepartmentId}, $"Tạo phòng ban ngành học thành công với id = {majorDepartmentId}"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

        /// <summary>
        /// Update a major departments
        /// </summary>
        /// <response code="200">
        /// Update a major departments successfully
        /// </response>
        /// <response code="400">
        /// Update a major departments fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Major Department" })]
        [Route("~/api/v{version:apiVersion}/admin-university/major-department/{majorDepartmentId:int}")]
        public async Task<IActionResult> UpdateMajorDepartment(int majorDepartmentId, [FromBody] UpdateMajorDepartmentRequest updateMajorDepartmentRequest)
        {
            try
            {
                await _majorDepartmentService.UpdateMajorDepartment(majorDepartmentId, updateMajorDepartmentRequest);
                return Ok(MyResponse<object>.OkWithMessage($"Cập nhập thành công phòng ban id = {majorDepartmentId}."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

        /// <summary>
        /// Delete a major departments by id
        /// </summary>
        /// <response code="200">
        /// Delete a major departments successfully
        /// </response>
        /// <response code="400">
        /// Delete a major departments fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin University - Major Department" })]
        [Route("~/api/v{version:apiVersion}/admin-university/major-department/{majorDepartmentId:int}")]
        public async Task<IActionResult> DeleteMajorDepartmentById(int majorDepartmentId)
        {
            try
            {
                await _majorDepartmentService.DeleteMajorDepartmentById(majorDepartmentId);
                return Ok(MyResponse<object>.OkWithData($"Xóa thành công  phòng ban id = {majorDepartmentId}."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Xóa thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "xóa thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
    }
}