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
    public class PaginationTests
    {
        private static readonly int PageSize = 25;

        private DribbbleClient dribbbleService;

        [TestInitialize]
        public void InitializeTests()
        {
            dribbbleService = new DribbbleClient(PageSize);
            dribbbleService.AccessToken = ConfigurationManager.AppSettings["BearerToken"];
        }

        [TestMethod]
        public async Task TestUserShotsPageSizeAsync()
        {
            var userShots = await dribbbleService.GetUserShotsAsync(1, 1);

            Assert.AreEqual(PageSize, userShots.Count);
        }

        [TestMethod]
        public async Task TestUserLikesPageSizeAsync()
        {
            var userLikes = await dribbbleService.GetUserLikesAsync(1, 1);

            Assert.AreEqual(PageSize, userLikes.Count);
        }
    }
}
