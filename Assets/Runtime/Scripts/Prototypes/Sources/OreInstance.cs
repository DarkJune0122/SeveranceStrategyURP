using SeveranceStrategy.Buildings;
using SeveranceStrategy.Game;

namespace SeveranceStrategy.Prototypes.Sources
{
    public class OreInstance : StaticInstance
    {
        public int amount = 40;

        private void Awake() => GameManager.ores.Add(this);
        private void OnDestroy() => GameManager.ores.Remove(this);


        public int GetAmount(int amount)
        {
            this.amount -= amount;
            if (this.amount > 0) return amount;

            Destroy(gameObject);
            return amount + this.amount;
        }
    }
}