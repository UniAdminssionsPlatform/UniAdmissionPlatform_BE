using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.UnitTest.AccountService
{
    [TestClass]
    public class GetAllAccountsTest
    {
        [TestMethod()]
        public async Task TestNormalCase()
        {
            var userService = TestAssemblyInitialize.ServiceProvider.GetService<IUserService>();
            // await userService!.GetAllAccounts(new UserAccountBaseViewModel(), null, 1, 10);
        }
    }
}