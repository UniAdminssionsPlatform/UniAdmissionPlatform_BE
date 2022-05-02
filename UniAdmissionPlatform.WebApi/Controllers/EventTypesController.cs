using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    
    public class EventTypesController : ControllerBase
    {
        private readonly IEventTypeService _eventTypeService;
        
        public EventTypesController(IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
            
        }
        
        
        /// <summary>
        /// Get list event types
        /// </summary>
        /// <response code="200">
        /// Get list event types successfully
        /// </response>
        /// <response code="400">
        /// Get list event types fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "EventTypes" })]
        [Route("~/api/v{version:apiVersion}/event-types")]
        public async Task<IActionResult> GetListEventType([FromQuery] EventTypeBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var eventTypes = await _eventTypeService.GetAllEventTypes(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<EventTypeBaseViewModel>>.OkWithDetail(eventTypes, $"Đạt được thành công"));
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
        
    }
}