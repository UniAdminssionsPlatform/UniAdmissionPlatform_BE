using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Entities;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Requests.User;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Responses.User;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUserService
    {
        Task<PageResult<UserBaseViewModel>> GetUsers(UserBaseViewModel filter, string sort, int page, int limit);
        Task<LoginResponse> Login(string uid, string email = null);
        Task<LoginResponse> Register(int id, RegisterRequest registerRequest);
        Task<int> CreateHighSchoolManagerAccount(int userId, int highSchoolId,
            RegisterForSchoolManagerRequest registerForSchoolManagerRequest);
        Task<int> CreateUniversityManagerAccount(int userId, int universityId,
            RegisterForUniversityManagerRequest registerForUniversityManagerRequest);
        Task<LoginResponse> CreateStudentAccount(int userId, int highSchoolId,
            RegisterForStudentRequest registerForStudentRequest);
        Task SwitchStatusStudentAccount(int studentId, int highSchoolId = 0);

        Task<PageResult<UserAccountBaseViewModel>> GetHighSchoolManagerStatusPending(UserAccountBaseViewModel filter, string sort,
            int page, int limit, int highSchoolId);

        Task SetActiveForHighSchoolAdmin(int userId, int highSchoolId);

        Task<PageResult<UserAccountBaseViewModel>> GetUniversityManagerStatusPending(UserAccountBaseViewModel filter,
            string sort, int page, int limit, int universityId);

        Task SetActiveForUniversityAdmin(int userId, int universityId);

        Task<PageResult<UserAccountBaseViewModel>> GetAllAccounts(UserAccountBaseViewModel filter, string sort,
            int page, int limit);
    }

    public partial class UserService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationProvider _mapper;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repository, IConfiguration configuration,
            IMapper mapper) : base(
            unitOfWork, repository)
        {
            _configuration = configuration;
            _mapper = mapper.ConfigurationProvider;
        }

        private const int LimitPaging = 50;
        private const int DefaultPaging = 10;

        public async Task<PageResult<UserBaseViewModel>> GetUsers(UserBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get().ProjectTo<UserBaseViewModel>(_mapper)
                .DynamicFilter(filter).PagingIQueryable(page, limit, LimitPaging, DefaultPaging);

            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<UserBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }

        public async Task<LoginResponse> Login(string uid, string email = null)
        {
            var user = await Get().Where(u => u.Uid == uid).Include(u => u.Account).FirstOrDefaultAsync();

            if (user != null)
                return user.Status switch
                {
                    (int)UserStatus.New => GenerateJwtTokenForNewUser(user, email),
                    (int)UserStatus.Pending => throw new ErrorResponse(StatusCodes.Status400BadRequest,
                        "Tài khoản này đang đợi được duyệt."),
                    (int)UserStatus.Active => GenerateJwtTokenForActiveUser(user),
                    (int)UserStatus.Lock => throw new ErrorResponse(StatusCodes.Status400BadRequest,
                        "Tài khoản này đã bị khóa."),
                    _ => null
                };


            var newUser = await InsertNewUser(uid);
            return GenerateJwtTokenForNewUser(newUser, email);
        }

        public async Task<LoginResponse> Register(int id, RegisterRequest registerRequest)
        {
            var mapper = _mapper.CreateMapper();
            var userInRequest = mapper.Map<User>(registerRequest);

            var userInDatabase = await Get().Where(u => u.Id == id).Include(u => u.Account).FirstAsync();

            if (userInDatabase.Account != null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Tài khoản đã đăng ký");
            }

            userInDatabase.Account = userInRequest.Account;
            userInDatabase.Status = (int)UserStatus.Active;
            userInDatabase.UpdatedAt = DateTime.Now;

            await UpdateAsyn(userInDatabase);

            return GenerateJwtTokenForActiveUser(userInDatabase);
        }

        private LoginResponse GenerateJwtTokenForActiveUser(User user)
        {
            var customClaims = new CustomClaims
            {
                UserId = user.Id,
                Role = user.Account.RoleId ?? "",
                UniversityId = user.Account.UniversityId,
                HighSchoolId = user.Account.HighSchoolId,
                OrganizationId = user.Account.OrganizationId,
                BufferTime = long.Parse(_configuration["Jwt:BufferTime"]),
                Exp = DateTime.UtcNow.AddSeconds(Convert.ToDouble(_configuration["Jwt:ExpiresTime"])).Ticks,
                Iss = _configuration["Jwt:Issuer"],
                Nbf = DateTime.UtcNow.AddSeconds(-5).Ticks,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SigningKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(customClaims.ToEnumerableClaims()),
                Expires = new DateTime(customClaims.Exp),
                Issuer = customClaims.Iss,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);
            return new LoginResponse
            {
                UserId = user.Id,
                Roles = customClaims.Role,
                UniversityId = user.Account.UniversityId,
                HighSchoolId = user.Account.HighSchoolId,
                OrganizationId = user.Account.OrganizationId,
                Token = tokenString,
                BufferTime = customClaims.BufferTime * 1000,
                ExpiresAt = ((long)new DateTime(customClaims.Exp).ToUniversalTime().Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalSeconds) * 1000,
                NeedRegister = false,
            };
        }

        private LoginResponse GenerateJwtTokenForNewUser(User user, string email = null)
        {
            var customClaims = new CustomClaims
            {
                UserId = user.Id,
                Role = "",
                BufferTime = long.Parse(_configuration["Jwt:BufferTime"]),
                Exp = DateTime.UtcNow.AddSeconds(Convert.ToDouble(_configuration["Jwt:ExpiresTime"])).Ticks,
                Iss = _configuration["Jwt:Issuer"],
                Nbf = DateTime.UtcNow.AddSeconds(-5).Ticks,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SigningKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(customClaims.ToEnumerableClaims()),
                Expires = new DateTime(customClaims.Exp),
                Issuer = customClaims.Iss,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);
            return new LoginResponse
            {
                UserId = user.Id,
                Roles = customClaims.Role,
                Token = tokenString,
                BufferTime = customClaims.BufferTime * 1000,
                ExpiresAt = ((long)new DateTime(customClaims.Exp).ToUniversalTime().Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalSeconds) * 1000,
                NeedRegister = true,
                EmailContact = email ?? ""
            };
        }

        private async Task<User> InsertNewUser(string uid)
        {
            var user = new User
            {
                Uid = uid,
                Status = (int)UserStatus.New,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await CreateAsyn(user);

            return user;
        }
        

        public async Task<LoginResponse> CreateStudentAccount(int userId, int highSchoolId,
            RegisterForStudentRequest registerForStudentRequest)
        {
            var user = await Get(u => u.Id == userId).Include(u => u.Account).FirstAsync();
            if (user.Status != (int)UserStatus.New)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái người dùng không hợp lệ");
            }

            var mapper = _mapper.CreateMapper();
            var userInRequest = mapper.Map<User>(registerForStudentRequest);
            user.Account = userInRequest.Account;
            user.Account.RoleId = "student";
            user.Account.HighSchoolId = highSchoolId;

            user.Account.Student = new Student
            {
                //todo:
                Status = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            
            user.Status = (int)UserStatus.Active;
            await UpdateAsyn(user);
            return GenerateJwtTokenForActiveUser(user);
        }

        public async Task<int> CreateHighSchoolManagerAccount(int userId, int highSchoolId,
            RegisterForSchoolManagerRequest registerForSchoolManagerRequest)
        {
            var user = await Get(u => u.Id == userId).Include(u => u.Account).FirstAsync();
            if (user.Status != (int)UserStatus.New)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái người dùng không hợp hệ");
            }

            var mapper = _mapper.CreateMapper();
            var userInRequest = mapper.Map<User>(registerForSchoolManagerRequest);
            user.Account = userInRequest.Account;
            user.Account.RoleId = "schoolAdmin";
            user.Account.HighSchoolId = highSchoolId;
            user.Status = (int)UserStatus.Pending;
            await UpdateAsyn(user);
            return user.Id;
        }
        
        public async Task<int> CreateUniversityManagerAccount(int userId, int universityId,
            RegisterForUniversityManagerRequest registerForUniversityManagerRequest)
        {
            var user = await Get(u => u.Id == userId).Include(u => u.Account).FirstAsync();
            if (user.Status != (int)UserStatus.New)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái người dùng không hợp hệ");
            }

            var mapper = _mapper.CreateMapper();
            var userInRequest = mapper.Map<User>(registerForUniversityManagerRequest);
            user.Account = userInRequest.Account;
            user.Account.RoleId = "schoolAdmin";
            user.Account.UniversityId = universityId;
            user.Status = (int)UserStatus.Pending;
            await UpdateAsyn(user);
            return user.Id;
        }

        public async Task ValidateStatus(int userId, UserStatus userStatus)
        {
            var user = await FirstOrDefaultAsyn(u => u.Id == userId);
            if (user == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, "Không thể tìm thấy người dùng.");
            }

            if (user.Status != (int)userStatus)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Trạng thái người dùng không hợp hệ");
            }
        }
        
        public async Task SwitchStatusStudentAccount(int studentId, int highSchoolId = 0)
        {
            var user = await Get().Where(u => u.Id == studentId && u.Status != (int)UserStatus.New && u.Account.Student != null).Include(u => u.Account).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Không tồn tại học sinh này.");
            }
            
            if (highSchoolId != 0 && highSchoolId != user.Account.HighSchoolId)
            {
                throw new ErrorResponse(StatusCodes.Status400BadRequest, "Bạn không thể quản lý học sinh này.");
            }

            user.Status = (int)(user.Status == (int)UserStatus.Active ? UserStatus.Lock : UserStatus.Active);

            await UpdateAsyn(user);
        }
        
        public async Task<PageResult<UserAccountBaseViewModel>> GetHighSchoolManagerStatusPending(UserAccountBaseViewModel filter, string sort, int page, int limit, int highSchoolId)
        {
            var (total, queryable) = Get()
                .Where(u => u.DeletedAt == null 
                            && u.Account.HighSchoolId == highSchoolId 
                            && u.Status == (int)UserStatus.Pending
                            && u.Account.Role.Id == "schoolAdmin")
                .Include( u => u.Account)
                .ThenInclude(u => u.Role)
                .ProjectTo<UserAccountBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<UserAccountBaseViewModel>
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
                .Where(t => t.Id == userId 
                            && t.DeletedAt == null 
                            && t.Status == (int)UserStatus.Pending
                            && t.Account.HighSchoolId == highSchoolId
                            && t.Account.Role.Id == "schoolAdmin")
                .FirstOrDefaultAsync();
            if (highSchoolAdmin == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy user với id = {userId}");
            }
            
            highSchoolAdmin.UpdatedAt = DateTime.Now;
            highSchoolAdmin.Status = (int)UserStatus.Active;
        
            await UpdateAsyn(highSchoolAdmin);
        }
        
        public async Task<PageResult<UserAccountBaseViewModel>> GetUniversityManagerStatusPending(UserAccountBaseViewModel filter, string sort, int page, int limit, int universityId)
        {
            var (total, queryable) = Get()
                .Where(u => u.DeletedAt == null 
                            && u.Account.UniversityId == universityId 
                            && u.Status == (int)UserStatus.Pending
                            && u.Account.Role.Id == "uniAdmin")
                .Include( u => u.Account)
                .ThenInclude(u => u.Role)
                .ProjectTo<UserAccountBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }

            return new PageResult<UserAccountBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
        
        public async Task SetActiveForUniversityAdmin(int userId, int universityId)
        {
            var universityAdmin = await Get()
                .Where(t => t.Id == userId 
                            && t.DeletedAt == null 
                            && t.Status == (int)UserStatus.Pending
                            && t.Account.UniversityId == universityId
                            && t.Account.Role.Id == "uniAdmin")
                .FirstOrDefaultAsync();
            if (universityAdmin == null)
            {
                throw new ErrorResponse(StatusCodes.Status404NotFound, $"Không tìm thấy user với id = {userId}");
            }
            
            universityAdmin.UpdatedAt = DateTime.Now;
            universityAdmin.Status = (int)UserStatus.Active;
        
            await UpdateAsyn(universityAdmin);
        }
        public async Task<PageResult<UserAccountBaseViewModel>> GetAllAccounts(UserAccountBaseViewModel filter, string sort, int page, int limit)
        {
            var (total, queryable) = Get()
                .Where(u => u.DeletedAt == null)
                .ProjectTo<UserAccountBaseViewModel>(_mapper)
                .DynamicFilter(filter)
                .PagingIQueryable(page, limit, LimitPaging, DefaultPaging);
        
            if (sort != null)
            {
                queryable = queryable.OrderBy(sort);
            }
            
            return new PageResult<UserAccountBaseViewModel>
            {
                List = await queryable.ToListAsync(),
                Page = page == 0 ? 1 : page,
                Limit = limit == 0 ? DefaultPaging : limit,
                Total = total
            };
        }
    }
}