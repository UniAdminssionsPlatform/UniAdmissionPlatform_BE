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
    public partial interface IMajorDepartmentRepository :IBaseRepository<MajorDepartment>
    {
    }
    public partial class MajorDepartmentRepository :BaseRepository<MajorDepartment>, IMajorDepartmentRepository
    {
         public MajorDepartmentRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

