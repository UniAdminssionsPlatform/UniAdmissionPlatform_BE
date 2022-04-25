using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Event;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    // [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IAuthService _authService;
        private readonly ISlotService _slotService;
        
        public EventsController(IEventService eventService, IAuthService authService, ISlotService slotService)
        {
            _eventService = eventService;
            _authService = authService;
            _slotService = slotService;
        }
        /// <summary>
        /// Create a new event
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
        [SwaggerOperation(Tags = new[] { "University - Event" })]
        [Route("~/api/v{version:apiVersion}/university/[controller]")]
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
        
        /// <summary>
        /// Update event by id
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
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "University - Event" })]
        [Route("~/api/v{version:apiVersion}/university/[controller]/{id:int}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventRequest updateEventRequest)
        {
            try
            {
                await _eventService.UpdateEvent(id, updateEventRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{id}, $"Cập nhập event thành công với ID = {id}"));
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
                            "Update fail, because server ís error");
                }
            }
        }
        
        /// <summary>
        /// Get a list events
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
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "University - Event" })]
        [Route("~/api/v{version:apiVersion}/university/[controller]")]
        public async Task<IActionResult> GetListEvent([FromQuery] EventBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var events = await _eventService.GetAllEvents(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<EventBaseViewModel>>.OkWithDetail(events, $"Đạt được thành công"));
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
        
        /// <summary>
        /// Get a specific event by id
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
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "University - Event" })]
        [Route("~/api/v{version:apiVersion}/university/[controller]/{id:int}")]
        public async Task<IActionResult> GetEventByID(int id)
        {
            try
            {
                var eventByID = await _eventService.GetEventByID(id);
                return Ok(MyResponse<EventBaseViewModel>.OkWithDetail(eventByID, "Truy cập thành công!"));
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
        
        /// <summary>
        /// Delete a event by id
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
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "University - Event" })]
        [Route("~/api/v{version:apiVersion}/university/[controller]/{id:int}")]
        public async Task<IActionResult> DeleteAEvent(int id)
        {
            try
            {
                await _eventService.DeleteEvent(id);
                return Ok(MyResponse<object>.OkWithMessage("Xóa thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Delete fail, because server ís error");
                }
            }
        }
        
        /// <summary>
        /// Booking slot for university manager
        /// </summary>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "University - Event" })]
        [Route("~/api/v{version:apiVersion}/university/[controller]/booking")]
        public async Task<IActionResult> BookSlotForUniAdmin(BookSlotForUniAdminRequest bookSlotForUniAdminRequest)
        {

            
            var universityId = _authService.GetUniversityId(HttpContext);

            try
            {
                var isValid = await _slotService.CheckStatusOfSlot(bookSlotForUniAdminRequest.SlotId, SlotStatus.Open);
                if (!isValid)
                {
                    throw new ErrorResponse(StatusCodes.Status400BadRequest, "Slot này không được mở để book.");
                }
                await _eventService.BookSlotForUniAdmin(universityId, bookSlotForUniAdminRequest);
                return Ok(MyResponse<object>.OkWithMessage("Book thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
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