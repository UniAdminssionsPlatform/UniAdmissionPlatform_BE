using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.GoalAdmission;
using UniAdmissionPlatform.BusinessTier.Requests.GoalAdmissionType;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class GoalAdmissionsController : ControllerBase
    {
        private readonly IGoalAdmissionService _goalAdmissionService;
        private readonly IGoalAdmissionTypeService _goalAdmissionTypeService;
        
        public GoalAdmissionsController(IGoalAdmissionService goalAdmissionService, IGoalAdmissionTypeService goalAdmissionTypeService)
        {
            _goalAdmissionService = goalAdmissionService;
            _goalAdmissionTypeService = goalAdmissionTypeService;
        }
        
        /// <summary>
        /// Create a new goal admission
        /// </summary>
        /// <response code="200">
        /// Create a new goal admission successfully
        /// </response>
        /// <response code="400">
        /// Create a new goal admission fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin University - Goal Admissions" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]")]
        [CasbinAuthorize]
        public async Task<IActionResult> CreateGoalAdmission([FromBody] CreateGoalAdmissionRequest createGoalAdmissionRequest)
        {
            try
            {
                var goalAdmissionId = await _goalAdmissionService.CreateGoalAdmission(createGoalAdmissionRequest);
                return Ok(MyResponse<int>.OkWithDetail(goalAdmissionId, $"Tạo thành công goalAdmission có id = {goalAdmissionId}"));
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
        
        /// <summary>
        /// Update a goal admission
        /// </summary>
        /// <response code="200">
        /// Update a goal admission successfully
        /// </response>
        /// <response code="400">
        /// Update a goal admission fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Goal Admissions" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{goalAdmissionId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> UpdateGoalAdmission(int goalAdmissionId, [FromBody] UpdateGoalAdmissionRequest updateGoalAdmissionRequest)
        {
            try
            {
                await _goalAdmissionService.UpdateGoalAdmission(goalAdmissionId, updateGoalAdmissionRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
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
        
        /// <summary>
        /// Get list goal admissions
        /// </summary>
        /// <response code="200">
        /// Get list goal admissions successfully
        /// </response>
        /// <response code="400">
        /// Get list goal admissions fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Goal Admissions" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetListGoalAdmission([FromQuery] GoalAdmissionBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var goalAdmissions = await _goalAdmissionService.GetAllGoalAdmissions(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<GoalAdmissionBaseViewModel>>.OkWithDetail(goalAdmissions, $"Đạt được thành công"));
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
        
        /// <summary>
        /// Delete a goal admission
        /// </summary>
        /// <response code="200">
        /// Delete a goal admission successfully
        /// </response>
        /// <response code="400">
        /// Delete a goal admission fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin University - Goal Admissions" })]
        [Route("~/api/v{version:apiVersion}/admin-university/[controller]/{goalAdmissionId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> DeleteAGoalAdmission(int goalAdmissionId)
        {
            try
            {
                await _goalAdmissionService.DeleteGoalAdmission(goalAdmissionId);
                return Ok(MyResponse<object>.OkWithMessage("Xóa thành công!"));
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
        
        /// <summary>
        /// Get goal admission by id
        /// </summary>
        /// <response code="200">
        /// Get goal admission by id successfully
        /// </response>
        /// <response code="400">
        /// Get goal admission by id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Goal Admissions" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{goalAdmissionId:int}")]
        public async Task<IActionResult> GetGoalAdmissionById(int goalAdmissionId)
        {
            try
            {
                var goalAdmissions = await _goalAdmissionService.GetGoalAdmissionById(goalAdmissionId);
                return Ok(MyResponse<GoalAdmissionBaseViewModel>.OkWithDetail(goalAdmissions, "Tìm kiếm thành công!"));
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
        
        /// <summary>
        /// Create a new goal admission type
        /// </summary>
        /// <response code="200">
        /// Create a new goal admission type successfully
        /// </response>
        /// <response code="400">
        /// Create a new goal admission type fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin University - Goal Admission Types" })]
        [Route("~/api/v{version:apiVersion}/admin-university/goal-admission-type")]
        [CasbinAuthorize]
        public async Task<IActionResult> CreateGoalAdmissionType([FromBody] CreateGoalAdmissionTypeRequest createGoalAdmissionTypeRequest)
        {
            try
            {
                var goalAdmissionTypeId = await _goalAdmissionTypeService.CreateGoalAdmissionType(createGoalAdmissionTypeRequest);
                return Ok(MyResponse<int>.OkWithDetail(goalAdmissionTypeId, $"Tạo thành công goalAdmissionType có id = {goalAdmissionTypeId}"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tạo thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Update a goal admission type
        /// </summary>
        /// <response code="200">
        /// Update a goal admission type successfully
        /// </response>
        /// <response code="400">
        /// Update a goal admission type fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin University - Goal Admission Types" })]
        [Route("~/api/v{version:apiVersion}/admin-university/goal-admission-type/{goalAdmissionTypeId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> UpdateGoalAdmissionType(int goalAdmissionTypeId, [FromBody] UpdateGoalAdmissionTypeRequest updateGoalAdmissionTypeRequest)
        {
            try
            {
                await _goalAdmissionTypeService.UpdateGoalAdmissionType(goalAdmissionTypeId, updateGoalAdmissionTypeRequest);
                return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Cập nhập thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get list goal admission types
        /// </summary>
        /// <response code="200">
        /// Get list goal admission types successfully
        /// </response>
        /// <response code="400">
        /// Get list goal admission types fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Goal Admission Types" })]
        [Route("~/api/v{version:apiVersion}/goal-admission-type")]
        public async Task<IActionResult> GetListGoalAdmissionType([FromQuery] GoalAdmissionTypeBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var goalAdmissions = await _goalAdmissionTypeService.GetAllGoalAdmissionTypeTypes(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<GoalAdmissionTypeBaseViewModel>>.OkWithDetail(goalAdmissions, $"Đạt được thành công"));
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
        
        /// <summary>
        /// Delete a goal admission type
        /// </summary>
        /// <response code="200">
        /// Delete a goal admission type successfully
        /// </response>
        /// <response code="400">
        /// Delete a goal admission type fail
        /// </response>
        /// /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin University - Goal Admission Types" })]
        [Route("~/api/v{version:apiVersion}/admin-university/goal-admission-type/{goalAdmissionTypeId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> DeleteAGoalAdmissionType(int goalAdmissionTypeId)
        {
            try
            {
                await _goalAdmissionTypeService.DeleteGoalAdmissionType(goalAdmissionTypeId);
                return Ok(MyResponse<object>.OkWithMessage("Xóa thành công!"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Xóa thất bại. " + e.Error.Message),
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Xóa thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message),
                };
            }
        }
        
        /// <summary>
        /// Get goal admission type by id
        /// </summary>
        /// <response code="200">
        /// Get goal admission type by id successfully
        /// </response>
        /// <response code="400">
        /// Get goal admission type by id fail
        /// </response>
        /// <response code="401">
        /// No Login
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Goal Admission Types" })]
        [Route("~/api/v{version:apiVersion}/goal-admission-type/{goalAdmissionTypeId:int}")]
        public async Task<IActionResult> GetGoalAdmissionTypeById(int goalAdmissionTypeId)
        {
            try
            {
                var goalAdmissions = await _goalAdmissionTypeService.GetGoalAdmissionTypeById(goalAdmissionTypeId);
                return Ok(MyResponse<GoalAdmissionTypeBaseViewModel>.OkWithDetail(goalAdmissions, "Tìm kiếm thành công!"));
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