using System;
using UnityEngine;

namespace SeveranceStrategy.Core
{
    public static class Environment
    {
        public const string TerrainLayer = "Terrain";


        // Modding IO
        public const string PrototypePrefix = "Prototypes/";

        // Naninovel Support
        public const char VectorValuesSeparator = '\'';


        // Exceptions
        public const string AwaitsForSupport = "This thing still in development";
        public static readonly NotImplementedException AddressableAssetsSupport = new("Addressable assets still not implemented");


        public static string AddString(this string @this, string str)
        {
            return (string.IsNullOrEmpty(@this) || @this.StartsWith(System.Environment.NewLine) ? @this + System.Environment.NewLine : null) + str;
        }
        public static Vector3 ParseToVector3(string str)
        {
            string[] values = str.Split(VectorValuesSeparator);
            if (values.Length != 3)
            {
                throw new Exception($"Provided string have more values than it's possible to handle");
            }

            Vector3 position = new();
            if (!float.TryParse(values[0], out position.x)) throw new Exception($"Unable to parse given string into {nameof(Vector3)} X value: {values[0]}");
            if (!float.TryParse(values[1], out position.y)) throw new Exception($"Unable to parse given string into {nameof(Vector3)} Y value: {values[1]}");
            if (!float.TryParse(values[2], out position.z)) throw new Exception($"Unable to parse given string into {nameof(Vector3)} Z value: {values[2]}");
            return position;
        }
    }
}
