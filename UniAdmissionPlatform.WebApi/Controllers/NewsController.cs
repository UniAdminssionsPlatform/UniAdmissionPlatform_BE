using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.News;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IAuthService _authService;

        public NewsController(INewsService newsService, IAuthService authService)
        {
            _newsService = newsService;
            _authService = authService;
        }

        /// <summary>
        /// Create a news
        /// </summary>
        /// <response code="200">
        /// Create a news successfully
        /// </response>
        /// <response code="400">
        /// Create a news fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin University - News" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]")]
        [CasbinAuthorize]
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsRequest createNewsRequest)
        {
            try
            {
                var universityId = _authService.GetUniversityId(HttpContext);
                var newsId = await _newsService.CreateNews(universityId, createNewsRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { newsId }, $"Tạo thành công tin tức có id = {newsId}"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo tin tức thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo tin tức thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }

        /// <summary>
        /// Update a news
        /// </summary>
        /// <response code="200">
        /// Update a news successfully
        /// </response>
        /// <response code="400">
        /// Update a news fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - News" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{newsId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> UpdateNews(int newsId, [FromBody] UpdateNewsRequest updateNewsRequest)
        {
            try
            {
                await _newsService.UpdateNews(newsId, updateNewsRequest);
                return Ok(MyResponse<object>.OkWithDetail(new { newsId }, $"Cập nhập tin tức thành công với id = {newsId}"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập tin tức thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập tin tức thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Delete a news by id
        /// </summary>
        /// <response code="200">
        /// Delete a news successfully
        /// </response>
        /// <response code="400">
        /// Delete a news fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin University - News" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{newsId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> DeleteNewsById(int newsId)
        {
            try
            {
                await _newsService.DeleteNewsById(newsId);
                return Ok(MyResponse<object>.OkWithData("Xóa thành công tin tức."));
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
        /// Get list news
        /// </summary>
        /// <response code="200">
        /// Get list news successfully
        /// </response>
        /// <response code="400">
        /// Get list news fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "News" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllNews([FromQuery] NewsWithUniversityViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allNews = await _newsService.GetAllNews(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<NewsWithUniversityViewModel>>.OkWithDetail(allNews, $"Đạt được thành công"));
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
        /// Get list news for university
        /// </summary>
        /// <response code="200">
        /// Get list news for university successfully
        /// </response>
        /// <response code="400">
        /// Get list news for university fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin University - News" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]")]
        [CasbinAuthorize]
        public async Task<IActionResult> GetAllNewsForAdminUniversity([FromQuery] NewsWithPublishViewModel filter, string sort, int page, int limit)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                var allNews = await _newsService.GetAllNewsForUniversityAdmin(filter, sort, page, limit, universityId);
                return Ok(MyResponse<PageResult<NewsWithPublishViewModel>>.OkWithDetail(allNews, $"Đạt được thành công"));
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
        
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - News" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{newsId:int}/set-publish")]
        [CasbinAuthorize]
        public async Task<IActionResult> GetAllNewsForAdminUniversity(int newsId, [FromBody] SetPublishRequest setPublishRequest)
        {
            var universityId = _authService.GetUniversityId(HttpContext);
            try
            {
                await _newsService.SetIsPublish(universityId, newsId, setPublishRequest.IsPublish);
                return Ok(MyResponse<PageResult<NewsWithPublishViewModel>>.OkWithMessage("Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, "Thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Get a news by id
        /// </summary>
        /// <response code="200">
        /// Get a news successfully
        /// </response>
        /// <response code="400">
        /// Get a news fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "News" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{newsId:int}")]
        public async Task<IActionResult> GetNewsById(int newsId)
        {
            try
            {
                var newsById = await _newsService.GetNewsById(newsId);
                return Ok(MyResponse<NewsWithUniversityViewModel>.OkWithDetail(newsById, $"Đạt được thành công"));
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
        
    }
}