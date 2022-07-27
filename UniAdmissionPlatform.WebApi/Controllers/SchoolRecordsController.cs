using System;
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
        private readonly ISchoolYearService _schoolYearService;

        

        public SchoolRecordsController(ISchoolRecordService schoolRecordService, IAuthService authService, ISchoolYearService schoolYearService)
        {
            _schoolRecordService = schoolRecordService;
            _authService = authService;
            _schoolYearService = schoolYearService;
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
        /// Delete a school records by id
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
        
        /// <summary>
        /// Get list school years
        /// </summary>
        /// <response code="200">
        /// Get list school years successfully
        /// </response>
        /// <response code="400">
        /// Get list school years fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - School Years" })]
        [Route("~/api/v{version:apiVersion}/student/school-years")]
        public async Task<IActionResult> GetAllSchoolYears([FromQuery] SchoolYearBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allSchoolRecord = await _schoolYearService.GetAllSchoolYears(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<SchoolYearBaseViewModel>>.OkWithDetail(allSchoolRecord, $"Đạt được thành công"));
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
        /// Get school years by id
        /// </summary>
        /// <response code="200">
        /// Get school years by id successfully
        /// </response>
        /// <response code="400">
        /// Get school years by id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - School Years" })]
        [Route("~/api/v{version:apiVersion}/student/school-years/{schoolYearId:int}")]
        public async Task<IActionResult> GetSchoolYearById(int schoolYearId)
        {
            try
            {
                var schoolYearById = await _schoolYearService.GetSchoolYearById(schoolYearId);
                return Ok(MyResponse<SchoolYearBaseViewModel>.OkWithDetail(schoolYearById, "Tìm kiếm thành công!"));
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
        /// Get excel template for import school records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/v{version:apiVersion}/school-records/import-excel-template")]
        public IActionResult GetImportSchoolRecordExcel()
        {
            var file = _schoolRecordService.GetImportSchoolRecordExcel();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "school-record");
        }

        /// <summary>
        /// Import school record
        /// </summary>
        /// <response code="200">
        /// Import school record successfully
        /// </response>
        /// <response code="400">
        /// Import school record fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Student - School Records" })]
        [Route("~/api/v{version:apiVersion}/student/school-records/{schoolYearId:int}/import-excel-template")]
        public async Task<IActionResult> ImportSchoolRecordExcel(int schoolYearId, IFormFile file)
        {
            try
            {
                var userId = _authService.GetUserId(HttpContext);
                var schoolRecordId = await _schoolRecordService.ImportSchoolRecord(userId, schoolYearId, file);
                return Ok(MyResponse<object>.OkWithDetail(new {schoolRecordId}, "Thành công."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get scores by school record id
        /// </summary>
        /// <response code="200">
        /// Get scores by school record id successfully
        /// </response>
        /// <response code="400">
        /// Get scores by school record id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - School Records" })]
        [Route("~/api/v{version:apiVersion}/student/school-record/get-score")]
        public async Task<IActionResult> GetScoreOfStudent(int schoolYearId)
        {
            var userId = _authService.GetUserId(HttpContext);
            try
            {
                var schoolRecord = await _schoolRecordService.GetByIdAndStudentId(schoolYearId, userId);
                return Ok(MyResponse<SchoolRecordWithStudentRecordItemModel>.OkWithDetail(schoolRecord, "Đạt được thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

    }
}