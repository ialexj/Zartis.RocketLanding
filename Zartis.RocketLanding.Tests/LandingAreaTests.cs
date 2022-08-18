using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zartis.RocketLanding.Tests
{
    [TestClass]
    public class LandingAreaTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyArea()
        {
            new LandingArea(default, new LandingPlatform(default, default));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyPlatform()
        {
            new LandingArea(new Size(10, 10), new LandingPlatform(new Position(1, 1), default));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlatformOutOfBounds()
        {
            new LandingArea(new Size(10, 10), new LandingPlatform(new Position(1, 1), new Size(10, 10)));
        }

        [TestMethod]
        public void CanLandOnPlatformOnly()
        {
            ShouldOnlyLandOnAcceptable(
                    new LandingArea(
                        new Size(100, 100),
                        new LandingPlatform(new Position(5, 5), new Size(10, 10))),

                acceptable:  Positions.AllBetween(new(5, 5), new(15, 15)));

            ShouldOnlyLandOnAcceptable(
                    new LandingArea(
                        new Size(100, 100),
                        new LandingPlatform(new Position(34, 22), new Size(19, 5))),

                acceptable: Positions.AllBetween(new(34, 22), new(34 + 19, 22 + 5)));

            ShouldOnlyLandOnAcceptable(
                    new LandingArea(
                        new Size(100, 100),
                        new LandingPlatform(new Position(0, 0), new Size(1, 1))),

                acceptable: Positions.AllBetween(new(0, 0), new(1, 1)));

            ShouldOnlyLandOnAcceptable(
                    area: new LandingArea(
                        new Size(10, 10),
                        new LandingPlatform(new Position(9, 9), new Size(1, 1))),

                acceptable: Positions.AllBetween(new(9, 9), new(10, 10)));
        }

        static void ShouldOnlyLandOnAcceptable(LandingArea area, IEnumerable<Position> acceptable)
        {
            var set = new HashSet<Position>(acceptable);

            for (uint x = 0; x < area.Size.Width; x++) {
                for (uint y = 0; y < area.Size.Height; y++) {
                    var pos = new Position(x, y);
                    if (set.Contains(pos)) {
                        Assert.IsTrue(area.CanLandAt(pos), $"{pos} should be ok to land.");
                    }
                    else {
                        Assert.IsFalse(area.CanLandAt(pos), $"{pos} should be out of platform.");
                    }
                }
            }
        }
    }
}
