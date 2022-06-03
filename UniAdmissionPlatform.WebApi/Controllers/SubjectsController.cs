using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Subject;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Get list subjects
        /// </summary>
        /// <response code="200">
        /// Get list subjects successfully
        /// </response>
        /// <response code="400">
        /// Get list subjects fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Subjects" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllSubject([FromQuery] SubjectBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allSubject = await _subjectService.GetAllSubject(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<SubjectBaseViewModel>>.OkWithDetail(allSubject, $"Đạt được thành công"));
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
        /// Get subject by id
        /// </summary>
        /// <response code="200">
        /// Get subject successfully
        /// </response>
        /// <response code="400">
        /// Get subject fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Subjects" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{subjectId:int}")]
        public async Task<IActionResult> GetSubjectById(int subjectId)
        {
            try
            {
                var subjectById = await _subjectService.GetSubjectById(subjectId);
                return Ok(MyResponse<SubjectBaseViewModel>.OkWithDetail(subjectById, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Create a new subject
        /// </summary>
        /// <response code="200">
        /// Create a new subject successfully
        /// </response>
        /// <response code="400">
        /// Create a new subject fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Subjects" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest createSubjectRequest)
        {
            try
            {
                var subjectId = await _subjectService.CreateSubject(createSubjectRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {subjectId}, $"Tạo môn học thành công với id = {subjectId}"));
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
        /// Update a subject
        /// </summary>
        /// <response code="200">
        /// Update a subject successfully
        /// </response>
        /// <response code="400">
        /// Update a subject fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin - Subjects" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{subjectId:int}")]
        public async Task<IActionResult> UpdateSubject(int subjectId, [FromBody] UpdateSubjectRequest updateSubjectRequest)
        {
            try
            {
                await _subjectService.UpdateSubject(subjectId, updateSubjectRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công  môn học."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Delete a subject by id
        /// </summary>
        /// <response code="200">
        /// Delete a subject successfully
        /// </response>
        /// <response code="400">
        /// Delete a subject fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin - Subjects" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{subjectId:int}")]
        public async Task<IActionResult> DeleteSubjectById(int subjectId)
        {
            try
            {
                await _subjectService.DeleteSubjectById(subjectId);
                return Ok(MyResponse<object>.OkWithData("Xóa thành công môn học."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }
        
        /// <summary>
        /// Get list base subjects
        /// </summary>
        /// <response code="200">
        /// Get list base subjects successfully
        /// </response>
        /// <response code="400">
        /// Get list base subjects fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Subjects" })]
        [Route("~/api/v{version:apiVersion}/[controller]/base-subjects")]
        public async Task<IActionResult> GetBaseSubject()
        {
            try
            {
                var baseSubjects = await _subjectService.GetBaseSubjects();
                return Ok(MyResponse<List<SubjectBaseViewModel>>.OkWithDetail(baseSubjects, "Đạt được thành công"));
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
    }
}