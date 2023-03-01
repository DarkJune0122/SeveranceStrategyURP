using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SeveranceStrategy.IO
{
    public sealed class SaveFile
    {
        public const string Root = "Saves";
        public const string Extention = ".ssav";


        // Serialized fields:
        public string name = nameof(SaveFile);
        /// <summary>
        /// Save file description, provided by user.
        /// </summary>
        public string description;
        /// <summary>
        /// Short recap of current situation/story stage in SaveFile.
        /// </summary>
        public string recap;
        /// <summary>
        /// Path to SaveFile in system.
        /// </summary>
        [NonSerialized] public string path;

        /// <summary>
        /// Size of chunk of map.
        /// <see cref="generatable"/> to undertand, whether Generator should be initialized.
        /// </summary>
        public Vector2Int chunkSize;

        /// <summary>
        /// Map generator seed.
        /// </summary>
        public int seed = 0;
        /// <summary>
        /// Id of location. Using to initialize different generators.
        /// </summary>
        public int generatorID = 0;
        /// <summary>
        /// Whether map can be generated during gameplay or not.
        /// </summary>
        public bool generatable = false;

        public SaveFile(string name) => this.name = name;


        public static SaveFile Load(string path)
        {
            if (!TryLoad(path, out SaveFile file)) throw Json.GetDeserializationException(path);
            return file;
        }
        public static bool TryLoad(string path, out SaveFile saveFile)
        {
            if (Json.TryLoad(path, out saveFile))
            {
                saveFile.path = path;
                return true;
            }
            else return false;
        }

        public static void Save(SaveFile file) => Json.Save(file.path, file);
        public static string GetPath(string nameOrPathExtention) => $"{Json.ClientPath}{Root}/{nameOrPathExtention}{Extention}";

        /// <summary>
        /// Loads ALL map data from disk.
        /// </summary>
        /// <remarks>
        /// To reduce allocations use <![CDATA[using()]]> operator.
        /// </remarks>
        /// <returns>Disposable <see cref="MapData"/>.</returns>
        public MapData LoadData()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("SaveFile in provided path doesn't exist or there's typo in path:\n" + path);
            }

            MapData data = new();
            string[] lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(MapData.EnvironmentKey))
                {
                    data.environment = lines[i][MapData.EnvironmentKey.Length..];
                }
                else if (lines[i].StartsWith(MapData.PatternKey))
                {
                    data.pattern = lines[i][MapData.PatternKey.Length..];
                }
                else if (lines[i].StartsWith(MapData.InstanceKey))
                {
                    data.instances = lines[i][MapData.InstanceKey.Length..];
                }
                else if (lines[i].StartsWith(MapData.StructureKey))
                {
                    data.units = lines[i][MapData.StructureKey.Length..];
                }
                else if (lines[i].StartsWith(MapData.UnitKey))
                {
                    data.structures = lines[i][MapData.UnitKey.Length..];
                }
            }

            return data;
        }

        /// <summary>
        /// Saves SaveFile on disk with provided MapData.
        /// </summary>
        /// <param name="data">Data of your game map.</param>
        public async Task SaveData(MapData data, CancellationToken token = default)
        {
            if (path == null) throw new IOException("SaveFile path invalid or empty.");

            Save(this);
            await File.WriteAllTextAsync(path, JsonUtility.ToJson(this), token);
            await using StreamWriter streamWriter = File.AppendText(path);
            await streamWriter.WriteAsync(System.Environment.NewLine);
            await streamWriter.WriteAsync(MapData.EnvironmentKey);
            await streamWriter.WriteLineAsync(data.environment);
            await streamWriter.WriteAsync(MapData.PatternKey);
            await streamWriter.WriteLineAsync(data.pattern);
            await streamWriter.WriteAsync(MapData.InstanceKey);
            await streamWriter.WriteLineAsync(data.instances);
            await streamWriter.WriteAsync(MapData.StructureKey);
            await streamWriter.WriteLineAsync(data.structures);
            await streamWriter.WriteAsync(MapData.UnitKey);
            await streamWriter.WriteLineAsync(data.units);
        }


        public class MapData : IDisposable
        {
            // Constants queued in loading order.
            public const string EnvironmentKey = nameof(environment) + ":";
            public const string PatternKey = nameof(pattern) + ":";
            public const string InstanceKey = nameof(instances) + ":";
            public const string StructureKey = nameof(structures) + ":";
            public const string UnitKey = nameof(units) + ":";

            public const char SerializedDataPrefix = 's';
            public const char ClassPrefix = 'c';
            public const char NamePrefix = 'n';
            public const char InstancePrefix = 'i';

            public string surfaces;
            public string environment;
            public string pattern;
            public string instances;
            public string structures;
            public string units;


            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects)
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    disposedValue = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            // ~MapData()
            // {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //     Dispose(disposing: false);
            // }

            void IDisposable.Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}