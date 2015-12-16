using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Tests
{
    [TestClass]
    public class CacheTests
    {
        private DribbbleClient dribbbleService;

        [TestInitialize]
        public void InitializeTests()
        {
            dribbbleService = new DribbbleClient();
            dribbbleService.AccessToken = ConfigurationManager.AppSettings["BearerToken"];
        }

        [TestMethod]
        public async Task TestRateLimitAsync()
        {
            await dribbbleService.GetUserAsync(1);

            Assert.IsTrue(dribbbleService.RateLimit > 0);
        }

        [TestMethod]
        public async Task TestRemainingRequestesAsync()
        {
            await dribbbleService.GetUserAsync(1);
            int remainingStart = dribbbleService.RemainingRequests;

            await dribbbleService.GetUserAsync(1);
            int remainingEnd = dribbbleService.RemainingRequests;

            Assert.AreNotEqual(remainingStart, remainingEnd);
        }
    }
}
