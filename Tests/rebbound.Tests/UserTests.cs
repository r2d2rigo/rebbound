using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Rebbound;
using System.Configuration;
using System.Threading.Tasks;

namespace Rebbound.Tests
{
    [TestClass]
    public class UserTests
    {
        private BaseCredentials credentials;
        private DribbbleClient dribbbleService;

        [TestInitialize]
        public void InitializeTests()
        {
            credentials = new BearerTokenCredentials(ConfigurationManager.AppSettings["BearerToken"]);
            dribbbleService = new DribbbleClient(credentials);
        }

        [TestMethod]
        public async Task TestGetUserAsync()
        {
            var user = await dribbbleService.GetUserAsync(1);

            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("simplebits", user.Username);
            Assert.IsFalse(string.IsNullOrEmpty(user.Name));
            Assert.IsFalse(string.IsNullOrEmpty(user.Bio));
            Assert.IsFalse(string.IsNullOrEmpty(user.AvatarUrl));
            Assert.IsFalse(string.IsNullOrEmpty(user.Location));
        }

        [TestMethod]
        public async Task TestGetUserShotsAsync()
        {
            var shots = await dribbbleService.GetUserShotsAsync(1);

            Assert.IsNotNull(shots);
            Assert.IsTrue(shots.Count > 0);
        }
    }
}
