namespace Zartis.RocketLanding
{
    /// <summary>
    /// Defines a landing platform in coordinates relative to a landing area.
    /// </summary>
    public record class LandingPlatform
    {
        public LandingPlatform(Position position, Size size)
        {
            if (size == default) {
                throw new ArgumentException("Invalid platform size.");
            }

            Position = position;
            Size = size;
        }

        public Position Position { get; }

        public Size Size { get; }

        public Position OuterBound => Position + Size;

        public bool IsOnPlatform(Position pos)
            => pos.X >= Position.X && pos.X <= OuterBound.X
            && pos.Y >= Position.Y && pos.Y <= OuterBound.Y;
    }
}
