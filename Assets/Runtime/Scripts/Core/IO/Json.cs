using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SeveranceStrategy.IO
{
    public static class Json
    {
        public static IOException GetSerializationException(string path) => new("Unable to serialize file!\n" + path);
        public static IOException GetDeserializationException(string path) => new("Unable to deserialize file!\n" + path);
        public static string ClientPath => $"{Application.dataPath}/Client/";


        public static T Load<T>(string path) where T : class
        {
            if (!TryLoad(path, out T instance)) throw GetDeserializationException(path);
            return instance;
        }

        public static T LoadOrDefault<T>(string path) where T : class => TryLoad(path, out T instance) ? instance : default;

        public static bool TryLoad<T>(string path, out T instance) where T : class
        {
            if (!File.Exists(path))
            {
                instance = null;
                return false;
            }

            using StreamReader reader = File.OpenText(path);
            instance = JsonUtility.FromJson<T>(reader.ReadLine());
            return true;
        }

        public static void Save<T>(string path, T instance) where T : class => File.WriteAllText(path, JsonUtility.ToJson(instance, true));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckOverwrite(string path) => File.Exists(path);
    }
}
