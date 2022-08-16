using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Requests.Casbin;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Attributes;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class CasbinController : ControllerBase
    {
        private readonly ICasbinService _casbinService;
        
        public CasbinController(ICasbinService casbinService)
        {
            _casbinService = casbinService;
        }

        /// <summary>
        /// Get list subjects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - Casbin" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/subjects")]
        [CasbinAuthorize]
        public IActionResult GetAllSubjects()
        {
            return Ok(MyResponse<List<string>>.OkWithData(_casbinService.GetAllSubjects()));
        }
        
        /// <summary>
        /// Get list actions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - Casbin" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/actions")]
        [CasbinAuthorize]
        public IActionResult GetAllActions()
        {
            return Ok(MyResponse<List<string>>.OkWithData(_casbinService.GetAllActions()));
        }
        
        /// <summary>
        /// Get list objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - Casbin" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/objects")]
        [CasbinAuthorize]
        public IActionResult GetAllObjects()
        {
            return Ok(MyResponse<List<string>>.OkWithData(_casbinService.GetAllObjects()));
        }
        
        /// <summary>
        /// Get current policy
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Admin - Casbin" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/policies")]
        [CasbinAuthorize]
        public IActionResult GetPolicy()
        {
            return Ok(MyResponse<List<List<string>>>.OkWithData(_casbinService.GetPolicy()));
        }

        /// <summary>
        /// Create a policy
        /// </summary>
        /// <param name="addPolicyRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Casbin" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/policies")]
        [CasbinAuthorize]
        public async Task<IActionResult> AddPolicy(AddPolicyRequest addPolicyRequest)
        {
            try
            {
                await _casbinService.AddPolicy(addPolicyRequest);
                return Ok(MyResponse<object>.OkWithMessage("Thêm quyền hành thành công"));
            }
            catch (Exception e)
            {
                return Ok(MyResponse<object>.FailWithMessage(e.Message));
            }
        }
        
        /// <summary>
        /// Delete a policy
        /// </summary>
        /// <param name="removePolicyRequest"></param>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin - Casbin" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/policies")]
        [CasbinAuthorize]
        public async Task<IActionResult> AddPolicy(RemovePolicyRequest removePolicyRequest)
        {
            try
            {
                await _casbinService.RemovePolicy(removePolicyRequest);
                return Ok(MyResponse<object>.OkWithMessage("Xóa quyền hành thành công"));
            }
            catch (Exception e)
            {
                return Ok(MyResponse<object>.FailWithMessage(e.Message));
            }
        }
    }
}