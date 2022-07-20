﻿using System.Linq;
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
        Task FollowUniversity(int studentId, int universityId);
        Task<FollowBaseViewModel> GetFollowUniversityById(int studentId, int universityId);
        Task<int> CountFollowByUniversityId(int universityId);
    }
    public partial class FollowService
    {
        private readonly IConfigurationProvider _mapper;
        
        public FollowService(IUnitOfWork unitOfWork, IFollowRepository repository, IMapper mapper) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task FollowUniversity(int studentId, int universityId)
        {
            var followUni = await Get()
                .Where(fu => fu.StudentId == studentId 
                            && fu.UniversityId == universityId)
                .FirstOrDefaultAsync();
            if (followUni == null)
            {
                var follow = new Follow()
                    {
                        StudentId = studentId,
                        UniversityId = universityId,
                        Status = (int)FollowUniversityStatus.Followed
                    };
                await CreateAsyn(follow);
            }
            else
            {
                followUni.Status = (int) (followUni.Status == (int) FollowUniversityStatus.Followed
                    ? FollowUniversityStatus.Unfollowed
                    : FollowUniversityStatus.Followed);
                await UpdateAsyn(followUni);
            }
        }
        
        public async Task<FollowBaseViewModel> GetFollowUniversityById(int studentId, int universityId)
        {
            var followUniversity = await Get()
                .Where(fu => fu.StudentId == studentId 
                             && fu.UniversityId == universityId)
                .ProjectTo<FollowBaseViewModel>(_mapper)
                .FirstOrDefaultAsync();

            if (followUniversity == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest,
                    $"Không tìm thấy theo dõi nào của học sinh có id = {studentId}");
            }
            return followUniversity;
        }
        
        public async Task<int> CountFollowByUniversityId(int universityId)
        {
            var followUniversityById = await Get()
                    .CountAsync(f => f.UniversityId == universityId 
                                     && f.Status == (int)FollowUniversityStatus.Followed);
            return followUniversityById;
        }
    }
}