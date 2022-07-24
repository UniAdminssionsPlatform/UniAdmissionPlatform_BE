using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Comment;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.Firestore;
using UniAdmissionPlatform.Firestore.Models;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IAuthService _authService;
        private readonly IAccountRepository _accountRepository;

        public CommentsController(ICommentService commentService, IAuthService authService, IAccountRepository accountRepository)
        {
            _commentService = commentService;
            _authService = authService;
            _accountRepository = accountRepository;
        }

        [HttpGet("event")]
        public async Task<IActionResult> GetEventComment(int eventId, int page = 1, int limit = 10)
        {
            var eventComments = await _commentService.GetEventComment(eventId, page, limit);
            return Ok(MyResponse<PageResult<Comment>>.OkWithDetail(eventComments, "Đạt được thành công."));
        }

        [HttpPost("event")]
        public async Task<IActionResult> CommentEvent([FromBody] CommentEventRequest commentEventRequest)
        {
            var userId = _authService.GetUserId(HttpContext);
            var account = _accountRepository.FirstOrDefault(a => a.Id == userId);
            await _commentService.CreateEventComment(new Comment
            {
                Content = commentEventRequest.Content,
                CreatedDate = Timestamp.GetCurrentTimestamp(),
                CreatedDateString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                UpdatedDate = Timestamp.GetCurrentTimestamp(),
                UpdatedDateString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ReferenceId = commentEventRequest.EventId,
                UserId = userId,
                UserName = account.FirstName + ' ' + account.MiddleName + ' ' + account.LastName
            });

            return Ok();
        }
        
        [HttpPost("university")]
        public async Task<IActionResult> CommentUniversity([FromBody] CommentUniversityRequest commentUniversityRequest)
        {
            var userId = _authService.GetUserId(HttpContext);
            var account = _accountRepository.FirstOrDefault(a => a.Id == userId);
            await _commentService.CreateUniversityComment(new Comment
            {
                Content = commentUniversityRequest.Content,
                CreatedDate = Timestamp.GetCurrentTimestamp(),
                CreatedDateString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                UpdatedDate = Timestamp.GetCurrentTimestamp(),
                UpdatedDateString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ReferenceId = commentUniversityRequest.UniversityId,
                UserId = userId,
                UserName = account.FirstName + ' ' + account.MiddleName + ' ' + account.LastName
            });

            return Ok();
        }
        
        [HttpGet("university")]
        public async Task<IActionResult> GetUniversityComments(int universityId, int page = 1, int limit = 10)
        {
            var universityComments = await _commentService.GetUniversityComment(universityId, page, limit);
            return Ok(MyResponse<PageResult<Comment>>.OkWithDetail(universityComments, "Đạt được thành công."));
        }
    }
}