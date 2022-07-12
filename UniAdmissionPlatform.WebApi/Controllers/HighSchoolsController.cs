using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.HighSchool;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class HighSchoolsController : ControllerBase
    {
        private readonly IHighSchoolService _highSchoolService;
        private readonly IEventCheckService _eventCheckService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public HighSchoolsController(IHighSchoolService highSchoolService, IEventCheckService eventCheckService, IAuthService authService, IUserService userService)
        {
            _highSchoolService = highSchoolService;
            _eventCheckService = eventCheckService;
            _authService = authService;
            _userService = userService;
        }
        
        
        
        /// <summary>
        /// Get a specific high school name by code
        /// </summary>
        /// <response code="200">
        /// Get a specific high school name by code successfully
        /// </response>
        /// <response code="400">
        /// Get a specific high school name by code fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/high-schools/get-by-code")]
        public async Task<IActionResult> GetHighSchoolByCode(string highSchoolCode)
        {
            try
            {
                var highSchools = await _highSchoolService.GetHighSchoolByCode(highSchoolCode);
                return Ok(MyResponse<HighSchoolCodeViewModel>.OkWithDetail(highSchools, $"Đạt được thành công"));
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
        /// Get a specific high school name by manager code
        /// </summary>
        /// <response code="200">
        /// Get a specific high school name by manager code successfully
        /// </response>
        /// <response code="400">
        /// Get a specific high school name by manager code fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/high-schools/get-by-manager-code")]
        public async Task<IActionResult> GetHighSchoolByManagerCode(string highSchoolManagerCode)
        {
            try
            {
                var highSchools = await _highSchoolService.GetHighSchoolByManagerCode(highSchoolManagerCode);
                return Ok(MyResponse<HighSchoolManagerCodeViewModel>.OkWithDetail(highSchools, $"Đạt được thành công"));
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
        /// Get list high schools
        /// </summary>
        /// <response code="200">
        /// Get list high schools successfully
        /// </response>
        /// <response code="400">
        /// Get list high schools fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/high-schools")]
        public async Task<IActionResult> GetAllHighSchools([FromQuery] GetHighSchoolBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var highSchool = await _highSchoolService.GetAllHighSchools(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<GetHighSchoolBaseViewModel>>.OkWithDetail(highSchool, $"Đạt được thành công"));
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
        /// Get event-slots of high school
        /// </summary>
        /// <response code="200">
        /// Get event-slots of high school successfully
        /// </response>
        /// <response code="400">
        /// Get event-slots of high school fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin University - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-university/high-school-slots/{highSchoolId:int}/events")]
        public async Task<IActionResult> GetEventsInfoByHighSchoolId(int highSchoolId, string sort, int page, int limit)
        {
            try
            {
                var eventSlots = await _eventCheckService.GetEventsByHighSchoolId(highSchoolId, sort, page, limit);
                return Ok(MyResponse<PageResult<EventWithSlotViewModel>>.OkWithDetail(eventSlots, $"Đạt được thành công"));
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
        /// Get a high school profile by id
        /// </summary>
        /// <response code="200">
        /// Get a high school profile successfully
        /// </response>
        /// <response code="400">
        /// Get a high school profile fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/high-school/profile/{highSchoolId:int}")]
        public async Task<IActionResult> GetHighSchoolProfile(int highSchoolId)
        {
            try
            {
                var highSchoolProfileById = await _highSchoolService.GetHighSchoolProfileById(highSchoolId);
                return Ok(MyResponse<GetHighSchoolBaseViewModel>.OkWithDetail(highSchoolProfileById, $"Đạt được thành công"));
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
        /// Update a high school profile
        /// </summary>
        /// <response code="200">
        /// Update a high school profile successfully
        /// </response>
        /// <response code="400">
        /// Update a high school profile fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/profile")]
        public async Task<IActionResult> UpdateHighSchoolProfile([FromBody] UpdateHighSchoolProfileRequest updateHighSchoolProfileRequest)
        {
            var id = _authService.GetHighSchoolId(HttpContext);
            try
            {
                await _highSchoolService.UpdateHighSchoolProfile(id, updateHighSchoolProfileRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{id}, $"Cập nhập tài khoản thành công với ID = {id}"));
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
        /// Create a new high school profile
        /// </summary>
        /// <response code="200">
        /// Create a new high school profile successfully
        /// </response>
        /// <response code="400">
        /// Create a new high school profile fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Accounts" })]
        [Route("~/api/v{version:apiVersion}/admin/accounts/high-school")]
        public async Task<IActionResult> CreateHighSchoolProfile([FromBody] CreateHighSchoolRequest createHighSchoolRequest)
        {
            try
            {
                var highSchoolId = await _highSchoolService.CreateHighSchoolProfile(createHighSchoolRequest);
                return Ok(MyResponse<int>.OkWithDetail(highSchoolId, $"Tạo thành công trường cấp 3 có id = {highSchoolId}"));
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
    }
}