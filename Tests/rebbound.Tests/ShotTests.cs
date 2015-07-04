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
    public class ShotTests
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
        public async Task TestGetShotAsync()
        {
            var shot = await dribbbleService.GetShotAsync(1);

            Assert.AreEqual(1, shot.Id);
            Assert.AreEqual("Working on the new shop", shot.Title);
            Assert.IsTrue(string.IsNullOrEmpty(shot.Description));
            Assert.IsTrue(shot.Height > 0);
            Assert.IsTrue(shot.Width > 0);
            Assert.IsNotNull(shot.Images);
            Assert.IsTrue(string.IsNullOrEmpty(shot.Images.HiDpi));
            Assert.IsFalse(string.IsNullOrEmpty(shot.Images.Normal));
            Assert.IsFalse(string.IsNullOrEmpty(shot.Images.Teaser));
        }
    }
}

