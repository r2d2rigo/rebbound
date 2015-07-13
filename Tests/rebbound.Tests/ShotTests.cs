using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rebbound.Models;
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

        [TestMethod]
        public async Task TestGetShotPaletteAsync()
        {
            var palette = await dribbbleService.GetShotPaletteAsync(1);

            Assert.AreEqual(6, palette.Count);

            var colours = new[]
                {
                    new RgbColor(159, 145, 149),
                    new RgbColor(237, 237, 229),
                    new RgbColor(52, 52, 84),
                    new RgbColor(28, 26, 32),
                    new RgbColor(96, 87, 100),
                    new RgbColor(153, 105, 90),
                };

            for (int i = 0; i < 6; i++ )
            {
                Assert.AreEqual(colours[0].R, palette[0].R);
                Assert.AreEqual(colours[0].G, palette[0].G);
                Assert.AreEqual(colours[0].B, palette[0].B);
            }
        }

        [TestMethod]
        public async Task TestGetShotCommentsAsync()
        {
            var comments = await dribbbleService.GetShotCommentsAsync(1);

            Assert.IsTrue(comments.Count >= 12);

            Assert.AreEqual(1, comments[0].Id);
            Assert.IsTrue(comments[0].LikesCount >= 2);
            Assert.AreEqual("owltastic", comments[0].User.Username);
        }
    }
}

