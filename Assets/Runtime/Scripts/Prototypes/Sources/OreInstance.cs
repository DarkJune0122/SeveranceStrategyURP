using SeveranceStrategy.Game;
using SeveranceStrategy.Protoinfo;
using SeveranceStrategy.Protoinfo.Sources;
using UnityEngine;

namespace SeveranceStrategy.Prototypes.Sources
{
    public class OreInstance : BlockIntance
    {
        [SerializeField] protected int amount = 40;


        protected OreInfo info;
        private void Awake() => GameManager.ores.Add(this);
        private void OnDestroy() => GameManager.ores.Remove(this);
        public override void Setup(BlockInfo info)
        {
            base.Setup(info);
            this.info = (OreInfo)info;
            amount = this.info.amount;
        }

        public int Extract(int amount)
        {
            this.amount -= amount;
            if (this.amount > 0) return amount;

            Destroy(gameObject);
            return amount + this.amount;
        }
    }
}