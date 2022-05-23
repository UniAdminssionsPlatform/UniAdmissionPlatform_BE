using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniAdmissionPlatform.BusinessTier.Generations.Services;

namespace UniAdmissionPlatform.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void TestMethod1()
        {
            var accountService = TestAssemblyInitialize.ServiceProvider.GetService<IAccountService>();
            Console.WriteLine(accountService!.Count());
        }
    }
}