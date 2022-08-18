using System.Diagnostics;

namespace Zartis.RocketLanding
{
    [DebuggerDisplay("{ID} @ {LastPosition}")]
    public record class Rocket
    {
        public Rocket(int id) => ID = id;

        public int ID { get; }

        public Position LastPosition { get; set; }
    }
}
