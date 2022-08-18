namespace Zartis.RocketLanding
{
    using static LandingStatus;

    public enum LandingStatus
    {
        Ok,
        OutOfPlatform,
        Clash
    }

    public static class LandingStatuses
    {
        public static string AsString(this LandingStatus status)
            => status switch {
                Ok => "ok for landing",
                OutOfPlatform => "out of platform",
                Clash => "clash",
                _ => throw new NotSupportedException()
            };
    }
}
