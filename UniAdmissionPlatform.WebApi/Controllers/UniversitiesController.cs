using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.HighSchool;
using UniAdmissionPlatform.BusinessTier.Requests.University;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class UniversitiesController : ControllerBase
    {
        private readonly IUniversityService _universityService;
        private readonly IProvinceService _provinceService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UniversitiesController(IUniversityService universityService, IProvinceService provinceService, IAuthService authService, IUserService userService)
        {
            _universityService = universityService;
            _provinceService = provinceService;
            _authService = authService;
            _userService = userService;
        }
        
         /// <summary>
        /// Get a specific universities name by code
        /// </summary>
        /// <response code="200">
        /// Get a specific universities name by code successfully
        /// </response>
        /// <response code="400">
        /// Get a specific universities name by code fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Universities" })]
        [Route("~/api/v{version:apiVersion}/[controller]/get-by-manager-code")]
        public async Task<IActionResult> GetUniversityByCode(string universityCode)
        {
            try
            {
                var universities = await _universityService.GetUniversityNameByCode(universityCode);
                return Ok(MyResponse<UniversityCodeViewModel>.OkWithDetail(universities, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server is error");
                }
            }
        }

        /// /// <summary>
        /// Get list universities
        /// </summary>
        /// <response code="200">
        /// Get list universities successfully
        /// </response>
        /// <response code="400">
        /// Get list universities fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Universities" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllUniversities([FromQuery] UniversityBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var tags = await _universityService.GetAllUniversities(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<UniversityBaseViewModel>>.OkWithDetail(tags, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server is error");
                }
            }
        }

        /// <summary>
        /// Get university by iD
        /// </summary>
        /// <response code="200">
        /// Get university by iD successfully
        /// </response>
        /// <response code="400">
        /// Get university by iD fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Universities" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{universityId:int}")]
        public async Task<IActionResult> GetUniversityByID(int universityId)
        {
            try
            {
                var universityById = await _universityService.GetUniversityByID(universityId);
                return Ok(MyResponse<UniversityBaseViewModel>.OkWithDetail(universityById, "Truy cập thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server is error");
                }
            }
        }


        /// <summary>
        /// Create a new university profile
        /// </summary>
        /// <response code="200">
        /// Create a new university profile successfully
        /// </response>
        /// <response code="400">
        /// Create a new university profile fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin/accounts/university")]
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityRequest createUniversityRequest)
        {
            try
            {
                await _provinceService.GetProvinceByID(createUniversityRequest.ProvinceId);
                var universityId = await _universityService.CreateUniversity(createUniversityRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { universityId },
                    $"Tạo trường đại học thành công với id = {universityId}"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Tạo thất bại. " + e.Error.Message);
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Tạo thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }
        
        /// <summary>
        /// Update a university profile
        /// </summary>
        /// <response code="200">
        /// Update a university profile successfully
        /// </response>
        /// <response code="400">
        /// Update a university profile fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Account" })]
        [Route("~/api/v{version:apiVersion}/admin-university/profile")]
        public async Task<IActionResult> UpdateHighSchoolProfile([FromBody] UpdateUniversityProfileRequest updateUniversityProfileRequest)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                await _universityService.UpdateUniversityProfile(universityId, updateUniversityProfileRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{universityId}, $"Cập nhập tài khoản thành công với ID = {universityId}"));
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
        /// Get list pending manager for university
        /// </summary>
        /// <response code="200">
        /// Get list pending manager for university successfully
        /// </response>
        /// <response code="400">
        /// Get list pending manager for university fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin University - Account" })]
        [Route("~/api/v{version:apiVersion}/admin-university/accounts/list")]
        public async Task<IActionResult> GetUniversityManagerStatusPending([FromQuery] UserAccountBaseViewModel filter, string sort, int page, int limit)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                var accountManager = await _userService.GetUniversityManagerStatusPending(filter, sort, page, limit, universityId);
                return Ok(MyResponse<PageResult<UserAccountBaseViewModel>>.OkWithDetail(accountManager, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Lấy danh sách thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Lấy danh sách thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Update status to active university manager
        /// </summary>
        /// <response code="200">
        /// Update status to active university manager successfully
        /// </response>
        /// <response code="400">
        /// Update status to active university manager fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Account" })]
        [Route("~/api/v{version:apiVersion}/admin-university/accounts/switch-status")]
        public async Task<IActionResult> SetActiveForUniversityAdmin(int userId)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                await _userService.SetActiveForUniversityAdmin(userId, universityId);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập trạng thái thành công!"));
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