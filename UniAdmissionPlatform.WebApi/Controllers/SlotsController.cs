using System;
using System.Collections.Generic;
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
        /// Create new slots
        /// </summary>
        /// <response code="200">
        /// Create new slots successfully
        /// </response>
        /// <response code="400">
        /// Create new slots fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin High School - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/")]
        public async Task<IActionResult> CreateSlot([FromBody] List<CreateSlotRequest> createSlotRequest)
        {
            var highSchoolId = _authService.GetHighSchoolId(HttpContext);
            try
            {
                await _slotService.CreateSlots(highSchoolId, createSlotRequest);
                return Ok(MyResponse<object>.OkWithMessage("Tạo slots thành công!"));
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
        
        /// <summary>
        /// Update a slot status to full
        /// </summary>
        /// <response code="200">
        /// Update a slot status to full successfully
        /// </response>
        /// <response code="400">
        /// Update a slot status to full fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/slot-full")]
        public async Task<IActionResult> UpdateSlotStatus(int slotId)
        {
            try
            {
                await _slotService.UpdateFullSlotStatus(slotId);
                return Ok(MyResponse<object>.OkWithMessage("Trạng thái buổi chuyển thành đã đầy!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Update a slot
        /// </summary>
        /// <response code="200">
        /// Update a slot successfully
        /// </response>
        /// <response code="400">
        /// Update a slot fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin High School - Slots" })]
        [Route("~/api/v{version:apiVersion}/admin-high-school/[controller]/{slotId:int}")]
        public async Task<IActionResult> UpdateTag(int slotId, [FromBody] UpdateSlotRequest updateSlotRequest)
        {
            try
            {
                await _slotService.UpdateSlot(slotId, updateSlotRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

        /// <summary>
        /// Get slot with events by id
        /// </summary>
        /// <response code="200">
        /// Get slot with events by id successfully
        /// </response>
        /// <response code="400">
        /// Get slot with events by id fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Slots" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{id:int}/events")]
        public async Task<IActionResult> GetListEventBySlotId(int id)
        {
            try
            {
                var eventsBySlotId =  await _slotService.GetEventsBySlotId(id);
                return Ok(MyResponse<SlotWithEventsViewModel>.OkWithDetail(eventsBySlotId, "Đạt được thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Đạt được thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }

        }
    }
}