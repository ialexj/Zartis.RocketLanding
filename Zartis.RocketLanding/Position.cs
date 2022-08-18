namespace Zartis.RocketLanding
{
    /// <summary>
    /// A possible landing position over a cartesian grid. 
    /// Each unit represents one full square on the grid.
    /// </summary>
    /// <param name="X">The X position on the grid.</param>
    /// <param name="Y">The Y position on the grid.</param>
    public readonly record struct Position(uint X, uint Y)
    {
        public bool CloseTo(Position pos, uint margin)
            => pos.X >= Math.Max(X - (long)margin, 0) && pos.X <= X + margin
            && pos.Y >= Math.Max(Y - (long)margin, 0) && pos.Y <= Y + margin;

        public static Position operator +(Position position, Size size) 
            => new(position.X + size.Width, position.Y + size.Height);
    }

    public static class Positions
    {
        public static IEnumerable<Position> AllBetween(Position a, Position b)
        {
            for (uint x = a.X; x <= b.X; x++) {
                for (uint y = a.Y; y <= b.Y; y++) {
                    yield return new Position(x, y);
                }
            }
        }

        public static IEnumerable<Position> InRectangle(Position origin, Size size) 
            => AllBetween(origin, origin + size);
    }
}
