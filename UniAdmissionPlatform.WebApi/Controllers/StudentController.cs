using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.StudentCertification;
using UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentCertificationService _studentStudentCertificationService;
        private readonly IAuthService _authService;
        private readonly IStudentRecordItemService _studentRecordItemService;


        public StudentController(IStudentCertificationService studentStudentCertificationService,
            IAuthService authService, IStudentRecordItemService studentRecordItemService)
        {
            _studentStudentCertificationService = studentStudentCertificationService;
            _authService = authService;
            _studentRecordItemService = studentRecordItemService;
        }

        /// <summary>
        /// Get list student certifications
        /// </summary>
        /// <response code="200">
        /// Get list student certifications successfully
        /// </response>
        /// <response code="400">
        /// Get list student certifications fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]/certifications")]
        public async Task<IActionResult> GetAllStudentCertification(
            [FromQuery] StudentCertificationBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allStudentCertification =
                    await _studentStudentCertificationService.GetAllStudentCertification(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<StudentCertificationBaseViewModel>>.OkWithDetail(
                    allStudentCertification, $"Đạt được thành công"));
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
        /// Get student certification by id
        /// </summary>
        /// <response code="200">
        /// Get student certification by id successfully
        /// </response>
        /// <response code="400">
        /// Get student certification by id fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]/certifications/{certificationId:int}")]
        public async Task<IActionResult> GetStudentCertificationByCertificationIdForStudent(int certificationId)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                var studentCertification =
                    await _studentStudentCertificationService.GetStudentCertificationById(certificationId, userId);
                return Ok(MyResponse<StudentCertificationBaseViewModel>.OkWithDetail(studentCertification,
                    "Đạt được thành công."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Lấy thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

        /// <summary>
        /// Create a new student certification
        /// </summary>
        /// <response code="200">
        /// Create a new student certification successfully
        /// </response>
        /// <response code="400">
        /// Create a new student certification fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Student - Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]/certifications")]
        public async Task<IActionResult> CreateStudentCertification(
            [FromBody] CreateStudentCertificationRequest createStudentCertificationRequest)
        {
            var studentId = _authService.GetUserId(HttpContext);
            try
            {
                var studentStudentCertificationId =
                    await _studentStudentCertificationService.CreateStudentCertification(studentId,
                        createStudentCertificationRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { studentStudentCertificationId },
                    $"Tạo chứng chỉ thành công với id = {studentStudentCertificationId}"));
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
        /// Update a student certification
        /// </summary>
        /// <response code="200">
        /// Update a student certification successfully
        /// </response>
        /// <response code="400">
        /// Update a student certification fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Student - Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]/certifications/{studentId:int}")]
        public async Task<IActionResult> UpdateStudentCertification(int studentId, int certificationId,
            [FromBody] UpdateStudentCertificationRequest updateStudentCertificationRequest)
        {
            try
            {
                await _studentStudentCertificationService.UpdateStudentCertification(studentId, certificationId,
                    updateStudentCertificationRequest);
                return Ok(MyResponse<object>.OkWithMessage($"Cập nhập thành công chứng chỉ id = {certificationId}."));
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
        /// Delete a student certification by id
        /// </summary>
        /// <response code="200">
        /// Delete a student certification successfully
        /// </response>
        /// <response code="400">
        /// Delete a student certification fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Student - Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]/certifications/{studentId:int}")]
        public async Task<IActionResult> DeleteStudentCertificationById(int studentId, int certificationId)
        {
            try
            {
                await _studentStudentCertificationService.DeleteStudentCertificationById(studentId, certificationId);
                return Ok(MyResponse<object>.OkWithData($"Xóa thành công chứng chỉ id = {certificationId}."));
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
        /// Get list student record items
        /// </summary>
        /// <response code="200">
        /// Get list student record items successfully
        /// </response>
        /// <response code="400">
        /// Get list student record items fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Record Items" })]
        [Route("~/api/v{version:apiVersion}/[controller]/record-items")]
        public async Task<IActionResult> GetAllStudentRecordItem([FromQuery] StudentRecordItemBaseViewModel filter,
            string sort, int page, int limit)
        {
            var userId = _authService.GetUserId(HttpContext);
            try
            {
                var allStudentRecordItem =
                    await _studentRecordItemService.GetAllStudentRecordItem(filter, userId, sort, page, limit);
                return Ok(MyResponse<PageResult<StudentRecordItemBaseViewModel>>.OkWithDetail(allStudentRecordItem,
                    $"Đạt được thành công"));
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
        /// Get student record items by iD
        /// </summary>
        /// <response code="200">
        /// Get student record items by iD successfully
        /// </response>
        /// <response code="400">
        /// Get student record items by iD fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Record Items" })]
        [Route("~/api/v{version:apiVersion}/[controller]/record-items/{studentRecordItemId:int}")]
        public async Task<IActionResult> GetStudentRecordItemById(int studentRecordItemId)
        {
            try
            {
                var studentRecordItemById =
                    await _studentRecordItemService.GetStudentRecordItemById(studentRecordItemId);
                return Ok(MyResponse<StudentRecordItemBaseViewModel>.OkWithDetail(studentRecordItemById,
                    "Truy cập thành công!"));
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
        /// Create a new student record item
        /// </summary>
        /// <response code="200">
        /// Create a new student record item successfully
        /// </response>
        /// <response code="400">
        /// Create a new student record item fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Student - Record Items" })]
        [Route("~/api/v{version:apiVersion}/[controller]/record-items")]
        public async Task<IActionResult> CreateStudentRecordItem(
            [FromBody] CreateStudentRecordItemRequest createStudentRecordItemRequest)
        {
            var userId = _authService.GetUserId(HttpContext);
            try
            {
                var studentRecordItem =
                    await _studentRecordItemService.CreateStudentRecordItem(createStudentRecordItemRequest, userId);
                return Ok(MyResponse<object>.OkWithDetail(new { studentRecordItem },
                    $"Tạo thông tin điểm thành công với id = {studentRecordItem}"));
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
        /// Update a student record item
        /// </summary>
        /// <response code="200">
        /// Update a student record item successfully
        /// </response>
        /// <response code="400">
        /// Update a student record item fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Student - Record Items" })]
        [Route("~/api/v{version:apiVersion}/[controller]/record-items/{studentRecordItemId:int}")]
        public async Task<IActionResult> UpdateStudentRecordItem(int studentRecordItemId,
            [FromBody] UpdateStudentRecordItemRequest updateStudentRecordItemRequest)
        {
            try
            {
                await _studentRecordItemService.UpdateStudentRecordItem(studentRecordItemId,
                    updateStudentRecordItemRequest);
                return Ok(MyResponse<object>.OkWithMessage(
                    $"Cập nhập thành công thông tin điểm id = {studentRecordItemId}."));
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
    }
}