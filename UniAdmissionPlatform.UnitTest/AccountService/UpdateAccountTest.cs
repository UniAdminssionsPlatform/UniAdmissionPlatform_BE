using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Account;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.UnitTest.AccountService
{
    [TestClass]
    public class UpdateAccountTest
    {
        [TestMethod()]
        public async Task TestNormalCase()
        {
            var accountService = TestAssemblyInitialize.ServiceProvider.GetService<IAccountService>();
            var now = DateTime.Now;
            var accountId = 1;
            await accountService!.UpdateAccount(accountId, new UpdateAccountRequestForAdmin
            {
                FirstName = "Tài",
                MiddleName = "Văn",
                LastName = "Cung",
                Address = "Thành phố Thủ Đức",
                Nationality = "Việt Nam",
                Religion = "Phật giáo",
                PhoneNumber = "0584465488",
                ProfileImageUrl = "https://thuvienanime.com/wp-content/uploads/2021/09/thong-tin-ve-nhan-vat-han-lap-min.jpeg",
                IdCard = "5588458888",
                PlaceOfBirth = "Thành phố Hồ Chí Minh",
                DateOfBirth = now,
                GenderId = 2,
                EmailContact = "bacnvse141019@fpt.edu.vn",
                RoleId = "schoolAdmin",
                WardId = 5253,
                HighSchoolId = 1
            });
            
            // test start
            var account = await accountService.FirstOrDefaultAsyn(a => a.Id == accountId);
            Assert.AreEqual(now.Ticks, account.DateOfBirth.Ticks);
        }

        [TestMethod()]
        public async Task NotFoundAccountIdExceptionCase()
        {
            var accountService = TestAssemblyInitialize.ServiceProvider.GetService<IAccountService>();
            var now = DateTime.Now;
            var accountId = -1;
            try
            {
                await accountService!.UpdateAccount(accountId, new UpdateAccountRequestForAdmin
                {
                    FirstName = "Tài",
                    MiddleName = "Văn",
                    LastName = "Cung",
                    Address = "Thành phố Thủ Đức",
                    Nationality = "Việt Nam",
                    Religion = "Phật giáo",
                    PhoneNumber = "0584465488",
                    ProfileImageUrl = "https://thuvienanime.com/wp-content/uploads/2021/09/thong-tin-ve-nhan-vat-han-lap-min.jpeg",
                    IdCard = "5588458888",
                    PlaceOfBirth = "Thành phố Hồ Chí Minh",
                    DateOfBirth = now,
                    GenderId = 2,
                    EmailContact = "bacnvse141019@fpt.edu.vn",
                    RoleId = "schoolAdmin",
                    WardId = 5253,
                    HighSchoolId = 1
                });
            }
            catch (ErrorResponse e)
            {
                // test start
                Assert.AreEqual(e.Error.Code, StatusCodes.Status404NotFound);
                return;
            }
            
            Assert.IsTrue(false);
        }

        [TestMethod]
        public async Task ThisAccountOnlyBelongToAHighSchoolExceptionCase()
        {
            var accountService = TestAssemblyInitialize.ServiceProvider.GetService<IAccountService>();
            var now = DateTime.Now;
            var accountId = -1;
            try
            {
                await accountService!.UpdateAccount(accountId, new UpdateAccountRequestForAdmin
                {
                    FirstName = "Tài",
                    MiddleName = "Văn",
                    LastName = "Cung",
                    Address = "Thành phố Thủ Đức",
                    Nationality = "Việt Nam",
                    Religion = "Phật giáo",
                    PhoneNumber = "0584465488",
                    ProfileImageUrl = "https://thuvienanime.com/wp-content/uploads/2021/09/thong-tin-ve-nhan-vat-han-lap-min.jpeg",
                    IdCard = "5588458888",
                    PlaceOfBirth = "Thành phố Hồ Chí Minh",
                    DateOfBirth = now,
                    GenderId = 2,
                    EmailContact = "bacnvse141019@fpt.edu.vn",
                    RoleId = "schoolAdmin",
                    WardId = 5253,
                    HighSchoolId = 1,
                    UniversityId = 1
                });
            }
            catch (ErrorResponse e)
            {
                // test start
                Assert.AreEqual(e.Error.Code, StatusCodes.Status400BadRequest);
                return;
            }
            
            Assert.IsTrue(false);
        }
        
        [TestMethod]
        public async Task ThisAccountOnlyBelongToAUniversityExceptionCase()
        {
            var accountService = TestAssemblyInitialize.ServiceProvider.GetService<IAccountService>();
            var now = DateTime.Now;
            var accountId = -1;
            try
            {
                await accountService!.UpdateAccount(accountId, new UpdateAccountRequestForAdmin
                {
                    FirstName = "Tài",
                    MiddleName = "Văn",
                    LastName = "Cung",
                    Address = "Thành phố Thủ Đức",
                    Nationality = "Việt Nam",
                    Religion = "Phật giáo",
                    PhoneNumber = "0584465488",
                    ProfileImageUrl = "https://thuvienanime.com/wp-content/uploads/2021/09/thong-tin-ve-nhan-vat-han-lap-min.jpeg",
                    IdCard = "5588458888",
                    PlaceOfBirth = "Thành phố Hồ Chí Minh",
                    DateOfBirth = now,
                    GenderId = 2,
                    EmailContact = "bacnvse141019@fpt.edu.vn",
                    RoleId = "uniAdmin",
                    WardId = 5253,
                    HighSchoolId = 1,
                    UniversityId = 1
                });
            }
            catch (ErrorResponse e)
            {
                // test start
                Assert.AreEqual(e.Error.Code, StatusCodes.Status400BadRequest);
                return;
            }
            
            Assert.IsTrue(false);
        }
        
        
    }
}