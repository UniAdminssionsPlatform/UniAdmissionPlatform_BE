using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Attributes;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowEventsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IFollowEventService _followEventService;

        public FollowEventsController(IAuthService authService, IFollowEventService followEventService)
        {
            _authService = authService;
            _followEventService = followEventService;
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Student - Follow Event" })]
        [Route("~/api/v{version:apiVersion}/student/is-follow")]
        [CasbinAuthorize]
        public IActionResult GetFollow(int eventId)
        {
            var userId = _authService.GetUserId(HttpContext);
            var isFollow = _followEventService.IsFollow(userId, eventId);
            return Ok(MyResponse<bool>.OkWithData(isFollow));
        }


        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Student - Follow Event" })]
        [Route("~/api/v{version:apiVersion}/student/follow")]
        [CasbinAuthorize]
        public async Task<IActionResult> Follow(int eventId, bool isFollow)
        {
            var userId = _authService.GetUserId(HttpContext);
            await _followEventService.Follow(userId, eventId, isFollow);
            return Ok(MyResponse<object>.OkWithMessage("Theo dõi thành công."));
        }
    }
}