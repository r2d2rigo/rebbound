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
        private DribbbleClient dribbbleService;

        [TestInitialize]
        public void InitializeTests()
        {
            dribbbleService = new DribbbleClient();
            dribbbleService.AccessToken = ConfigurationManager.AppSettings["BearerToken"];
        }

        [TestMethod]
        public async Task TestGetUserByIdAsync()
        {
            var user = await dribbbleService.GetUserAsync(1);

            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("simplebits", user.Username);
            Assert.IsFalse(string.IsNullOrEmpty(user.Name));
            Assert.IsFalse(string.IsNullOrEmpty(user.Bio));
            Assert.IsFalse(string.IsNullOrEmpty(user.AvatarUrl));
            Assert.IsFalse(string.IsNullOrEmpty(user.Location));
            Assert.IsTrue(user.FollowersCount > 0);
            Assert.IsTrue(user.FollowingsCount > 0);

            Assert.IsNotNull(user.Links);
            Assert.IsFalse(string.IsNullOrEmpty(user.Links.Web));
            Assert.IsFalse(string.IsNullOrEmpty(user.Links.Twitter));
        }

        [TestMethod]
        public async Task TestGetUserByUsernameAsync()
        {
            var user = await dribbbleService.GetUserAsync("simplebits");

            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("simplebits", user.Username);
            Assert.IsFalse(string.IsNullOrEmpty(user.Name));
            Assert.IsFalse(string.IsNullOrEmpty(user.Bio));
            Assert.IsFalse(string.IsNullOrEmpty(user.AvatarUrl));
            Assert.IsFalse(string.IsNullOrEmpty(user.Location));
            Assert.IsTrue(user.FollowersCount > 0);
            Assert.IsTrue(user.FollowingsCount > 0);

            Assert.IsNotNull(user.Links);
            Assert.IsFalse(string.IsNullOrEmpty(user.Links.Web));
            Assert.IsFalse(string.IsNullOrEmpty(user.Links.Twitter));
        }

        [TestMethod]
        public async Task TestGetUserShotsAsync()
        {
            var shots = await dribbbleService.GetUserShotsAsync(1, 1);

            Assert.IsNotNull(shots);
            Assert.IsTrue(shots.Count > 0);
        }

        [TestMethod]
        public async Task TestGetFollowingShotsAsync()
        {
            var shots = await dribbbleService.GetFollowingShotsAsync();

            Assert.IsNotNull(shots);
            Assert.IsTrue(shots.Count > 0);
        }
    }
}
