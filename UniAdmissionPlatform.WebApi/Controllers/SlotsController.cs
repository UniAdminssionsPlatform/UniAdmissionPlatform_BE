using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Slot;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISlotService _slotService;

        public SlotsController(IAuthService authService, ISlotService slotService)
        {
            _authService = authService;
            _slotService = slotService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest createSlotRequest)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                var slotId = await _slotService.CreateSlot(highSchoolId, createSlotRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { slotId }, "Tạo slot thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    (int)HttpStatusCode.BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
    }
}