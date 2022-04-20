using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("get-students-info")]
        public async Task<IActionResult> GetStudentInfo([FromQuery] AccountBaseViewModel filter, int page, int limit, string sort)
        {
            return Ok(await _accountService.GetAll(filter, page, limit, sort));
        }
    }
}