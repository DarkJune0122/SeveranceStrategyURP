using UnityEngine;

namespace SeveranceStrategy.Protoinfo.Sources
{
    [CreateAssetMenu(menuName = "ProtoInfo/Source/" + nameof(OreInfo))]
    public class OreInfo : BlockInfo
    {
        public string type;
        public int amount;
    }
}
