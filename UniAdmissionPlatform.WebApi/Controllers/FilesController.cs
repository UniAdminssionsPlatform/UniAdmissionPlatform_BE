using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Toolkit;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFirebaseStorageService _firebaseStorageService;

        public FilesController(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        /// <summary>
        /// Upload a new file image(.png, .jpg)
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
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([Required] IFormFile file)
        {
            try
            {
                FileToolKit.ValidateFileName(file, new[] {".jpg", ".png"}, FileMaxSize);
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    (int) HttpStatusCode.BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Tải lên thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message)
                };
            }

            try
            {
                var lastIndexOf = file.FileName.LastIndexOf(".", StringComparison.Ordinal);
                var fileUrl = await _firebaseStorageService.UploadImage(
                    file.FileName[lastIndexOf..file.FileName.Length].ToLower(), "image", file.OpenReadStream());
                return Ok(MyResponse<object>.OkWithDetail(new
                {
                    fileUrl,
                }, "Tải lên thành công"));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    _ => new GlobalException(ExceptionCode.PrintErrorObjectOut, e.Error.Message)
                };
            }
        }

        private const long FileMaxSize = 10000000;

    }
}