/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
namespace UniAdmissionPlatform.BusinessTier.Generations.Repositories
{
    public partial interface IStudentReportRepository :IBaseRepository<StudentReport>
    {
    }
    public partial class StudentReportRepository :BaseRepository<StudentReport>, IStudentReportRepository
    {
         public StudentReportRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

