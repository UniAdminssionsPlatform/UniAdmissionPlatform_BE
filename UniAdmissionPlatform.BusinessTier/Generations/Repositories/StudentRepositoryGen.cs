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
    public partial interface IStudentRepository :IBaseRepository<Student>
    {
    }
    public partial class StudentRepository :BaseRepository<Student>, IStudentRepository
    {
         public StudentRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

