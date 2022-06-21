using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Follow;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IFollowService
    {
        Task<int> CreateFollow(int studentId, CreateFollowRequest createFollowRequest);

    }
    public partial class FollowService
    {
        private readonly IConfigurationProvider _mapper;
        
        public FollowService(IUnitOfWork unitOfWork, IFollowRepository repository, IMapper mapper) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        

        public async Task<int> CreateFollow(int studentId, CreateFollowRequest createFollowRequest)
        {
            var follow = _mapper.CreateMapper().Map<Follow>(createFollowRequest);
            follow.StudentId = studentId;
            follow.Status = (int)FollowUniversityStatus.Followed;

            await CreateAsyn(follow);
            return follow.StudentId;
        }
        
    }
}