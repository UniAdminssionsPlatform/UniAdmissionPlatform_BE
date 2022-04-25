using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IAccountService
    {
        Task<PageResult<AccountViewModelWithHighSchool>> GetAll(
            AccountBaseViewModel accountBaseViewModel, int page, int limit, string sort);
        Task<PageResult<AccountViewModelWithUniversity>> GetAllUniAccount(
            AccountBaseViewModel uniAccountUniBaseViewModel, int page, int limit, string sort);

        Task<PageResult<AccountBaseViewModel>> GetAllAccounts(
            AccountBaseViewModel filter, string sort, int page, int limit);

        Task UploadAvatar(int accountId, string avatarUrl);
        Task UpdateUniAccount(int id, UpdateUniAccountRequest updateUniAccountRequest);
        Task UpdateAccount(int id, UpdateAccountRequestForAdmin updateAccountRequestForAdmin);
    }
    public partial class AccountService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IRoleService _roleService;

        public AccountService(IUnitOfWork unitOfWork, IAccountRepository repository, IMapper mapper, IRoleService roleService) : base(unitOfWork,
            repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _roleService = roleService;
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
        
        public async Task<PageResult<AccountBaseViewModel>> GetAllAccounts(AccountBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().ProjectTo<AccountBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
        
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<AccountBaseViewModel>
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
                throw new ErrorResponse(StatusCodes.Status404NotFound, "User không có trên hệ thống!");
            }

            account.ProfileImageUrl = avatarUrl;

            await UpdateAsyn(account);
        }

        public async Task UpdateUniAccount(int id, UpdateUniAccountRequest updateUniAccountRequest)
        {
            var uniAccount = await Get().Where(a => a.Id == id).FirstOrDefaultAsync();
            if (uniAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tài khoản với id = {id}");
            }
            
            var mapper = _mapper.CreateMapper();
            uniAccount = mapper.Map(updateUniAccountRequest, uniAccount);
            
            await UpdateAsyn(uniAccount);
        }
        
        public async Task UpdateAccount(int id, UpdateAccountRequestForAdmin updateAccountRequestForAdmin)
        {
            RoleBaseViewModel role;
            try
            {
                role = await _roleService.GetRoleById(updateAccountRequestForAdmin.RoleId);
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw;
                }
            }

            try
            {
                ValidateRoleAndIdentifyId(updateAccountRequestForAdmin, (IdentifyIdEnum)role.IdentifyId);
            }
            catch (ErrorResponse e)
            {
                switch (e.Error.Code)
                {
                    default:
                        throw;
                }
            }

            var uapAccount = await Get().Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (uapAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tài khoản với id = {id}");
            }

            var mapper = _mapper.CreateMapper();
            uapAccount = mapper.Map(updateAccountRequestForAdmin, uapAccount);
            if (updateAccountRequestForAdmin.UniversityId == null)
            {
                uapAccount.UniversityId = null;
            }
            
            if (updateAccountRequestForAdmin.HighSchoolId == null)
            {
                uapAccount.HighSchoolId = null;
            }
            
            if (updateAccountRequestForAdmin.OrganizationId == null)
            {
                uapAccount.OrganizationId = null;
            }
            
            await UpdateAsyn(uapAccount);
        }

        private static void ValidateRoleAndIdentifyId(UpdateAccountRequestForAdmin accountRequestForAdmin, IdentifyIdEnum identifyIdEnum)
        {
            int? highSchoolId = accountRequestForAdmin.HighSchoolId; 
            int? universityId = accountRequestForAdmin.UniversityId;
            int? organizationId = accountRequestForAdmin.OrganizationId;
            switch (identifyIdEnum)
            {
                case IdentifyIdEnum.HighSchoolId:
                    if (highSchoolId == null || universityId != null || organizationId != null)
                    {
                        throw new ErrorResponse(StatusCodes.Status400BadRequest,
                            "Người dùng này chỉ thuộc về 1 trường cấp 3");
                    }
                    break;
                case IdentifyIdEnum.UniversityId:
                    if (highSchoolId != null || universityId == null || organizationId != null)
                    {
                        throw new ErrorResponse(StatusCodes.Status400BadRequest,
                            "Người dùng này chỉ thuộc về 1 trường đại học");
                    }
                    break;
                case IdentifyIdEnum.OrganizationId:
                    if (highSchoolId != null || universityId == null || organizationId != null)
                    {
                        throw new ErrorResponse(StatusCodes.Status400BadRequest,
                            "Người dùng này chỉ thuộc về 1 tổ chức.");
                    }
                    break;
            }
        }
    }
}