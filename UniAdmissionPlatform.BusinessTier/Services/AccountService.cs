﻿using System;
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
        Task<PageResult<AccountViewModelWithHighSchool>> GetAllStudents(AccountBaseViewModel accountBaseViewModel,
            int page, int limit, string sort);
        Task UploadAvatar(int accountId, string avatarUrl);
        Task UpdateUniAccount(int id, UpdateProfileRequest updateProfileRequest);
        Task UpdateAccount(int id, UpdateAccountRequestForAdmin updateAccountRequestForAdmin);
        Task<AccountStudentByIdViewModelWithHighSchool> GetStudentAccountById(int studentId);

        Task<PageResult<ManagerAccountBaseViewModel>> GetAllAccountForAdmin(
            ManagerAccountBaseViewModel managerAccountBaseViewModel, int page, int limit, string sort);
        Task<PageResult<ManagerAccountBaseViewModel>> GetUniversityManagerStatusPending(ManagerAccountBaseViewModel filter,
            string sort, int page, int limit, int universityId);
        Task<PageResult<ManagerAccountBaseViewModel>> GetHighSchoolManagerStatusPending(ManagerAccountBaseViewModel filter, string sort,
            int page, int limit, int highSchoolId);
        Task SetActiveForHighSchoolAdmin(int userId, int highSchoolId);
        Task SetActiveForUniversityAdmin(int userId, int universityId);
        Task<AccountAdminHighSchoolByIdViewModelWithHighSchool> GetAdminHighSchoolAccountById(int adminHighSchoolId);
        Task<AccountAdminUniversityByIdViewModelWithHighSchool> GetAdminUniversityAccountById(int adminUniversityId);
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
            var (total, queryable) = Get().Include(a => a.IdNavigation)
                .Where(a => a.HighSchoolId != null)
                .ProjectTo<AccountViewModelWithHighSchool>(_mapper).DynamicFilter(filter)
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
        
        public async Task<PageResult<AccountViewModelWithHighSchool>> GetAllStudents(AccountBaseViewModel accountBaseViewModel, int page, int limit, string sort)
        {
            var mapper = _mapper.CreateMapper();
            var filter = mapper.Map<AccountViewModelWithHighSchool>(accountBaseViewModel);
            var (total, queryable) = Get().Include(a => a.IdNavigation)
                .Where(a => a.HighSchoolId != null && a.Student != null)
                .ProjectTo<AccountViewModelWithHighSchool>(_mapper).DynamicFilter(filter)
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
                throw new ErrorResponse(StatusCodes.Status404NotFound, "User không có trên hệ thống!");
            }

            account.ProfileImageUrl = avatarUrl;

            await UpdateAsyn(account);
        }

        public async Task UpdateUniAccount(int id, UpdateProfileRequest updateProfileRequest)
        {
            var uniAccount = await Get().Where(a => a.Id == id).FirstOrDefaultAsync();
            if (uniAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tài khoản với id = {id}");
            }
            
            var mapper = _mapper.CreateMapper();
            uniAccount = mapper.Map(updateProfileRequest, uniAccount);
            
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
        
        public async Task<AccountStudentByIdViewModelWithHighSchool> GetStudentAccountById(int studentId)
        {
            
            var studentAccount = await Get()
                .Where(a => a.Id == studentId)
                .Include(a => a.IdNavigation)
                .ProjectTo<AccountStudentByIdViewModelWithHighSchool>(_mapper).FirstOrDefaultAsync();
            if (studentAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tài khoản id = {studentId}.");
            }
            return studentAccount;
        }
        
        public async Task<AccountAdminHighSchoolByIdViewModelWithHighSchool> GetAdminHighSchoolAccountById(int adminHighSchoolId)
        {
            
            var highSchoolAccount = await Get()
                .Where(a => a.Id == adminHighSchoolId)
                .Include(a => a.IdNavigation)
                .ProjectTo<AccountAdminHighSchoolByIdViewModelWithHighSchool>(_mapper).FirstOrDefaultAsync();
            if (highSchoolAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tài khoản id = {adminHighSchoolId}.");
            }
            return highSchoolAccount;
        }
        
        public async Task<AccountAdminUniversityByIdViewModelWithHighSchool> GetAdminUniversityAccountById(int adminUniversityId)
        {
            
            var adminUniversityAccount = await Get()
                .Where(a => a.Id == adminUniversityId)
                .Include(a => a.IdNavigation)
                .ProjectTo<AccountAdminUniversityByIdViewModelWithHighSchool>(_mapper).FirstOrDefaultAsync();
            if (adminUniversityAccount == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy tài khoản id = {adminUniversityId}.");
            }
            return adminUniversityAccount;
        }
        
        public async Task<PageResult<ManagerAccountBaseViewModel>> GetAllAccountForAdmin(ManagerAccountBaseViewModel managerAccountBaseViewModel, int page, int limit, string sort)
        {
            var mapper = _mapper.CreateMapper();
            var filter = mapper.Map<ManagerAccountBaseViewModel>(managerAccountBaseViewModel);
            var (total, queryable) = Get()
                .ProjectTo<ManagerAccountBaseViewModel>(_mapper).DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<ManagerAccountBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<ManagerAccountBaseViewModel>> GetUniversityManagerStatusPending(ManagerAccountBaseViewModel filter, string sort, int page, int limit, int universityId)
        {
            var (total, queryable) = Get()
                .Where(a => a.RoleId == "uniAdmin" 
                            && a.UniversityId == universityId 
                            && a.IdNavigation.Status == (int)UserStatus.Pending 
                            && a.IdNavigation.DeletedAt == null)
                .Include(a=> a.IdNavigation)
                .ProjectTo<ManagerAccountBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<ManagerAccountBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task<PageResult<ManagerAccountBaseViewModel>> GetHighSchoolManagerStatusPending(ManagerAccountBaseViewModel filter, string sort, int page, int limit, int highSchoolId)
        {
            var (total, queryable) = Get()
                .Where(a => a.RoleId == "schoolAdmin" 
                            && a.HighSchoolId == highSchoolId 
                            && a.IdNavigation.Status == (int)UserStatus.Pending 
                            && a.IdNavigation.DeletedAt == null)
                .Include(a=> a.IdNavigation)
                .ProjectTo<ManagerAccountBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<ManagerAccountBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task SetActiveForHighSchoolAdmin(int userId, int highSchoolId)
        {
            var highSchoolAdmin = await Get()
                .Where(a => a.Id == userId 
                            && a.IdNavigation.DeletedAt == null 
                            && a.IdNavigation.Status == (int)UserStatus.Pending
                            && a.HighSchoolId == highSchoolId
                            && a.RoleId == "schoolAdmin")
                .Include(a=> a.IdNavigation)
                .FirstOrDefaultAsync();
            if (highSchoolAdmin == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy user với id = {userId}");
            }
            
            highSchoolAdmin.IdNavigation.UpdatedAt = DateTime.Now;
            highSchoolAdmin.IdNavigation.Status = (int)UserStatus.Active;
        
            await UpdateAsyn(highSchoolAdmin);
        }
        
        public async Task SetActiveForUniversityAdmin(int userId, int universityId)
        {
            var universityAdmin = await Get()
                .Where(a => a.Id == userId 
                            && a.IdNavigation.DeletedAt == null 
                            && a.IdNavigation.Status == (int)UserStatus.Pending
                            && a.UniversityId == universityId
                            && a.RoleId == "uniAdmin")
                .Include(a=> a.IdNavigation)
                .FirstOrDefaultAsync();
            if (universityAdmin == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy user với id = {userId}");
            }
            
            universityAdmin.IdNavigation.UpdatedAt = DateTime.Now;
            universityAdmin.IdNavigation.Status = (int)UserStatus.Active;
        
            await UpdateAsyn(universityAdmin);
        }
    }
}