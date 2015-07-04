using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Rebbound;
using System.Configuration;
using System.Threading.Tasks;

namespace rebbound.Tests
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
        public async Task TestGetUserByIdAsync()
        {
            var user = await dribbbleService.GetUserByIdAsync(1);

            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("simplebits", user.Username);
            Assert.IsFalse(string.IsNullOrEmpty(user.Name));
            Assert.IsFalse(string.IsNullOrEmpty(user.Bio));
            Assert.IsFalse(string.IsNullOrEmpty(user.AvatarUrl));
            Assert.IsFalse(string.IsNullOrEmpty(user.Location));
        }
    }
}
