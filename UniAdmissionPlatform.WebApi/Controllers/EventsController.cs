using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IAuthService _authService;
        
        public EventsController(IEventService eventService, IAuthService authService)
        {
            _eventService = eventService;
            _authService = authService;
        }
        /// <summary>
        /// Create a event
        /// </summary>
        /// <response code="200">
        ///     <table id="doc">
        ///         <tr>
        ///             <th>Code</th>
        ///             <th>Description</th>
        ///         </tr>
        ///         <tr>
        ///             <td>0 (action success)</td>
        ///             <td>Success</td>
        ///         </tr>
        ///         <tr>
        ///             <td>7 (action fail)</td>
        ///             <td>Fail</td>
        ///         </tr>
        ///     </table>
        /// </response>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest createEventRequest)
        {
            try
            {
                var universityId = _authService.GetUniversityId(HttpContext);
                var eventId = await _eventService.CreateEvent(universityId, createEventRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{eventId}, $"Tạo thành công event có id = {eventId}"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server ís error");
                }
            }
        }

        [HttpPut("book-slot-for-uni-admin")]
        public async Task<IActionResult> BookSlotForUniAdmin(BookSlotForUniAdminRequest bookSlotForUniAdminRequest)
        {
            var universityId = _authService.GetUniversityId(HttpContext);

            try
            {
                await _eventService.BookSlotForUniAdmin(universityId, bookSlotForUniAdminRequest);
                return Ok(MyResponse<object>.OkWithMessage("Book thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case (int) HttpStatusCode.BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Book thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

    }
}