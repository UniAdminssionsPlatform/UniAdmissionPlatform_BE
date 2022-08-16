using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Participation;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationsController : ControllerBase
    {
        private readonly IParticipationService _participationService;
        private readonly IAuthService _authService;

        public ParticipationsController(IParticipationService participationService, IAuthService authService)
        {
            _participationService = participationService;
            _authService = authService;
        }

        /// <summary>
        /// Get list participations
        /// </summary>
        /// <response code="200">
        /// Get list participations successfully
        /// </response>
        /// <response code="400">
        /// Get list participations fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Participations" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllParticipations([FromQuery] ParticipationBaseViewModel filter,
            string sort, int page, int limit)
        {
            try
            {
                var participations = await _participationService.GetParticipations(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<ParticipationBaseViewModel>>.OkWithDetail(participations,
                    "Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Get a participation by id
        /// </summary>
        /// <response code="200">
        /// Get a participation by id successfully
        /// </response>
        /// <response code="400">
        /// Get a participation by id fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Participations" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> GetParticipationById(int id)
        {
            try
            {
                var participation = await _participationService.GetById(id);
                return Ok(MyResponse<ParticipationBaseViewModel>.OkWithDetail(participation, "Đạt được thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Create a participation by id
        /// </summary>
        /// <response code="200">
        /// Create a participation by id successfully
        /// </response>
        /// <response code="400">
        /// Create a participation by id fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Student - Participations" })]
        [Route("~/api/v{version:apiVersion}/student/[controller]")]
        [CasbinAuthorize]
        public async Task<IActionResult> CreateParticipationForStudent([FromBody] CreateParticipationRequestForStudent createParticipationRequestForStudent)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                var participationId = await _participationService.CreateParticipation(userId, createParticipationRequestForStudent);
                return Ok(MyResponse<object>.OkWithDetail(new { participationId }, $"Tạo sự tham gia thành công với id = {participationId}."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Update a participation by id
        /// </summary>
        /// <response code="200">
        /// Update a participation by id successfully
        /// </response>
        /// <response code="400">
        /// Update a participation by id fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Student - Participations" })]
        [Route("~/api/v{version:apiVersion}/student/[controller]/{id:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> UpdateParticipationForStudent(int id,
            UpdateParticipationRequestForStudent updateParticipationRequestForStudent)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                await _participationService.UpdateParticipation(id, updateParticipationRequestForStudent, userId);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập sự tham gia thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest :
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                           "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Delete a participation by id
        /// </summary>
        /// <response code="200">
        /// Delete a participation by id successfully
        /// </response>
        /// <response code="400">
        /// Delete a participation by id fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Student - Participations" })]
        [Route("~/api/v{version:apiVersion}/student/[controller]/{id:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> DeleteParticipationForStudent(int id)
        {
            var userId = _authService.GetUserId(HttpContext);

            try
            {
                await _participationService.DeleteParticipation(id, userId);
                return Ok(MyResponse<object>.OkWithMessage("Xóa sự tham gia thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest :
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
    }
}