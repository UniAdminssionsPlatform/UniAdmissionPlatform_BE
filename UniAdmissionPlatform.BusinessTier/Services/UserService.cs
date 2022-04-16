using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Entities;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.User;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Responses.User;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface IUserService
    {
        Task<LoginResponse> Login(string uid);
        Task<LoginResponse> Register(int id, RegisterRequest registerRequest);
    }
    public partial class UserService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationProvider _mapper;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repository, IConfiguration configuration, IMapper mapper) : base(
            unitOfWork, repository)
        {
            _configuration = configuration;
            _mapper = mapper.ConfigurationProvider;
        }
        
        public async Task<LoginResponse> Login(string uid)
        {
            var user = await Get().Where(u => u.Uid == uid).Include(u => u.Account).FirstOrDefaultAsync();

            if (user != null)
                return user.Status switch
                {
                    (int)UserStatus.New => GenerateJwtTokenForNewUser(user),
                    (int)UserStatus.Lock => throw new ErrorResponse((int)HttpStatusCode.BadRequest,
                        "Tài khoản này đã bị khóa"),
                    (int)UserStatus.Active => GenerateJwtTokenForActiveUser(user),
                    _ => null
                };
            
            
            var newUser = await InsertNewUser(uid);
            return GenerateJwtTokenForNewUser(newUser);
        }

        public async Task<LoginResponse> Register(int id, RegisterRequest registerRequest)
        {
            var mapper = _mapper.CreateMapper();
            var userInRequest = mapper.Map<User>(registerRequest);

            var userInDatabase = await Get().Where(u => u.Id == id).Include(u => u.Account).FirstAsync();

            if (userInDatabase.Account != null)
            {
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Tài khoản đã đăng ký");
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
        
        private LoginResponse GenerateJwtTokenForNewUser(User user)
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
        
        
    }
}