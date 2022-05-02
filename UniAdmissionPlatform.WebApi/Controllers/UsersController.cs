using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Requests.User;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Responses.User;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly IHighSchoolService _highSchoolService;

        public UsersController(IUserService userService, IAuthService authService, IAccountService accountService, IHighSchoolService highSchoolService)
        {
            _userService = userService;
            _authService = authService;
            _accountService = accountService;
            _highSchoolService = highSchoolService;
        }
        
        /// <summary>
        /// Login by firebase token
        /// </summary>
        /// <response code="200">
        /// Login by firebase token successfully
        /// </response>
        /// <response code="400">
        /// Login by firebase token fail
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "User" })]
        [Route("~/api/v{version:apiVersion}/[controller]/login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(loginRequest.FirebaseToken);
            var uid = decodedToken.Uid;

            try
            {
                var loginResponse = await _userService.Login(uid);
                return Ok(MyResponse<LoginResponse>.OkWithDetail(loginResponse, "Đăng nhập thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Đăng nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }
        
        /// /// <summary>
        /// Register an account
        /// </summary>
        /// <response code="200">
        /// Register an account successfully
        /// </response>
        /// <response code="400">
        /// Register an account fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "User" })]
        [Route("~/api/v{version:apiVersion}/[controller]/register")]
        [CustomAuthorize]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                var loginResponse = await _userService.Register(userId, registerRequest);
                return Ok(MyResponse<LoginResponse>.OkWithDetail(loginResponse, "Đăng ký thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Đăng ký thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
        
        /// <summary>
        /// Create a new manager account
        /// </summary>
        /// <response code="200">
        /// Create a new manager account successfully
        /// </response>
        /// <response code="400">
        /// Create a new manager account fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest createAccountRequest)
        {
            try
            {
                var accountId = await _userService.CreateAccount(createAccountRequest);
                return Ok(MyResponse<int>.OkWithDetail(accountId, $"Tạo thành công Account có id = {accountId}"));
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
        /// Register for student
        /// </summary>
        /// <response code="200">
        /// Register for student successfully
        /// </response>
        /// <response code="400">
        /// Register for student fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "User" })]
        [Route("~/api/v{version:apiVersion}/[controller]/register-student")]
        public async Task<IActionResult> RegisterForStudent([FromBody] RegisterForStudentRequest registerForStudentRequest)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                var highSchool = await _highSchoolService.GetHighSchoolByCode(registerForStudentRequest.HighSchoolCode);
                var studentAccount = await _userService.CreateStudentAccount(userId, highSchool.Id, registerForStudentRequest);
                return Ok(MyResponse<object>.OkWithDetail(studentAccount,"Đăng ký thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Đăng ký thất bại. " + e.Error.Message);
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Đăng ký thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
        
        /// <summary>
        /// Register for school manager
        /// </summary>
        /// <response code="200">
        /// Register for school manager successfully
        /// </response>
        /// <response code="400">
        /// Register for school manager fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "User" })]
        [Route("~/api/v{version:apiVersion}/[controller]/register-school-manager")]
        public async Task<IActionResult> RegisterForStudent([FromBody] RegisterForSchoolManagerRequest registerForSchoolManagerRequest)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                var highSchool = await _highSchoolService.GetHighSchoolByManagerCode(registerForSchoolManagerRequest.HighSchoolManagerCode);
                var id = await _userService.CreateHighSchoolManagerAccount(userId, highSchool.Id, registerForSchoolManagerRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {id},"Đăng ký thành công. Vui lòng chờ xác thực."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Đăng ký thất bại. " + e.Error.Message);
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Đăng ký thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
    }
}