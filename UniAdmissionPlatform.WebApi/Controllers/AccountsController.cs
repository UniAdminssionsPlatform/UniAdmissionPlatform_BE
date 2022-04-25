using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Toolkit;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.WebApi.Attributes;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public AccountsController(IAccountService accountService, IFirebaseStorageService firebaseStorageService, IAuthService authService)
        {
            _accountService = accountService;
            _firebaseStorageService = firebaseStorageService;
            _authService = authService;
        }
        
        /// <summary>
        /// Get a list of student accounts
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
        [HttpGet("students")]
        public async Task<IActionResult> GetStudentInfo([FromQuery] AccountBaseViewModel filter, int page, int limit, string sort)
        {
            return Ok(await _accountService.GetAll(filter, page, limit, sort));
        }
        
        /// <summary>
        /// Get a list of university accounts
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
        [HttpGet("universities")]
        public async Task<IActionResult> GetUniversityAccount([FromQuery] AccountBaseViewModel filter, int page, int limit, string sort)
        {
            return Ok(await _accountService.GetAllUniAccount(filter, page, limit, sort));
        }
        
        /// <summary>
        /// Update a user account 
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
        [HttpPut("user/{id:int}")]
        public async Task<IActionResult> UpdateUniAccount(int id, [FromBody] UpdateUniAccountRequest updateUniAccountRequest)
        {
            try
            {
                await _accountService.UpdateUniAccount(id, updateUniAccountRequest);
                return Ok(MyResponse<object>.OkWithDetail(new{id}, $"Cập nhập tài khoản thành công với ID = {id}"));
                // return Ok(MyResponse<object>.OkWithMessage("Cập nhập thành công!"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Update fail, because server ís error");
                }
            }
        }
        
        /// <summary>
        /// Update all account in Admin 
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
        [HttpPut("admin/{id:int}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountRequestForAdmin updateAccountRequestForAdmin)
        {
            try
            {
                await _accountService.UpdateAccount(id, updateAccountRequestForAdmin);
                return Ok(MyResponse<object>.OkWithDetail(new{id}, $"Cập nhập tài khoản thành công với ID = {id}"));
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    case StatusCodes.Status404NotFound:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    case StatusCodes.Status400BadRequest:
                        // Business rule
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Cập nhập thất bại. " + e.Error.Message);
                    default:
                        throw new GlobalException(ExceptionCode.PrintMessageErrorOut,
                            "Update fail, because server ís error");
                }
            }
        }
        
        /// <summary>
        /// Get list accounts
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
        public async Task<IActionResult> GetAllAccounts([FromQuery] AccountBaseViewModel filter, string sort,
            int page, int limit)
        {
            try
            {
                var events = await _accountService.GetAllAccounts(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<AccountBaseViewModel>>.OkWithDetail(events, $"Đạt được thành công"));
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
        /// Upload a new avatar
        /// </summary>
        [HttpPost("update-avatar")]
        [CustomAuthorize]
        public async Task<IActionResult> UpdateAvatar([Required] IFormFile file)
        {
            try
            {
                FileToolKit.ValidateFileName(file, new[] { ".jpg", ".png", ".jpeg" }, FileMaxSize);
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status400BadRequest => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Đổi ảnh đại diện thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message)
                };
            }

            string fileUrl;
            try
            {
                var lastIndexOf = file.FileName.LastIndexOf(".", StringComparison.Ordinal);
                fileUrl = await _firebaseStorageService.UploadImage(file.FileName[lastIndexOf..file.FileName.Length].ToLower(),"avatar",file.OpenReadStream());
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    _ => new GlobalException(ExceptionCode.PrintErrorObjectOut, e.Error.Message)
                };
            }


            var userId = _authService.GetUserId(HttpContext);
            try
            {
                await _accountService.UploadAvatar(userId, fileUrl);

                return Ok(MyResponse<object>.OkWithMessage("Đổi ảnh đại diện thành công."));
            }
            catch (ErrorResponse e)
            {
                throw e.Error.Code switch
                {
                    StatusCodes.Status404NotFound => new GlobalException(ExceptionCode.PrintMessageErrorOut,
                        "Đổi ảnh đại diện thất bại. " + e.Error.Message),
                    _ => new GlobalException(ExceptionCode.PrintMessageErrorOut, e.Error.Message)
                };
            }
        }
        
        private const long FileMaxSize = 10000000;
    }
}