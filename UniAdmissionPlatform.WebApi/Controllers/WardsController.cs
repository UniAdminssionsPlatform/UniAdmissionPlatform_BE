using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class WardsController : ControllerBase
    {
        private readonly IWardService _wardService;

        public WardsController(IWardService wardService)
        {
            _wardService = wardService;
        }

        /// <summary>
        /// Get list wards
        /// </summary>
        /// <response code="200">
        /// Get list wards successfully
        /// </response>
        /// <response code="400">
        /// Get list wards fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Ward" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllWards([FromQuery] WardViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allWards = await _wardService.GetAllWards(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<WardViewModel>>.OkWithData(allWards));
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