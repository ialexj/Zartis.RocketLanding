namespace Zartis.RocketLanding
{
    /// <summary>
    /// Coordinates landings between multiple rockets.
    /// </summary>
    public class LandingCoordinator
    {
        readonly Dictionary<int, Rocket> _rockets = new();

        public LandingCoordinator(LandingArea area, uint safetyMargin = 1)
        {
            Area = area ?? throw new ArgumentNullException(nameof(area));
            SafetyMargin = safetyMargin;
        }

        public LandingArea Area { get; }

        public uint SafetyMargin { get; }

        public LandingStatus RequestLanding(int rocketID, Position pos)
        {
            // If a rocket is about to land at a position where it would clash with the safety margin of another rocket,
            //   should a new safety margin be reserved for the clashing rocket?
            // The safe bet is yes, since the rocket might still end up landing at a reserved position.
            // We also reserve space for rockets that happen to land outside the platform.

            StoreLastPosition(rocketID, pos);
            
            if (!Area.CanLandAt(pos))
                return LandingStatus.OutOfPlatform;
            else if (ClashesWithOtherRockets(rocketID, pos))
                return LandingStatus.Clash;
            else
                return LandingStatus.Ok;
        }

        bool ClashesWithOtherRockets(int rocketID, Position pos)
            => _rockets.Values
                .Where(r => r.ID != rocketID)
                .Any(r => r.LastPosition.CloseTo(pos, SafetyMargin));

        void StoreLastPosition(int rocketID, Position pos)
        {
            if (!_rockets.TryGetValue(rocketID, out var info)) {
                info = new Rocket(rocketID);
                _rockets.Add(rocketID, info);
            }

            info.LastPosition = pos;
        }
    }
}
