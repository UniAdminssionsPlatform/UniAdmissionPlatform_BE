using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.ViewModels;

namespace UniAdmissionPlatform.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void TestMethod1()
        {
            var userService = TestAssemblyInitialize.ServiceProvider.GetService<IUserService>();
            userService!.GetAllAccounts(new UserAccountBaseViewModel(), null, 1, 10);
        }
        
        
    }
}