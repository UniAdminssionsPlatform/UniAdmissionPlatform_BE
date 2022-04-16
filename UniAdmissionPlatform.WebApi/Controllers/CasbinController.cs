using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Requests.Casbin;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CasbinController : ControllerBase
    {
        private readonly ICasbinService _casbinService;

        public CasbinController(ICasbinService casbinService)
        {
            _casbinService = casbinService;
        }

        /// <summary>
        /// Lấy tất cả các subjects
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-subjects")]
        public IActionResult GetAllSubjects()
        {
            return Ok(MyResponse<List<string>>.OkWithData(_casbinService.GetAllSubjects()));
        }
        
        /// <summary>
        /// Lấy tất cả các actions
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-actions")]
        public IActionResult GetAllActions()
        {
            return Ok(MyResponse<List<string>>.OkWithData(_casbinService.GetAllActions()));
        }
        
        /// <summary>
        /// Lấy tất cả các objects
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-object")]
        public IActionResult GetAllObjects()
        {
            return Ok(MyResponse<List<string>>.OkWithData(_casbinService.GetAllObjects()));
        }
        
        /// <summary>
        /// Lấy quyền hành hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-policy")]
        public IActionResult GetPolicy()
        {
            return Ok(MyResponse<List<List<string>>>.OkWithData(_casbinService.GetPolicy()));
        }

        /// <summary>
        /// Thêm quyền hành
        /// </summary>
        /// <param name="addPolicyRequest"></param>
        /// <returns></returns>
        [HttpPost("add-policy")]
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
        /// Xóa quyền hành
        /// </summary>
        /// <param name="removePolicyRequest"></param>
        /// <returns></returns>
        [HttpPost("remove-policy")]
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