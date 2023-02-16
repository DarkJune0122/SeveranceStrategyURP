namespace SeveranceStrategy.Buildings
{
    public sealed class Receipt
    {
        public Stack[] required;
        public Stack output;
        public float time;
    }
    public struct Stack
    {
        public string item;
        public float amount;
        public bool isFluid;
    }
}
