using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zartis.RocketLanding.Tests
{
    [TestClass]
    public class LandingCoordinatorTests
    {
        const int Rocket0 = 0;
        const int Rocket1 = 1;
        const int Rocket2 = 2;

        [TestMethod]
        public void CanLandOnPlatformOnly()
        {
            // Ensure that coordinator accepts positions within landing area, and none outside
            static void LandingsShouldMatchPlatform(LandingCoordinator landing)
            {
                foreach (var pos in Positions.InRectangle(default, landing.Area.Size)) {
                    Assert.AreEqual(
                        landing.RequestLanding(Rocket0, pos) == LandingStatus.Ok,
                        landing.Area.CanLandAt(pos));
                }
            }

            LandingsShouldMatchPlatform(
                new LandingCoordinator(
                    new LandingArea(
                        new Size(100, 100),
                        new LandingPlatform(new Position(5, 5), new Size(10, 10)))));

            LandingsShouldMatchPlatform(
                new LandingCoordinator(
                    new LandingArea(
                        new Size(100, 100),
                        new LandingPlatform(new Position(34, 22), new Size(19, 5)))));

            LandingsShouldMatchPlatform(
                new LandingCoordinator(
                    new LandingArea(
                        new Size(100, 100),
                        new LandingPlatform(new Position(0, 0), new Size(1, 1)))));

            LandingsShouldMatchPlatform(
                new LandingCoordinator(
                    area: new LandingArea(
                        new Size(10, 10),
                        new LandingPlatform(new Position(9, 9), new Size(1, 1)))));
        }

        [TestMethod]
        public void RespectsSafetyMargin()
        {
            var coordinator = new LandingCoordinator(
                new LandingArea(
                    new Size(100, 100),
                    new LandingPlatform(new(5, 5), new(10, 10))),
                safetyMargin: 1);

            // Should clash around rocket
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket0, new(5, 5)));
            ShouldClashAt(coordinator, Rocket1, Positions.AllBetween(new(4, 4), new(6, 6)));

            // Move Rocket 0 and check again
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket0, new(6, 6)));
            ShouldClashAt(coordinator, Rocket1, Positions.AllBetween(new(5, 5), new(7, 7)));

            // Now Rocket 1 reserves a spot, so Rocket 0 should clash
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket1, new(9, 9)));
            ShouldClashAt(coordinator, Rocket0, Positions.AllBetween(new(8, 8), new(10, 10)));
        }

        [TestMethod]
        public void LargerSafetyMargin()
        {
            var coordinator = new LandingCoordinator(
                new LandingArea(
                    new Size(100, 100),
                    new LandingPlatform(new(5, 5), new(10, 10))),
                safetyMargin: 2);

            // Should clash around rocket
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket0, new(5, 5)));
            ShouldClashAt(coordinator, Rocket1, Positions.AllBetween(new(3, 3), new(7, 7)));
        }

        [TestMethod]
        public void NoSafetyMargin()
        {
            var coordinator = new LandingCoordinator(
                new LandingArea(
                    new Size(100, 100),
                    new LandingPlatform(new(5, 5), new(10, 10))),
                safetyMargin: 0);

            // Should clash around rocket
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket0, new(5, 5)));
            ShouldClashAt(coordinator, Rocket1, Positions.AllBetween(new(5, 5), new(5, 5)));
        }

        [TestMethod]
        public void ClashingRocketsReservePosition()
        {
            var coordinator = new LandingCoordinator(
                new LandingArea(
                    new Size(100, 100),
                    new LandingPlatform(new Position(5, 5), new Size(10, 10))),
                safetyMargin: 1);

            // Rocket0 reserves a spot with a safety margin
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket0, new(5, 5)));

            // Rocket1 should clash, however, it should still reserve a position
            Assert.AreEqual(LandingStatus.Clash, coordinator.RequestLanding(Rocket1, new(6, 6)));

            // Rocket2 will clash with Rocket1's potential landing site, even though it doesn't clash with Rocket0
            Assert.AreEqual(LandingStatus.Clash, coordinator.RequestLanding(Rocket2, new(7, 7)));

            // Rocket2 corrected itself to outside both clashing zones.
            Assert.AreEqual(LandingStatus.Ok, coordinator.RequestLanding(Rocket2, new(8, 8)));
        }

        [TestMethod]
        public void RocketsOffPlatformReservePosition()
        {
            var coordinator = new LandingCoordinator(
                new LandingArea(
                    new Size(100, 100),
                    new LandingPlatform(new Position(5, 5), new Size(10, 10))),
                safetyMargin: 1);

            // Rocket0 reserves a spot with a safety margin even though it's landing outside the platform.
            Assert.AreEqual(LandingStatus.OutOfPlatform, coordinator.RequestLanding(Rocket0, new(16, 16)));

            // Rocket1 should clash.
            Assert.AreEqual(LandingStatus.Clash, coordinator.RequestLanding(Rocket1, new(15, 15)));
        }


        // Ensure that can land on non-clashing and can't land on clashing
        static void ShouldClashAt(LandingCoordinator landing, int rocketID, IEnumerable<Position> clashes)
        {
            var set = new HashSet<Position>(clashes);

            foreach (var pos in Positions.InRectangle(landing.Area.Platform.Position, landing.Area.Platform.Size)) {
                var res = landing.RequestLanding(rocketID, pos);
                if (set.Contains(pos)) {
                    Assert.AreEqual(LandingStatus.Clash, res);
                }
                else {
                    Assert.AreEqual(LandingStatus.Ok, res);
                }
            }
        }
    }
}
