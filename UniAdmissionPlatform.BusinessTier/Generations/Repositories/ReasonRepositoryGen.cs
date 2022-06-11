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
    public partial interface IReasonRepository :IBaseRepository<Reason>
    {
    }
    public partial class ReasonRepository :BaseRepository<Reason>, IReasonRepository
    {
         public ReasonRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

