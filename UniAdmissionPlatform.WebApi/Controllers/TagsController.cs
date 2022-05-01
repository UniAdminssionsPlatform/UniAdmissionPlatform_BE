using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Tag;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        
        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }
        
        /// <summary>
        /// Create a new tag
        /// </summary>
        /// <response code="200">
        /// Create a new tag successfully
        /// </response>
        /// <response code="400">
        /// Create a new tag fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Tags" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest createTagRequest)
        {
            try
            {
                var tagId = await _tagService.CreateTag(createTagRequest);
                return Ok(MyResponse<int>.OkWithDetail(tagId, $"Tạo thành công tag có id = {tagId}"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server is error");
                }
            }
        }
        
        /// <summary>
        /// Create a new tag
        /// </summary>
        /// <response code="200">
        /// Create a new tag successfully
        /// </response>
        /// <response code="400">
        /// Create a new tag fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Tags" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{tagId:int}")]
        public async Task<IActionResult> UpdateTag(int tagId, [FromBody] UpdateTagRequest updateTagRequest)
        {
            try
            {
                await _tagService.UpdateTag(tagId, updateTagRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Update fail, because server is error");
                }
            }
        }
        
        /// <summary>
        /// Get list tags
        /// </summary>
        /// <response code="200">
        /// Get list tags successfully
        /// </response>
        /// <response code="400">
        /// Get list tags fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Tags" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetListTag([FromQuery] TagBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var tags = await _tagService.GetAllTags(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<TagBaseViewModel>>.OkWithDetail(tags, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server is error");
                }
            }
        }
        
        /// <summary>
        /// Delete a tag
        /// </summary>
        /// <response code="200">
        /// Delete a tag successfully
        /// </response>
        /// <response code="400">
        /// Delete a tag fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Tags" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{tagId:int}")]
        public async Task<IActionResult> DeleteATag(int tagId)
        {
            try
            {
                await _tagService.DeleteTag(tagId);
                return Ok(MyResponse<object>.OkWithMessage("Xóa thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Xóa thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Delete fail, because server is error");
                }
            }
        }
        
        /// <summary>
        /// Get tag by id
        /// </summary>
        /// <response code="200">
        /// Get tag by id successfully
        /// </response>
        /// <response code="400">
        /// Get tag by id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Tags" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{tagId:int}")]
        public async Task<IActionResult> GetTagById(int tagId)
        {
            try
            {
                var tags = await _tagService.GetTagById(tagId);
                return Ok(MyResponse<TagBaseViewModel>.OkWithDetail(tags, "Tìm kiếm thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tìm kiếm thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

    }
}