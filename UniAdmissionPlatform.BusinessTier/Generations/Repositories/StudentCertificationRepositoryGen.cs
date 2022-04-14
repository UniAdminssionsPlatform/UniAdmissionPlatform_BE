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
    public partial interface IStudentCertificationRepository :IBaseRepository<StudentCertification>
    {
    }
    public partial class StudentCertificationRepository :BaseRepository<StudentCertification>, IStudentCertificationRepository
    {
         public StudentCertificationRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

