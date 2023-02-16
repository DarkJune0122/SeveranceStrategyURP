using UnityEditor;
using UnityEngine;

namespace Dark.NodeFlow.Configuration
{
    public static class NodeLibrary
    {
        // Folders
        public static string AssetFolder = nameof(NodeFlow);
        public static string ConfigurationsFolder = "Configurations";


        public static T LoadConfiguration<T>() where T : ScriptableObject
        {
            // Loads configuration file from resources.
            string path = $"{ConfigurationsFolder}/{typeof(T).Name}";
            T config = Resources.Load<T>(path);

#if UNITY_EDITOR
            // Creates new file in Asset Database with default values if it's required.
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(config, $"Assets/{AssetFolder}/Resources/{ConfigurationsFolder}/{typeof(T).Name}.asset");
            }

            // returns total processed configuration class.
            return config;
#else
            return config ??= ScriptableObject.CreateInstance<T>();
#endif
        }
    }
}
