using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;
using UniAdmissionPlatform.BusinessTier.Generations.Services;


namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleTypeService;
        
        public RolesController(IRoleService roleTypeService)
        {
            _roleTypeService = roleTypeService;
            
        }
        
        /// <summary>
        /// Get list roles
        /// </summary>
        /// <response code="200">
        /// Get list roles successfully
        /// </response>
        /// <response code="400">
        /// Get list roles fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - Roles" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]")]
        public async Task<IActionResult> GetListRole([FromQuery] RoleBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var eventTypes = await _roleTypeService.GetAllRoles(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<RoleBaseViewModel>>.OkWithDetail(eventTypes, $"Đạt được thành công"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cannot create, because server ís error");
                }
            }
        }
        
    }
}