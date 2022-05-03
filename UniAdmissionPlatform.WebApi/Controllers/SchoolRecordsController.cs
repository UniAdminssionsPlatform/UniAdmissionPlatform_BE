using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.SchoolRecord;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class SchoolRecordsController : ControllerBase
    {
        private readonly ISchoolRecordService _schoolRecordService;
        private readonly IAuthService _authService;
        

        public SchoolRecordsController(ISchoolRecordService schoolRecordService, IAuthService authService)
        {
            _schoolRecordService = schoolRecordService;
            _authService = authService;
        }

        /// <summary>
        /// Get list school records
        /// </summary>
        /// <response code="200">
        /// Get list school records successfully
        /// </response>
        /// <response code="400">
        /// Get list school records fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - School Records" })]
        [Route("~/api/v{version:apiVersion}/student/school-record")]
        public async Task<IActionResult> GetAllSchoolRecord([FromQuery] SchoolRecordBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allSchoolRecord = await _schoolRecordService.GetAllSchoolRecord(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<SchoolRecordBaseViewModel>>.OkWithDetail(allSchoolRecord, $"Đạt được thành công"));
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
        /// Create a new school records
        /// </summary>
        /// <response code="200">
        /// Create a new school records successfully
        /// </response>
        /// <response code="400">
        /// Create a new school records fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Student - School Records" })]
        [Route("~/api/v{version:apiVersion}/student/school-record")]
        public async Task<IActionResult> CreateSchoolRecord([FromBody] CreateSchoolRecordRequest createSchoolRecordRequest)
        {
            var studentId = _authService.GetUserId(HttpContext);
            try
            {
                var schoolRecordId = await _schoolRecordService.CreateSchoolRecord(studentId, createSchoolRecordRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {schoolRecordId}, $"Tạo phiếu điểm(School Record) thành công với id = {schoolRecordId}"));
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
        /// Update a school records
        /// </summary>
        /// <response code="200">
        /// Update a school records successfully
        /// </response>
        /// <response code="400">
        /// Update a school records fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Student - School Records" })]
        [Route("~/api/v{version:apiVersion}/student/school-record/{schoolRecordId:int}")]
        public async Task<IActionResult> UpdateSchoolRecord(int schoolRecordId, [FromBody] UpdateSchoolRecordRequest updateSchoolRecordRequest)
        {
            var studentId = _authService.GetUserId(HttpContext);
            try
            {
                await _schoolRecordService.UpdateSchoolRecord(schoolRecordId, studentId, updateSchoolRecordRequest);
                return Ok(MyResponse<object>.OkWithMessage($"Cập nhập thành công phiếu điểm(School Record) id = {schoolRecordId}."));
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
        /// Delete a school records by id (not done yet)
        /// </summary>
        /// <response code="200">
        /// Delete a school records successfully
        /// </response>
        /// <response code="400">
        /// Delete a school records fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Student - School Records" })]
        [Route("~/api/v{version:apiVersion}/student/school-record/{schoolRecordId:int}")]
        public async Task<IActionResult> DeleteSchoolRecordById(int schoolRecordId)
        {
            var studentId = _authService.GetUserId(HttpContext);
            try
            {
                await _schoolRecordService.DeleteSchoolRecordById(schoolRecordId, studentId);
                return Ok(MyResponse<object>.OkWithData($"Xóa thành công phiếu điểm(School Record) id = {schoolRecordId}."));
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