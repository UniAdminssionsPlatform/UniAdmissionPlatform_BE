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
    public partial interface IStudentRecordItemRepository :IBaseRepository<StudentRecordItem>
    {
    }
    public partial class StudentRecordItemRepository :BaseRepository<StudentRecordItem>, IStudentRecordItemRepository
    {
         public StudentRecordItemRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

