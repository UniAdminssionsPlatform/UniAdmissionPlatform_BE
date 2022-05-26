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
using UniAdmissionPlatform.BusinessTier.Requests.SubjectGroupMajor;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectGroupMajorsController : ControllerBase
    {
        private readonly ISubjectGroupMajorService _subjectGroupMajorService;

        public SubjectGroupMajorsController(ISubjectGroupMajorService subjectGroupMajorService)
        {
            _subjectGroupMajorService = subjectGroupMajorService;
        }

        /// <summary>
        /// Get subject group majors
        /// </summary>
        /// <response code="200">
        /// Get subject group majors successfully
        /// </response>
        /// <response code="400">
        /// Get subject group majors fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Subject Group Majors" })]
        [Route("~/api/v{version:apiVersion}/subject-group-majors")]
        public async Task<IActionResult> GetSubjectGroupMajors([FromQuery] SubjectGroupMajorBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var subjectGroupMajors = await _subjectGroupMajorService.GetSubjectGroupMajors(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<SubjectGroupMajorBaseViewModel>>.OkWithDetail(subjectGroupMajors, "Đạt được thành công."));
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
        [SwaggerOperation(Tags = new[] { "Admin - Subject Group Majors" })]
        [Route("~/api/v{version:apiVersion}/admin/subject-group-majors")]
        public async Task<IActionResult> CreateSubjectGroupMajor(CreateSubjectGroupMajorRequest createSubjectGroupMajorRequest)
        {
            try
            {
                await _subjectGroupMajorService.CreateSubjectGroupMajor(createSubjectGroupMajorRequest);
                return Ok(MyResponse<object>.OkWithMessage("Tạo thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Tạo thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Delete subject group
        /// </summary>
        /// <response code="200">
        /// Delete subject group successfully
        /// </response>
        /// <response code="400">
        /// Delete subject group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin - Subject Group Majors" })]
        [Route("~/api/v{version:apiVersion}/admin/subject-group-majors")]
        public async Task<IActionResult> DeleteSubjectGroupMajor([FromQuery] int subjectGroupId, [FromQuery] int majorId)
        {
            try
            {
                await _subjectGroupMajorService.DeleteSubjectGroupMajor(majorId, subjectGroupId);
                return Ok(MyResponse<object>.OkWithMessage("Xóa thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Tạo thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
    }
}