using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IAccountService
    {
        Task<PageResult<AccountViewModelWithHighSchool>> GetAll(
            AccountBaseViewModel accountBaseViewModel, int page, int limit, string sort);
        
        Task<PageResult<AccountViewModelWithUniversity>> GetAllUniAccount(
            AccountBaseViewModel uniAccountUniBaseViewModel, int page, int limit, string sort);

        Task UploadAvatar(int accountId, string avatarUrl);
    }
    public partial class AccountService
    {
        private readonly IConfigurationProvider _mapper;

        public AccountService(IUnitOfWork unitOfWork, IAccountRepository repository, IMapper mapper) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        private const int DefaultPaging = 20;
        private const int LimitPaging = 50;
        
        public async Task<PageResult<AccountViewModelWithHighSchool>> GetAll(AccountBaseViewModel accountBaseViewModel, int page, int limit, string sort)
        {
            var mapper = _mapper.CreateMapper();
            var filter = mapper.Map<AccountViewModelWithHighSchool>(accountBaseViewModel);
            var (total, queryable) = Get().Where(a => a.HighSchoolId != null).ProjectTo<AccountViewModelWithHighSchool>(_mapper).DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<AccountViewModelWithHighSchool>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<AccountViewModelWithUniversity>> GetAllUniAccount(AccountBaseViewModel uniAccountUniBaseViewModel, int page, int limit, string sort)
        {
            var mapper = _mapper.CreateMapper();
            var filter = mapper.Map<AccountViewModelWithUniversity>(uniAccountUniBaseViewModel);
            var (total, queryable) = Get().Where(a => a.UniversityId != null).ProjectTo<AccountViewModelWithUniversity>(_mapper).DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<AccountViewModelWithUniversity>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task UploadAvatar(int accountId, string avatarUrl)
        {
            var account = await Get(a => a.Id == accountId).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new ErrorResponse((int) HttpStatusCode.NotFound, "User không có trên hệ thống!");
            }

            account.ProfileImageUrl = avatarUrl;

            await UpdateAsyn(account);
        }
    }
}