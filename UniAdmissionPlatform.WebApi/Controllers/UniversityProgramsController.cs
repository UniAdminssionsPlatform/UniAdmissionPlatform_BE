using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.UniversityProgram;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class UniversityProgramsController : ControllerBase
    {
        private readonly IUniversityProgramService _universityProgramService;
      
        

        public UniversityProgramsController(IUniversityProgramService universityProgramService)
        {
            _universityProgramService = universityProgramService;
           
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "University Programs" })]
        [Route("~/api/v{version:apiVersion}/university-programs/university-program-admissions")]
        public async Task<IActionResult> GetUniversityProgramAdmissions([FromQuery] int university, [FromQuery] int schoolYearId)
        {
            var universityProgramAdmissions = await _universityProgramService.GetUniversityAdmissionProgram(university, schoolYearId);
            return Ok(MyResponse<ListUniversityProgramAdmission>.OkWithDetail(universityProgramAdmissions, "Đạt được thành công."));
        }

        /// <summary>
        /// Get a university program by id
        /// </summary>
        /// <response code="200">
        /// Get a university program by id successfully
        /// </response>
        /// <response code="400">
        /// Get a university program by id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "University Programs" })]
        [Route("~/api/v{version:apiVersion}/university-programs/{universityProgramId:int}")]
        public async Task<IActionResult> GetTagById(int universityProgramId)
        {
            try
            {
                var universityProgram = await _universityProgramService.GetUniversityProgramById(universityProgramId);
                return Ok(MyResponse<UniversityProgramBaseViewModel>.OkWithDetail(universityProgram, "Tìm kiếm thành công!"));
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
        /// Get list university programs
        /// </summary>
        /// <response code="200">
        /// Get list university programs successfully
        /// </response>
        /// <response code="400">
        /// Get list university programs fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "University Programs" })]
        [Route("~/api/v{version:apiVersion}/university-programs")]
        public async Task<IActionResult> GetAllUniversityProgram([FromQuery] UniversityProgramWithMajorDepartmentAndSchoolYearModel filter, string sort, int page, int limit)
        {
            try
            {
                var allUniversityProgram = await _universityProgramService.GetAllUniversityProgramWithDetail(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>>.OkWithDetail(allUniversityProgram, $"Đạt được thành công"));
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
        /// Get list university programs by university id
        /// </summary>
        /// <response code="200">
        /// Get list university programs by university id successfully
        /// </response>
        /// <response code="400">
        /// Get list university programs by university id fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "University Programs" })]
        [Route("~/api/v{version:apiVersion}/universities/{universityId:int}/university-programs")]
        public async Task<IActionResult> GetAllUniversityProgramByUniversityId(int universityId, string sort, int page, int limit)
        {
            try
            {
                var allUniversityProgram = await _universityProgramService.GetAllUniversityProgramWithDetailByUniversityId(universityId, sort, page, limit);
                return Ok(MyResponse<PageResult<UniversityProgramWithMajorDepartmentAndSchoolYearModel>>.OkWithDetail(allUniversityProgram, $"Đạt được thành công"));
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
        /// Create a new university program
        /// </summary>
        /// <response code="200">
        /// Create a new university program successfully
        /// </response>
        /// <response code="400">
        /// Create a new university program fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin University - University Programs" })]
        [Route("~/api/v{version:apiVersion}/admin-university/university-programs")]
        public async Task<IActionResult> CreateUniversityProgram([FromBody] CreateUniversityProgramRequest createUniversityProgramRequest)
        {
            
            try
            {
                var universityProgramId = await _universityProgramService.CreateUniversityProgram(createUniversityProgramRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {universityProgramId}, $"Tạo chương trình học ngành học thành công với id = {universityProgramId}"));
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
        /// Update a university program
        /// </summary>
        /// <response code="200">
        /// Update a university program successfully
        /// </response>
        /// <response code="400">
        /// Update a university program fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - University Programs" })]
        [Route("~/api/v{version:apiVersion}/admin-university/university-programs/{universityProgramId:int}")]
        public async Task<IActionResult> UpdateUniversityProgram(int universityProgramId, [FromBody] UpdateUniversityProgramRequest updateUniversityProgramRequest)
        {
            try
            {
                await _universityProgramService.UpdateUniversityProgram(universityProgramId, updateUniversityProgramRequest);
                return Ok(MyResponse<object>.OkWithMessage($"Cập nhập thành công chương trình học id = {universityProgramId}."));
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
        /// Delete a university program by id
        /// </summary>
        /// <response code="200">
        /// Delete a university program successfully
        /// </response>
        /// <response code="400">
        /// Delete a university program fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin University - University Programs" })]
        [Route("~/api/v{version:apiVersion}/admin-university/university-programs/{universityProgramId:int}")]
        public async Task<IActionResult> DeleteUniversityProgramById(int universityProgramId)
        {
            try
            {
                await _universityProgramService.DeleteUniversityProgramById(universityProgramId);
                return Ok(MyResponse<object>.OkWithData($"Xóa thành công  chương trình học id = {universityProgramId}."));
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