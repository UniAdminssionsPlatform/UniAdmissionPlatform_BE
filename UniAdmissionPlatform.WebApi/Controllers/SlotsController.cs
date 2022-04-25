using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Slot;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
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
        
        /// <summary>
        /// Get list slots
        /// </summary>
        /// <response code="200">
        /// Get list slots successfully
        /// </response>
        /// <response code="400">
        /// Get list slots fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin High School - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]")]
        public async Task<IActionResult> GetSlotsForHighSchoolAdmin([FromQuery] SlotFilterForSchoolAdmin slotFilterForSchoolAdmin, int page, int limit)
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
        
        /// <summary>
        /// Create a new slot
        /// </summary>
        /// <response code="200">
        /// Create a new slot successfully
        /// </response>
        /// <response code="400">
        /// Create a new slot fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin High School - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/")]
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
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Close a slot
        /// </summary>
        /// <response code="200">
        /// Close a slot successfully
        /// </response>
        /// <response code="400">
        /// Close a slot fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/close-slot")]
        public async Task<IActionResult> CloseSlot(int slotId)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                await _slotService.CloseSlot(highSchoolId, slotId);
                return Ok(MyResponse<object>.OkWithMessage("Đóng buổi thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Đóng thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Đóng thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get list slots
        /// </summary>
        /// <response code="200">
        /// Get list slots successfully
        /// </response>
        /// <response code="400">
        /// Get list slots fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin University - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]")]
        public async Task<IActionResult> GetSlotsForUniAdmin([FromQuery] SlotFilterForUniAdmin slotFilterForUniAdmin, int page, int limit)
        {
            try
            {
                var slots = await _slotService.GetSlotForAdminUni(slotFilterForUniAdmin, page, limit);
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
    }
}