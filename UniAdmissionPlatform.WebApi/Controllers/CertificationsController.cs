using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Certification;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [ApiController]
    public class CertificationsController : ControllerBase
    {
        private readonly ICertificationService _certificationService;

        public CertificationsController(ICertificationService certificationService)
        {
            _certificationService = certificationService;
        }

        /// <summary>
        /// Get list certifications
        /// </summary>
        /// <response code="200">
        /// Get list certifications successfully
        /// </response>
        /// <response code="400">
        /// Get list certifications fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllCertification([FromQuery] CertificationBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var allCertification = await _certificationService.GetAllCertification(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<CertificationBaseViewModel>>.OkWithDetail(allCertification, $"Đạt được thành công"));
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
        /// Get a certification by id
        /// </summary>
        /// <response code="200">
        /// Get a certification successfully
        /// </response>
        /// <response code="400">
        /// Get a certification fail
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Certifications" })]
        [Route("~/api/v{version:apiVersion}/[controller]/{certificationId:int}")]
        public async Task<IActionResult> GetCertificationById(int certificationId)
        {
            try
            {
                var certificationById = await _certificationService.GetCertificationById(certificationId);
                return Ok(MyResponse<CertificationBaseViewModel>.OkWithDetail(certificationById, $"Đạt được thành công"));
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

        /// <summary>
        /// Create a new certification
        /// </summary>
        /// <response code="200">
        /// Create a new certification successfully
        /// </response>
        /// <response code="400">
        /// Create a new certification fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin - Certifications" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]")]
        [CasbinAuthorize]
        public async Task<IActionResult> CreateCertification([FromBody] CreateCertificationRequest createCertificationRequest)
        {
            try
            {
                var certificationId = await _certificationService.CreateCertification(createCertificationRequest);
                return Ok(MyResponse<object>.OkWithDetail(new {certificationId}, $"Tạo chứng chỉ thành công với id = {certificationId}"));
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
        /// Update a certification
        /// </summary>
        /// <response code="200">
        /// Update a certification successfully
        /// </response>
        /// <response code="400">
        /// Update a certification fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpPut]
        [SwaggerOperation(Tags = new[] { "Admin - Certifications" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{certificationId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> UpdateCertification(int certificationId, [FromBody] UpdateCertificationRequest updateCertificationRequest)
        {
            try
            {
                await _certificationService.UpdateCertification(certificationId, updateCertificationRequest);
                return Ok(MyResponse<object>.OkWithMessage($"Cập nhập thành công chứng chỉ id = {certificationId}."));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status400BadRequest:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message);
                }
            }
        }

        /// <summary>
        /// Delete a certification by id
        /// </summary>
        /// <response code="200">
        /// Delete a certification successfully
        /// </response>
        /// <response code="400">
        /// Delete a certification fail
        /// </response>
        /// <response code="401">
        /// No login
        /// </response>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Admin - Certifications" })]
        [Route("~/api/v{version:apiVersion}/admin/[controller]/{certificationId:int}")]
        [CasbinAuthorize]
        public async Task<IActionResult> DeleteCertificationById(int certificationId)
        {
            try
            {
                await _certificationService.DeleteCertificationById(certificationId);
                return Ok(MyResponse<object>.OkWithData($"Xóa thành công chứng chỉ id ={certificationId}."));
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
    }
}