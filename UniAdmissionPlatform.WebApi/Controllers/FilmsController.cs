using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Responses;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        /// <summary>
        /// Lấy tất cả các tên phim hoạt hình hay nhất hệ mặt trời
        /// </summary>
        /// <response code="200"><h3>code="0": success <br/>
        /// code="7": fail</h3></response>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFilms()
        {
            IList<Film> films = new List<Film>();
            films.Add(new Film("Phàm nhân tu tiên", "Hàn Lập"));
            films.Add(new Film("Đấu la đại lục", "Đường Tam"));

            return Ok(MyResponse<IList<Film>>.OkWithData(films));
        }
    }
}