using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectGroupSubjectsController : ControllerBase
    {
        private readonly ISubjectGroupSubjectService _subjectGroupSubjectService;

        public SubjectGroupSubjectsController(ISubjectGroupSubjectService subjectGroupSubjectService)
        {
            _subjectGroupSubjectService = subjectGroupSubjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjectGroupSubjects(SubjectGroupSubjectBaseViewModel filter, string sort, int page, int limit)
        {
            try
            {
                var subjectGroupSubjects = await _subjectGroupSubjectService.GetSubjectGroupSubjects(filter, sort, page, limit);
                return Ok(MyResponse<PageResult<SubjectGroupSubjectBaseViewModel>>.OkWithDetail(subjectGroupSubjects, "Đạt được thành công."));
            }
            catch (ErrorResponse e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}