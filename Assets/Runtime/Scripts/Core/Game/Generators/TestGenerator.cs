using SeveranceStrategy.Buildings;
using SeveranceStrategy.Core;
using SeveranceStrategy.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SeveranceStrategy.Game.Generators
{
    [CreateAssetMenu(menuName = "Generators/" + nameof(TestGenerator))]
    public sealed class TestGenerator : Generator
    {
        public override string Name => nameof(TestGenerator);


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize() => CreateInstance<TestGenerator>();


        protected override async Task Generate(SaveFile file, SaveFile.MapData mapData, CancellationToken token)
        {
            Debug.Log("Generator initializing - " + nameof(TestGenerator));
            await Task.CompletedTask;

            int xLen = file.chunkSize.x;
            int yLen = file.chunkSize.y;
            //uint instanceId = 0; // instace not using in creating environment, as it would never interact with each other.

            StringBuilder builder = new();
            builder.Append("c:" + nameof(StaticInstance));
            mapData.environment = builder.ToString();
        }
    }
}
