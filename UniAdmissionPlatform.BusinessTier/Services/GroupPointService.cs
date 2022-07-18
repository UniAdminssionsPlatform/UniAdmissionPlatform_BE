using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.DataTier.BaseConnect;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IGroupPointService
    {
        public Task Sync();
    }

    public partial class GroupPointService
    {
        private readonly IUniversityProgramRepository _universityProgramRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GroupPointService(IUnitOfWork unitOfWork,IGroupPointRepository repository, IUniversityProgramRepository universityProgramRepository):base(unitOfWork,repository)
        {
            _universityProgramRepository = universityProgramRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Sync()
        {
            var groupPoints = await Get().ToListAsync();
            var year = DateTime.Now.Year - 1;

            var universityPrograms = await _universityProgramRepository.Get(up => up.SchoolYear.Year == year && up.DeletedAt == null).ToListAsync();
            foreach (var universityProgram in universityPrograms)
            {
                foreach (var groupPoint in groupPoints)
                {
                    if (universityProgram.RecordPoint != null && groupPoint.Point != null && universityProgram.RecordPoint + 1 >= groupPoint.Point)
                    {
                        universityProgram.GroupPointId = groupPoint.Id;
                    }
                }
            }

            await _unitOfWork.CommitAsync();
        }
    }
}