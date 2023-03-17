using System;

namespace SeveranceStrategy.Core
{
    public static class Environment
    {
        public const string PrototypePrefix = "Prototypes/";

        public const string TerrainLayer = "Terrain";

        // Exceptions
        public const string AwaitsForSupport = "This thing still in development";
        public static readonly NotImplementedException AddressableAssetsSupport = new("Addressable assets still not implemented");
    }
}
