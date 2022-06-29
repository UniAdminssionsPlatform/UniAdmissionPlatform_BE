using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Requests.User;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Responses.User;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IHighSchoolService _highSchoolService;
        private readonly IUniversityService _universityService;

        public UsersController(IUserService userService, IAuthService authService, IHighSchoolService highSchoolService, IUniversityService universityService)
        {
            _userService = userService;
            _authService = authService;
            _highSchoolService = highSchoolService;
            _universityService = universityService;
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
            var decodedTokenClaim = (JArray) ((JObject) ((JObject) decodedToken.Claims["firebase"])?["identities"])?["email"];
            var email = (string) decodedTokenClaim?[0];
            var uid = decodedToken.Uid;

            try
            {
                var loginResponse = await _userService.Login(uid, email);
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
        public async Task<IActionResult> RegisterForHighSchoolManager([FromBody] RegisterForSchoolManagerRequest registerForSchoolManagerRequest)
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
        
        /// <summary>
        /// Register for university manager
        /// </summary>
        /// <response code="200">
        /// Register for university manager successfully
        /// </response>
        /// <response code="400">
        /// Register for university manager fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "User" })]
        [Route("~/api/v{version:apiVersion}/[controller]/register-university-manager")]
        public async Task<IActionResult> RegisterForUniversityManager([FromBody] RegisterForUniversityManagerRequest registerForUniversityManagerRequest)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                var university = await _universityService.GetUniversityNameByCode(registerForUniversityManagerRequest.UniversityCode);
                var id = await _userService.CreateUniversityManagerAccount(userId, university.Id, registerForUniversityManagerRequest);
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
        
        
        /// <summary>
        /// Get list users
        /// </summary>
        /// <response code="200">
        /// Get list users successfully
        /// </response>
        /// <response code="400">
        /// Get list users fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - User" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]")]
        public async Task<IActionResult> GetUsers([FromQuery] UserBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var users = await _userService.GetUsers(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<UserBaseViewModel>>.OkWithDetail(users, "Đạt được thành công."));
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
        /// Switch status of student
        /// </summary>
        /// <response code="200">
        /// Switch status of student successfully
        /// </response>
        /// <response code="400">
        /// Switch status of student fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - User" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/{studentId:int}")]
        public async Task<IActionResult> SwitchStatusStudentAccount(int studentId)
        {
            try
            {
                var highSchoolId = _authService.GetHighSchoolId(HttpContext);
                await _userService.SwitchStatusStudentAccount(studentId, highSchoolId);
                return Ok(MyResponse<object>.OkWithMessage("Chuyển đổi thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Chuyển đổi thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
        
        /// <summary>
        /// Switch status of user for admin
        /// </summary>
        /// <response code="200">
        /// Switch status of user for admin successfully
        /// </response>
        /// <response code="400">
        /// Switch status of user for admin fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin/accounts/{userId:int}")]
        public async Task<IActionResult> SwitchStatusAccountForAdmin(int userId)
        {
            try
            {
                await _userService.SwitchStatusAccountForAdmin(userId);
                return Ok(MyResponse<object>.OkWithMessage("Chuyển đổi trạng thái thành công."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Chuyển đổi trạng thái thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Chuyển đổi trạng thái thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
    }
}