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
    public partial interface ICasbinRuleRepository :IBaseRepository<CasbinRule>
    {
    }
    public partial class CasbinRuleRepository :BaseRepository<CasbinRule>, ICasbinRuleRepository
    {
         public CasbinRuleRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

