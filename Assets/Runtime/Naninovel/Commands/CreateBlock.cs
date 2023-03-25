using Naninovel;
using SeveranceStrategy.Protoinfo;
using SeveranceStrategy.Prototypes;
using UnityEngine;

namespace SeveranceStrategy.Naninovel.Commands
{
    [CommandAlias("createBlock")]
    public sealed class CreateBlock : Command
    {
        [ParameterAlias(NamelessParameterAlias), RequiredParameter]
        public StringParameter name;

        [ParameterAlias(nameof(parent))]
        public StringParameter parent;

        [ParameterAlias(nameof(position))]
        public StringParameter position;

        [ParameterAlias(nameof(rotation))]
        public StringParameter rotation;


        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            BlockInfo info = BlockInfo.GetInfo<BlockInfo>(name);
            Transform parent = this.parent.HasValue ? GameObject.Find(this.parent.Value).transform : null;

            if (position.HasValue || rotation.HasValue) info.CreatePrototype<BlockIntance>(
                position: Core.Environment.ParseToVector3(position),
                rotation: Quaternion.Euler(Core.Environment.ParseToVector3(rotation)),
                parent);
            else info.CreatePrototype<BlockIntance>(parent);

            return UniTask.CompletedTask;
        }
    }
}
