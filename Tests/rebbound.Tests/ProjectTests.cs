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
    class ProjectTests
    {
        private DribbbleClient dribbbleService;

        [TestInitialize]
        public void InitializeTests()
        {
            dribbbleService = new DribbbleClient();
            dribbbleService.AccessToken = ConfigurationManager.AppSettings["BearerToken"];
        }

        [TestMethod]
        public async Task TestGetProjectAsync()
        {
            var project = await dribbbleService.GetProjectAsync(1);

            Assert.AreEqual(1, project.Id);
        }

        [TestMethod]
        public async Task TestGetProjectShotsAsync()
        {
            var projectShots = await dribbbleService.GetProjectShotsAsync(1);

            Assert.IsNotNull(projectShots);
            Assert.AreNotEqual(0, projectShots.Count);
        }
    }
}
