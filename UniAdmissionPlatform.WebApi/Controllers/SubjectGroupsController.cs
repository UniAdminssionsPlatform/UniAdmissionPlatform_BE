using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroup;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectGroupsController : ControllerBase
    {
        private readonly ISubjectGroupService _subjectGroupService;
        private readonly IAuthService _authService;

        public SubjectGroupsController(ISubjectGroupService subjectGroupService, IAuthService authService)
        {
            _subjectGroupService = subjectGroupService;
            _authService = authService;
        }

        /// <summary>
        /// Get subject group
        /// </summary>
        /// <response code="200">
        /// Get subject group successfully
        /// </response>
        /// <response code="400">
        /// Get subject group fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Subject Groups" })]
        [Route("~/api/v{version:apiVersion}/subject-groups/{id:int}")]
        public async Task<IActionResult> GetSubjectGroupById(int id)
        {
            try
            {
                var subjectGroup = await _subjectGroupService.GetById(id);
                return Ok(MyResponse<SubjectGroupBaseViewModel>.OkWithDetail(subjectGroup, "Đạt được thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Get list subject groups
        /// </summary>
        /// <response code="200">
        /// Get list subject groups successfully
        /// </response>
        /// <response code="400">
        /// Get list subject groups fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Subject Groups" })]
        [Route("~/api/v{version:apiVersion}/subject-groups")]
        public async Task<IActionResult> GetAllSubjectGroups([FromQuery] SubjectGroupBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var subjectGroups = await _subjectGroupService.GetAllSubjectGroups(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<SubjectGroupBaseViewModel>>.OkWithData(subjectGroups));
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
        /// Create subject group
        /// </summary>
        /// <response code="200">
        /// Create subject group successfully
        /// </response>
        /// <response code="400">
        /// Create subject group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Subject Groups" })]
        [Route("~/api/v{version:apiVersion}/admin/subject-groups")]
        public async Task<IActionResult> CreateSubjectGroup(
            [FromBody] CreateSubjectGroupRequest createSubjectGroupRequest)
        {
            try
            {
                var subjectGroupId = await _subjectGroupService.CreateSubjectGroup(createSubjectGroupRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { subjectGroupId }, "Tạo khối thi thành công!"));
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
        /// Update subject group
        /// </summary>
        /// <response code="200">
        /// Update subject group successfully
        /// </response>
        /// <response code="400">
        /// Update subject group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "Admin - Subject Groups" })]
        [Route("~/api/v{version:apiVersion}/admin/subject-groups/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateSubjectGroup(int id,
            [FromBody] UpdateSubjectGroupRequest updateSubjectGroupRequest)
        {
            try
            {
                await _subjectGroupService.UpdateSubjectGroup(id, updateSubjectGroupRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập khối thi thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập khối thi thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Update subject group
        /// </summary>
        /// <response code="200">
        /// Update subject group successfully
        /// </response>
        /// <response code="400">
        /// Update subject group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "Admin - Subject Groups" })]
        [Route("~/api/v{version:apiVersion}/admin/subject-groups/{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSubjectGroup(int id)
        {
            try
            {
                await _subjectGroupService.DeleteSubjectGroup(id);
                return Ok(MyResponse<object>.OkWithMessage("Xóa khối thi thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa khối thi thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Get scores by subject group id
        /// </summary>
        /// <response code="200">
        /// Get scores by subject group id successfully
        /// </response>
        /// <response code="400">
        /// Get scores by subject group id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Subject Groups" })]
        [Route("~/api/v{version:apiVersion}/student/subject-groups/{id:int}/get-score")]
        public IActionResult GetScoreOfStudent(int id, int schoolYearId)
        {
            var userId = _authService.GetUserId(HttpContext);
            try
            {
                var schoolRecord = _subjectGroupService.GetScoreOfStudent(id, schoolYearId, userId);
                return Ok(schoolRecord);
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

    }
}