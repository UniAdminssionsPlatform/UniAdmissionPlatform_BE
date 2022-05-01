using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.StudentCertificaiton;
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
        

        public StudentController(IStudentCertificationService studentStudentCertificationService, IAuthService authService)
        {
            _studentStudentCertificationService = studentStudentCertificationService;
            _authService = authService;
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
        public async Task<IActionResult> GetAllStudentCertification([FromQuery] StudentCertificationBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allStudentCertification = await _studentStudentCertificationService.GetAllStudentCertification(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<StudentCertificationBaseViewModel>>.OkWithDetail(allStudentCertification, $"Đạt được thành công"));
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
        public async Task<IActionResult> CreateStudentCertification([FromBody] CreateStudentCertificationRequest createStudentCertificationRequest)
        {
            var studentId = _authService.GetUserId(HttpContext);
            try
            {
                var studentStudentCertificationId = await _studentStudentCertificationService.CreateStudentCertification(studentId, createStudentCertificationRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {studentStudentCertificationId}, $"Tạo chứng chỉ thành công với id = {studentStudentCertificationId}"));
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
        public async Task<IActionResult> UpdateStudentCertification(int studentId, int certificationId, [FromBody] UpdateStudentCertificationRequest updateStudentCertificationRequest)
        {
            try
            {
                await _studentStudentCertificationService.UpdateStudentCertification(studentId, certificationId, updateStudentCertificationRequest);
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
    }
}