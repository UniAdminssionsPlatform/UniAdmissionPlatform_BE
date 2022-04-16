/////////////////////////////////////////////////////////////////
//
//              AUTO-GENERATED
//
/////////////////////////////////////////////////////////////////

using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    
    public partial interface IStudentRecordItemService:IBaseService<StudentRecordItem>
    {
    }
    public partial class StudentRecordItemService:BaseService<StudentRecordItem>,IStudentRecordItemService
    {
        public StudentRecordItemService(IUnitOfWork unitOfWork,IStudentRecordItemRepository repository):base(unitOfWork,repository){}
    }
}
