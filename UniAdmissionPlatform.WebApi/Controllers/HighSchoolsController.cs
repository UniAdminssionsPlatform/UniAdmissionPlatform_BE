﻿using System;
using System.Threading.Tasks;
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
    public class HighSchoolsController : ControllerBase
    {
        private readonly IHighSchoolService _highSchoolService;
        private readonly IEventCheckService _eventCheckService;

        public HighSchoolsController(IHighSchoolService highSchoolService, IEventCheckService eventCheckService)
        {
            _highSchoolService = highSchoolService;
            _eventCheckService = eventCheckService;
        }
        
        
        
        /// <summary>
        /// Get a specific high school name by code
        /// </summary>
        /// <response code="200">
        /// Get a specific high school name by code successfully
        /// </response>
        /// <response code="400">
        /// Get a specific high school name by code fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/[controller]/get-by-code")]
        public async Task<IActionResult> GetHighSchoolByCode(string highSchoolCode)
        {
            try
            {
                var highSchools = await _highSchoolService.GetHighSchoolByCode(highSchoolCode);
                return Ok(MyResponse<HighSchoolCodeViewModel>.OkWithDetail(highSchools, $"Đạt được thành công"));
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
        
        /// <summary>
        /// Get list high schools
        /// </summary>
        /// <response code="200">
        /// Get list high schools successfully
        /// </response>
        /// <response code="400">
        /// Get list high schools fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllHighSchools([FromQuery] GetHighSchoolBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var highSchool = await _highSchoolService.GetAllHighSchools(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<GetHighSchoolBaseViewModel>>.OkWithDetail(highSchool, $"Đạt được thành công"));
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

        /// <summary>
        /// Get event-slots of high school
        /// </summary>
        /// <response code="200">
        /// Get event-slots of high school successfully
        /// </response>
        /// <response code="400">
        /// Get event-slots of high school fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "High Schools" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{highSchoolId:int}/event-slot")]
        public async Task<IActionResult> GetEventsInfoByHighSchoolId(int highSchoolId, string sort, int page, int limit)
        {
            try
            {
                var eventSlots = await _eventCheckService.GetEventsByHighSchoolId(highSchoolId, sort, page, limit);
                return Ok(MyResponse<PageResult<EventWithSlotViewModel>>.OkWithDetail(eventSlots, $"Đạt được thành công"));
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