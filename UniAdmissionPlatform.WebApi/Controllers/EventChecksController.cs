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
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/{eventCheckId:int}")]
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
            try
            {
                var eventBySlotId = await _eventCheckService.GetEventBySlotId(slotId);
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
    }
}