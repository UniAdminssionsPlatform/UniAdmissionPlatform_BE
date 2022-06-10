using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    // [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EventChecksController : ControllerBase
    {
        private readonly IEventCheckService _eventCheckService;
        private readonly IAuthService _authService;

        public EventChecksController(IEventCheckService eventCheckService, IAuthService authService)
        {
            _eventCheckService = eventCheckService;
            _authService = authService;
        }

        
        /// /// <summary>
        /// Change event check status to approve
        /// </summary>
        /// <response code="200">
        /// Approve a event with id successfully
        /// </response>
        /// <response code="400">
        /// Approve fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - EventCheck" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/event-checks/{eventCheckId:int}/approve")]
        public async Task<IActionResult> ApproveEventToSlot(int eventCheckId)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);

            try
            {
                await _eventCheckService.ApproveEventToSlot(highSchoolId, eventCheckId);
                return Ok(MyResponse<object>.OkWithMessage("Duyệt thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Duyệt sự kiện không thành công. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Duyệt sự kiện không thành công. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// /// <summary>
        /// Change event check status to reject
        /// </summary>
        /// <response code="200">
        /// Reject a event with id successfully
        /// </response>
        /// <response code="400">
        /// Reject fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - EventCheck" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/event-checks/{eventCheckId:int}/reject")]
        public async Task<IActionResult> RejectEventToSlot(int eventCheckId)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);

            try
            {
                await _eventCheckService.RejectEventToSlot(highSchoolId, eventCheckId);
                return Ok(MyResponse<object>.OkWithMessage("Từ chối thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Từ chối sự kiện không thành công. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Từ chối sự kiện không thành công. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get a event by slot id
        /// </summary>
        /// <response code="200">
        /// Get a event by slot id successfully
        /// </response>
        /// <response code="400">
        /// Get a event by slot id fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin High School - Events" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/events/{slotId:int}")]
        public async Task<IActionResult> GetEventBySlotId(int slotId)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                var eventBySlotId = await _eventCheckService.GetEventBySlotId(slotId, highSchoolId);
                return Ok(MyResponse<EventBySlotBaseViewModel>.OkWithDetail(eventBySlotId, "Truy cập thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get event history list by high school id
        /// </summary>
        /// <response code="200">
        /// Get event history list by high school id successfully
        /// </response>
        /// <response code="400">
        /// Get event history list by high school id fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin High School - Events" })]
        [Route("~/api/v{version:apiVersion}/events/{highSchoolId:int}/history")]
        public async Task<IActionResult> GetEventsHistoryByHighSchoolId(int highSchoolId)
        {
            try
            {
                var eventBySlotId = await _eventCheckService.GetEventsHistoryByHighSchoolId(highSchoolId, null, 1, 10);
                return Ok(MyResponse<PageResult<EventWithSlotViewModel>>.OkWithDetail(eventBySlotId, "Truy cập thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

        /// /// <summary>
        /// Get list event check
        /// </summary>
        /// <response code="200">
        /// Get list event check successfully
        /// </response>
        /// <response code="400">
        /// Get list event check fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin University - Event checks" })]
        [Route("~/api/v{version:apiVersion}/admin-university/event-checks")]
        public async Task<IActionResult> GetAllEventCheckForUniAdmin([FromQuery] EventCheckWithEventAndSlotModel filter, string sort,int page, int limit)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            var eventCheckForUniAdmin = await _eventCheckService.GetEventCheckForUniAdmin(universityId, filter, sort, page, limit);
            return Ok(MyResponse<PageResult<EventCheckWithEventAndSlotModel>>.OkWithDetail(eventCheckForUniAdmin, "Đạt được thành công"));
        }
    }
}