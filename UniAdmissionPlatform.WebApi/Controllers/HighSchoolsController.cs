using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class HighSchoolsController : ControllerBase
    {
        private readonly IHighSchoolService _highSchoolService;
        private readonly IEventCheckService _eventCheckService;

        public HighSchoolsController(IHighSchoolService highSchoolService, IEventCheckService eventCheckService)
        {
            _highSchoolService = highSchoolService;
            _eventCheckService = eventCheckService;
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
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/high-schools/{highSchoolId:int}/event-slot")]
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
        [SwaggerOperation(Tags = new[] { "Accounts" })]
        [Route("~/api/v{version:apiVersion}/accounts/high-school/profile/{highSchoolId:int}")]
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
    }
}