using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.MajorGroup;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorGroupsController : ControllerBase
    {
        private readonly IMajorGroupService _majorGroupService;

        public MajorGroupsController(IMajorGroupService majorGroupService)
        {
            _majorGroupService = majorGroupService;
        }

        /// <summary>
        /// Get list major group
        /// </summary>
        /// <response code="200">
        /// Get list major group successfully
        /// </response>
        /// <response code="400">
        /// Get list major group fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Major Groups" })]
        [Route("~/api/v{version:apiVersion}/major-groups")]
        public async Task<IActionResult> GetAllMajorGroup([FromQuery] MajorGroupBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allMajorGroup = await _majorGroupService.GetAllMajorGroup(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<MajorGroupBaseViewModel>>.OkWithDetail(allMajorGroup,
                    $"Đạt được thành công"));
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
        /// Get a major group by id
        /// </summary>
        /// <response code="200">
        ///  Get a major group successfully
        /// </response>
        /// <response code="400">
        ///  Get a major group fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Major Groups" })]
        [Route("~/api/v{version:apiVersion}/major-groups/{id:int}")]
        public async Task<IActionResult> GetMajorGroupById(int id)
        {
            try
            {
                var majorGroup = await _majorGroupService.GetMajorGroupById(id);
                return Ok(MyResponse<MajorGroupBaseViewModel>.OkWithDetail(majorGroup, "Đạt được thành công."));
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
        /// Create a major group
        /// </summary>
        /// <response code="200">
        /// Create a major group successfully
        /// </response>
        /// <response code="400">
        /// Create a major group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "Admin - Major Groups" })]
        [Route("~/api/v{version:apiVersion}/admin/major-groups")]
        [HttpPost]
        [CasbinAuthorize]
        public async Task<IActionResult> GetMajorGroupById([FromBody] CreateMajorGroupRequest createMajorGroupRequest)
        {
            try
            {
                var majorGroupId = await _majorGroupService.CreateMajorGroup(createMajorGroupRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { majorGroupId },
                    $"Tạo thành công nhóm ngành với id = {majorGroupId}"));
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
        /// Update a major group
        /// </summary>
        /// <response code="200">
        /// Update a major group successfully
        /// </response>
        /// <response code="400">
        /// Update a major group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "Admin - Major Groups" })]
        [Route("~/api/v{version:apiVersion}/admin/major-groups/{id:int}")]
        [HttpPut]
        [CasbinAuthorize]
        public async Task<IActionResult> UpdateMajorGroup(int id, UpdateMajorGroupRequest updateMajorGroupRequest)
        {
            try
            {
                await _majorGroupService.UpdateMajorGroup(id, updateMajorGroupRequest);
                return Ok(MyResponse<object>.FailWithMessage("Cập nhập nhóm ngành thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Delete a major group
        /// </summary>
        /// <response code="200">
        /// Delete a major group successfully
        /// </response>
        /// <response code="400">
        /// Delete a major group fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "Admin - Major Groups" })]
        [Route("~/api/v{version:apiVersion}/admin/major-groups/{id:int}")]
        [HttpDelete]
        [CasbinAuthorize]
        public async Task<IActionResult> DeleteMajorGroupById(int id)
        {
            try
            {
                await _majorGroupService.DeleteMajorGroup(id);
                return Ok(MyResponse<object>.FailWithMessage("Xóa nhóm ngành thành công."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            e.Error.Message);
                }
            }
        }
    }
}