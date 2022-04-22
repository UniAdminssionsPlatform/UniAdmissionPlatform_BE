using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
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

        [HttpPut("appvove-event-to-slot")]
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
                    (int)HttpStatusCode.NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Duyệt sự kiện không thành công. " + e.Error.Message),
                    (int)HttpStatusCode.BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Duyệt sự kiện không thành công. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
    }
}