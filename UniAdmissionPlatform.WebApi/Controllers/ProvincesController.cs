﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceService _eventTypeService;
        
        public ProvincesController(IProvinceService eventTypeService)
        {
            _eventTypeService = eventTypeService;
            
        }
        
        /// <summary>
        /// Get list province
        /// </summary>
        /// <response code="200">
        ///     <table id="doc">
        ///         <tr>
        ///             <th>Code</th>
        ///             <th>Description</th>
        ///         </tr>
        ///         <tr>
        ///             <td>0 (action success)</td>
        ///             <td>Success</td>
        ///         </tr>
        ///         <tr>
        ///             <td>7 (action fail)</td>
        ///             <td>Fail</td>
        ///         </tr>
        ///     </table>
        /// </response>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetListProvince([FromQuery] ProvinceBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var eventTypes = await _eventTypeService.GetAllProvinces(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<ProvinceBaseViewModel>>.OkWithDetail(eventTypes, $"Đạt được thành công"));
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