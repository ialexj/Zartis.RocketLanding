namespace Zartis.RocketLanding.Services
{
    // Exposing the service on a network accessible by inbound orbital-class rockets is left to another department.
    // Probably wouldn't use HTTP though.

    /// <summary>
    /// Provides access to landing pad management.
    /// </summary>
    public interface ILandingService
    {
        /// <summary>
        /// Informs that the rocket is targetting a given position to land, and requests information about whether it's a valid landing site.
        /// </summary>
        /// <remarks>Rockets tend to come down whether you allow them to or not.</remarks>
        /// <param name="rocketID">The identifier assigned to the rocket.</param>
        /// <param name="position">The position at which the rocket wants to land.</param>
        /// <returns>The spec demands that this should be a string. So either 'ok for landing', 'out of platform', or 'clash'.</returns>
        string RequestLanding(int rocketID, Position position);
    }
}
