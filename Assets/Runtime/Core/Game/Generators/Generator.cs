using SeveranceStrategy.IO;
using SeveranceStrategy.Loading;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static SeveranceStrategy.IO.SaveFile;

namespace SeveranceStrategy.Core
{
    public abstract class Generator : ScriptableObject
    {
        public const string DataSeparator = ";;";
        public const string ParamSeparator = "/";

        private static readonly Dictionary<string, Generator> m_generators = new();

        /// <summary>
        /// Generator name.
        /// </summary>
        public abstract string Name { get; }

        public Generator() => m_generators.Add(Name, this);
        protected virtual async Task Generate(SaveFile file, MapData mapData, CancellationToken token) => await Task.CompletedTask;



        // Static entry:
        /// <summary>
        /// Generates new map with given seed.
        /// </summary>
        /// <param name="file">File with your predefined data.</param>
        /// <param name="generator">Name of used generator.</param>
        public static async Task GenerateMap(SaveFile file, string generator, CancellationToken token = default)
        {
            if (file.generatable == false)
            {
                Debug.LogWarning("Can't generate ungeneratable map. Please, consider using new SaveFile(args) to create new maps.");
                return;
            }
            if (File.Exists(file.path)) Debug.LogWarning("File by provided path already exist and will be overwriten.");

            LoadManager.Progress("Generating begins...", .7f);
            MapData mapData = new();

            // Trying to use custon generator.
            if (m_generators.TryGetValue(generator, out var instance))
            {
                await instance.Generate(file, mapData, token);
            }
            else throw new System.Exception("Given generator doesn't present in hashset.");

            LoadManager.Progress("Saving file to a disk...", .8f);
            await file.SaveData(mapData, token);
            Debug.Log("Map generated successfully!");
        }
    }
}