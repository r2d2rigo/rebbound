using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rebbound.Cache;
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
        private DribbbleClient cachedDribbbleService;

        [TestInitialize]
        public void InitializeTests()
        {
            dribbbleService = new DribbbleClient();
            dribbbleService.AccessToken = ConfigurationManager.AppSettings["BearerToken"];

            cachedDribbbleService = new DribbbleClient(new InMemoryCachedHttpClient());
            cachedDribbbleService.AccessToken = ConfigurationManager.AppSettings["BearerToken"];
        }

        [TestMethod]
        public async Task TestRateLimitAsync()
        {
            await dribbbleService.GetUserAsync(1);

            Assert.IsTrue(dribbbleService.RateLimit > 0);
        }

        [TestMethod]
        public async Task TestRemainingRequestsAsync()
        {
            await dribbbleService.GetUserAsync(1);
            int remainingStart = dribbbleService.RemainingRequests;

            await dribbbleService.GetUserAsync(1);
            int remainingEnd = dribbbleService.RemainingRequests;

            Assert.AreNotEqual(remainingStart, remainingEnd);
        }

        [TestMethod]
        public async Task TestCachedGetUserAsync()
        {
            var originalUser = await cachedDribbbleService.GetUserAsync(1);
            int remainingStart = dribbbleService.RemainingRequests;

            var cachedUser = await cachedDribbbleService.GetUserAsync(1);
            int remainingEnd = dribbbleService.RemainingRequests;

            Assert.AreEqual(remainingStart, remainingEnd);
        }
    }
}
