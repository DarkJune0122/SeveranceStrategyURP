using SeveranceStrategy.TickManagement;
using UnityEngine;

namespace SeveranceStrategy.Buildings.Processing
{
    public class Processor : HealthObject
    {
        [Header(nameof(Processor))]
        [SerializeField] protected string[] m_receipts;


        protected Receipt m_receipt = new()
        {
            time = 2f,
        };
        protected float m_time;
        public override void Tick()
        {
            m_time += TickManager.TimeDelta;

            if (m_time < m_receipt.time) return;
            m_time %= m_receipt.time;

            Process();
        }

        protected virtual void Process()
        {
            Debug.Log($"Crafted {(m_receipt.output.isFluid ? "fluid" : "item")} {m_receipt.output.item} with amount of {m_receipt.output.amount}");
        }

        public void SetReceipt(Receipt receipt)
        {
            m_receipt = receipt;
            enabled = receipt != null;
        }
    }
}