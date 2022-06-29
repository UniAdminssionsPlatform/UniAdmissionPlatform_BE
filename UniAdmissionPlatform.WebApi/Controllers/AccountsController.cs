using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Toolkit;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public AccountsController(IAccountService accountService, IFirebaseStorageService firebaseStorageService, IAuthService authService)
        {
            _accountService = accountService;
            _firebaseStorageService = firebaseStorageService;
            _authService = authService;
        }
        
        /// <summary>
        /// Get list student accounts
        /// </summary>
        /// <response code="200">
        /// Get list student accounts successfully
        /// </response>
        /// <response code="400">
        /// Get list student accounts fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        [Route("~/api/v{version:apiVersion}/[controller]/student")]
        public async Task<IActionResult> GetStudentInfo([FromQuery] AccountBaseViewModel filter, int page, int limit, string sort)
        {
            try
            {
                var accounts = await _accountService.GetAllStudents(filter, page, limit, sort);
                return Ok(MyResponse<PageResult<AccountViewModelWithHighSchool>>.OkWithData(accounts));
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
        /// Get list accounts for admin
        /// </summary>
        /// <response code="200">
        /// Get list accounts for admin successfully
        /// </response>
        /// <response code="400">
        /// Get list accounts for admin fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin/accounts")]
        public async Task<IActionResult> GetAllAccountForAdmin([FromQuery] ManagerAccountBaseViewModel filter, int page, int limit, string sort)
        {
            try
            {
                var accounts = await _accountService.GetAllAccountForAdmin(filter, page, limit, sort);
                return Ok(MyResponse<PageResult<ManagerAccountBaseViewModel>>.OkWithData(accounts));
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
        /// Get a student account by id
        /// </summary>
        /// <response code="200">
        /// Get a student account successfully
        /// </response>
        /// <response code="400">
        /// Get a student account fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin High School - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/{studentId:int}")]
        public async Task<IActionResult> GetStudentAccountById(int studentId)
        {
            try
            {
                var studentAccountById = await _accountService.GetStudentAccountById(studentId);
                return Ok(MyResponse<AccountStudentByIdViewModelWithHighSchool>.OkWithDetail(studentAccountById, $"Đạt được thành công"));
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
        /// Get student profile
        /// </summary>
        /// <response code="200">
        /// Get student profile successfully
        /// </response>
        /// <response code="400">
        /// Get student profile fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Account" })]
        [Route("~/api/v{version:apiVersion}/students/[controller]/profile")]
        public async Task<IActionResult> GetStudentProfile()
        {
            var userId = _authService.GetUserId(HttpContext);
            try
            {
                var studentAccountById = await _accountService.GetStudentAccountById(userId);
                return Ok(MyResponse<AccountStudentByIdViewModelWithHighSchool>.OkWithDetail(studentAccountById, $"Đạt được thành công"));
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
        /// Get list university accounts
        /// </summary>
        /// <response code="200">
        /// Get list university accounts successfully
        /// </response>
        /// <response code="400">
        /// Get list university accounts fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        [Route("~/api/v{version:apiVersion}/[controller]/university")]
        public async Task<IActionResult> GetUniversityAccount([FromQuery] AccountBaseViewModel filter, int page, int limit, string sort)
        {
            try
            {
                return Ok(await _accountService.GetAllUniAccount(filter, page, limit, sort));
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
        /// Update a profile
        /// </summary>
        /// <response code="200">
        /// Update a profile successfully
        /// </response>
        /// <response code="400">
        /// Update a profile fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        [Route("~/api/v{version:apiVersion}/me")]
        public async Task<IActionResult> UpdateUniAccount([FromBody] UpdateProfileRequest updateProfileRequest)
        {
            var id = _authService.GetUserId(HttpContext);
            try
            {
                await _accountService.UpdateUniAccount(id, updateProfileRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{id}, $"Cập nhập tài khoản thành công với ID = {id}"));
                // return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Update fail, because server is error");
                }
            }
        }
        
        /// <summary>
        /// Update user account
        /// </summary>
        /// <response code="200">
        /// Update user account successfully
        /// </response>
        /// <response code="400">
        /// Update user account fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{id:int}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountRequestForAdmin updateAccountRequestForAdmin)
        {
            try
            {
                await _accountService.UpdateAccount(id, updateAccountRequestForAdmin);
                return Ok(MyResponse<object>.OkWithDetail(new{id}, $"Cập nhập tài khoản thành công với ID = {id}"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    case StatusCodes.Status400BadRequest:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Update fail, because server is error");
                }
            }
        }
        
        
        /// <summary>
        /// Upload a new avatar
        /// </summary>
        /// <response code="200">
        /// Upload a new avatar successfully
        /// </response>
        /// <response code="400">
        /// Upload a new avatar fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        [Route("~/api/v{version:apiVersion}/me/upload-avatar")]
        [CustomAuthorize]
        public async Task<IActionResult> UpdateAvatar([Required] IFormFile file)
        {
            try
            {
                FileToolKit.ValidateFileName(file, new[] { ".jpg", ".png", ".jpeg" }, FileMaxSize);
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Đổi ảnh đại diện thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message)
                };
            }

            string fileUrl;
            try
            {
                var lastIndexOf = file.FileName.LastIndexOf(".", StringComparison.Ordinal);
                fileUrl = await _firebaseStorageService.UploadImage(file.FileName[lastIndexOf..file.FileName.Length].ToLower(),"avatar",file.OpenReadStream());
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    _ => new GlobalException(ExceptionCode.PrintErrorObjectOut, e.Error.Message)
                };
            }


            var userId = _authService.GetUserId(HttpContext);
            try
            {
                await _accountService.UploadAvatar(userId, fileUrl);

                return Ok(MyResponse<object>.OkWithMessage("Đổi ảnh đại diện thành công."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Đổi ảnh đại diện thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message)
                };
            }
        }
        
        private const long FileMaxSize = 10000000;
        
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
        [SwaggerOperation(Tags = new[] { "Admin University - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin-university/accounts/list")]
        [HiddenObjectParams("account.")]
        public async Task<IActionResult> GetUniversityManagerStatusPending([FromQuery] ManagerAccountBaseViewModel filter, string sort, int page, int limit)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                var accountManager = await _accountService.GetUniversityManagerStatusPending(filter, sort, page, limit, universityId);
                return Ok(MyResponse<PageResult<ManagerAccountBaseViewModel>>.OkWithDetail(accountManager, $"Đạt được thành công"));
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
        /// Get list pending manager for high school
        /// </summary>
        /// <response code="200">
        /// Get list pending manager for high school successfully
        /// </response>
        /// <response code="400">
        /// Get list pending manager for high school fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin High School - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/accounts/list")]
        [HiddenObjectParams("account.")]
        public async Task<IActionResult> GetHighSchoolManagerStatusPending([FromQuery] ManagerAccountBaseViewModel filter, string sort, int page, int limit)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                var accountManager = await _accountService.GetHighSchoolManagerStatusPending(filter, sort, page, limit, highSchoolId);
                return Ok(MyResponse<PageResult<ManagerAccountBaseViewModel>>.OkWithDetail(accountManager, $"Đạt được thành công"));
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
        [SwaggerOperation(Tags = new[] { "Admin University - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin-university/accounts/switch-status")]
        public async Task<IActionResult> SetActiveForUniversityAdmin(int userId)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                await _accountService.SetActiveForUniversityAdmin(userId, universityId);
                return Ok(MyResponse<object>.OkWithMessage("Xét duyệt thành công!"));
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
        /// Update status to active high school manager
        /// </summary>
        /// <response code="200">
        /// Update status to active high school manager successfully
        /// </response>
        /// <response code="400">
        /// Update status to active high school manager fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/accounts/switch-status")]
        public async Task<IActionResult> SetActiveForHighSchoolAdmin(int userId)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                await _accountService.SetActiveForHighSchoolAdmin(userId, highSchoolId);
                return Ok(MyResponse<object>.OkWithMessage("Xét duyệt thành công!"));
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