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
    public partial interface IMajorGroupRepository :IBaseRepository<MajorGroup>
    {
    }
    public partial class MajorGroupRepository :BaseRepository<MajorGroup>, IMajorGroupRepository
    {
         public MajorGroupRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

