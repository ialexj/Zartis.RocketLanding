using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zartis.RocketLanding.Tests
{
    [TestClass]
    public class LandingPlatformTests
    {
        [TestMethod]
        public void Simple()
        {
            var platform = new LandingPlatform(new(5, 5), new(10, 10));

            Assert.IsTrue(platform.IsOnPlatform(new(5, 5)));
            Assert.IsTrue(platform.IsOnPlatform(new(15, 15)));

            Assert.IsFalse(platform.IsOnPlatform(new(4, 4)));
            Assert.IsFalse(platform.IsOnPlatform(new(4, 5)));
            Assert.IsFalse(platform.IsOnPlatform(new(5, 4)));

            Assert.IsFalse(platform.IsOnPlatform(new(15, 16)));
            Assert.IsFalse(platform.IsOnPlatform(new(16, 15)));
            Assert.IsFalse(platform.IsOnPlatform(new(16, 16)));
        }
    }
}
