using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zartis.RocketLanding;

namespace Zartis.RocketLanding.Tests
{
    [TestClass]
    public class PositionTests
    {
        static Position Pos(uint x, uint y) => new(x, y);

        static void Test(Position origin, uint margin, Position[] match)
        {
            foreach (uint x in Enumerable.Range(0, 10)) {
                foreach (uint y in Enumerable.Range(0, 10)) {
                    var pos = Pos(x, y);
                    Assert.AreEqual(
                        match.Contains(pos), origin.CloseTo(pos, margin),
                        $"Expected {pos} to be close to {origin} within {margin}.");
                }
            }
        }

        [TestMethod]
        public void NoMargin() => Test(Pos(5, 5), 0,
            new[] {
                Pos(5, 5)
            });

        [TestMethod]
        public void Margin1() => Test(Pos(5, 5), 1, 
            new[] {
                Pos(4, 4), Pos(4, 5), Pos(4, 6),
                Pos(5, 4), Pos(5, 5), Pos(5, 6),
                Pos(6, 4), Pos(6, 5), Pos(6, 6)
            });

        [TestMethod]
        public void Margin2() => Test(Pos(5, 5), 2,
            new[] {
                Pos(3, 3), Pos(3, 4), Pos(3, 5), Pos(3, 6), Pos(3, 7),
                Pos(4, 3), Pos(4, 4), Pos(4, 5), Pos(4, 6), Pos(4, 7),
                Pos(5, 3), Pos(5, 4), Pos(5, 5), Pos(5, 6), Pos(5, 7),
                Pos(6, 3), Pos(6, 4), Pos(6, 5), Pos(6, 6), Pos(6, 7),
                Pos(7, 3), Pos(7, 4), Pos(7, 5), Pos(7, 6), Pos(7, 7),
            });

        [TestMethod]
        public void NegativeOverflow() => Test(Pos(0, 0), 1,
            new[] {
                Pos(0, 0), Pos(0, 1),
                Pos(1, 0), Pos(1, 1)
            });

        
    }
}
