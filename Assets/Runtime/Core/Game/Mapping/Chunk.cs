using SeveranceStrategy.Buildings;
using System;

namespace SeveranceStrategy.Core
{
    public sealed class Chunk
    {
        public readonly StaticInstance[,] environment;
        public readonly DynamicInstance[,] game;

        public Chunk(int xSize, int ySize)
        {
            environment = new StaticInstance[xSize, ySize];
            game = new DynamicInstance[xSize, ySize];
        }

        public string Serialize() => throw new NotImplementedException();
        public void Deserialize(string str) => throw new NotImplementedException(); 
    }
}
