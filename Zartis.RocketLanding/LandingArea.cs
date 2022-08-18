namespace Zartis.RocketLanding
{
    /// <summary>
    /// Defines an area that can contain a landing platform.
    /// </summary>
    public record class LandingArea
    {
        public LandingArea(Size size, LandingPlatform platform)
        {
            if (size == default)
                throw new ArgumentException("Invalid landing area.");

            if (platform.OuterBound.X > size.Width || platform.OuterBound.Y > size.Height)
                throw new ArgumentException("Platform out of bounds.");

            Size = size;
            Platform = platform;
        }

        public Size Size { get; }

        public LandingPlatform Platform { get; } // One day we might have more than one platform in the area.

        public bool CanLandAt(Position pos)
            // no point checking Size
            // as we already know that the platform is in bounds
            => Platform.IsOnPlatform(pos);                
    }
}
