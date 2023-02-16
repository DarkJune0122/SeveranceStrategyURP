using System.Collections.Generic;

namespace SeveranceStrategy.Blocks
{
    public class Team
    {
        public readonly List<DestroyableObject.DestroyableInstance> Units = new();
        public ushort TeamID;

        public Team(ushort teamID) => TeamID = teamID;
    }
}
