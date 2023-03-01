using SeveranceStrategy.Buildings;
using SeveranceStrategy.IO;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SeveranceStrategy.Core
{
    public sealed class Map
    {
        public const float DefaultY = 0;

        /// <summary>
        /// Currently playing map.
        /// </summary>
        public static Map current;
        /// <summary>
        /// Current map's tilemap
        /// </summary>
        public Tilemap tilemap;
        /// <summary>
        /// Associated with <see cref="current"/> map, save file.
        /// </summary>
        public static SaveFile associatedFile;


        // Serialized fields:
        public string name;

        public readonly Chunk chunk;
        public Map(int initialXSize, int initialYSize)
        {
           // m_generateChunks = generateChunks;

#if DEBUG || UNITY_EDITOR
            // chunkSizeX = Mathf.Clamp(chunkSizeX, MinimalChunkSize, int.MaxValue);
            // chunkSizeY = Mathf.Clamp(chunkSizeY, MinimalChunkSize, int.MaxValue);
#else
            throw new Exception("Chunk system is not implemented! You cannot use one-chunk map outside of DEBUG build!");
#endif

            chunk = new Chunk(initialXSize, initialYSize);
        }

        /*public StaticInstance Raycast(Vector2 position)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);
            return chunk.game[x, y] == null ? chunk.environment[x, y] == null ? null : chunk.environment[x, y] : chunk.game[x, y];
        }

        /// <remarks>
        /// You SHOULD transform position using <see cref="Tranform"/> method before using this one!
        /// </remarks>
        /// <param name="position">Transformed building position.</param>
        /// <param name="size">Building size.</param>
        /// <returns>True if placement is valid.</returns>
        public bool ValidatePosition(Vector2 position, Vector2Int size)
            => ForEachCell(position, size.x, size.y, (x, y) => chunk.environment[x, y] == null);

        /// <inheritdoc cref=" ValidatePosition(Vector2, Vector2Int)"/>
        /// <param name="pattern">Building pattern. Use this if your building not purely solid.</param>
        public bool ValidatePosition(Vector2 position, bool[,] pattern)
            => ForEachCell(position, pattern.GetLength(0), pattern.GetLength(1), (x, y) => pattern[x, y] && chunk.environment[x, y] == null && chunk.game[x, y] == null);

        private bool ForEachCell(Vector2 position, int xSize, int ySize, Func<int, int, bool> func)
        {
            int x = Mathf.RoundToInt(position.x - xSize * 0.5f);
            int y = Mathf.RoundToInt(position.y - ySize * 0.5f);
            xSize += x; // From that point on, sizes using as Length in compartions.
            ySize += y;

            for (; x < xSize; x++)
            {
                for (; x < ySize; x++)
                {
                    if (func(x, y)) continue;
                    return false;
                }
            }
            return true;
        }*/


        /// <summary>
        /// Transforms <paramref name="position"/> to map grid space.
        /// </summary>
        /// <param name="position">Real object position</param>
        /// <param name="size"></param>
        /// <returns>More persice position on map grid.s</returns>
        public Vector2 Tranform(Vector3 position, Vector2Int size)
        {
            position.x -= (size.x - 1) % 2;
            position.y -= (size.y - 1) % 2;
            return new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        }


        // Methods for saving and loading game file.
        public SaveFile Serialize() => throw new NotImplementedException();
        public static Map Load() => throw new NotImplementedException();

        public sealed class Context
        {
            public Context() { }
        }
    }
}
