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
        /// Create a new event successfully
        /// </response>
        /// <response code="400">
        /// Create a new event fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin University - Events" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]")]
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
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo sự kiện thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo sự kiện thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Update a new event
        /// </summary>
        /// <response code="200">
        /// Update a event successfully
        /// </response>
        /// <response code="400">
        /// Update a event fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Events" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{eventId:int}")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromBody] UpdateEventRequest updateEventRequest)
        {
            try
            {
                await _eventService.UpdateEvent(eventId, updateEventRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{eventId}, $"Cập nhập event thành công với ID = {eventId}"));
                // return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập sự kiện thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập sự kiện thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get list events
        /// </summary>
        /// <response code="200">
        /// Get list events successfully
        /// </response>
        /// <response code="400">
        /// Get list events fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Events" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        
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
        /// Get a event by id
        /// </summary>
        /// <response code="200">
        /// Get a event by id successfully
        /// </response>
        /// <response code="400">
        /// Get a event by id fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Events" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{eventId:int}")]
        public async Task<IActionResult> GetEventByID(int eventId)
        {
            try
            {
                var eventByID = await _eventService.GetEventByID(eventId);
                return Ok(MyResponse<EventBaseViewModel>.OkWithDetail(eventByID, "Truy cập thành công!"));
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
        /// Delete a event by id
        /// </summary>
        /// <response code="200">
        /// Delete a event by id successfully
        /// </response>
        /// <response code="400">
        /// Delete a event by id fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin University - Events" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{eventId:int}")]
        public async Task<IActionResult> DeleteAEvent(int eventId)
        {
            try
            {
                await _eventService.DeleteEvent(eventId);
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
        /// Book a slot in admin university
        /// </summary>
        /// <response code="200">
        /// Book a slot successfully
        /// </response>
        /// <response code="400">
        /// Book a slot fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Events" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/book")]
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