using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Slot;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
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

        [HttpGet("get-slots-for-school-admin")]
        public async Task<IActionResult> GetSlotsForUniAdmin([FromQuery] SlotFilterForSchoolAdmin slotFilterForSchoolAdmin, int page, int limit)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                var slots = await _slotService.GetSlotForSchoolUni(highSchoolId, slotFilterForSchoolAdmin, page, limit);
                return Ok(MyResponse<PageResult<SlotViewModel>>.OkWithDetail(slots, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
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