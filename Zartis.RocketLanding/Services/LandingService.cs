using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zartis.RocketLanding.Services
{
    // TODO: inject with an IOC container
    public class LandingService : ILandingService
    {
        readonly LandingCoordinator _coordinator;

        public LandingService(LandingCoordinator coordinator)
        {
            _coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        }

        public string RequestLanding(int rocketID, Position position)
        {
            return _coordinator.RequestLanding(rocketID, position).AsString();
        }
    }
}
