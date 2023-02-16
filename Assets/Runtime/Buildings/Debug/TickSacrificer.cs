using SeveranceStrategy.Buildings.Processing;
using UnityEngine;
using UnityEngine.UI;

namespace SeveranceStrategy.Buildings.Debugging
{
    public sealed class TickSacrificer : Processor
    {
        [Header(nameof(TickSacrificer))]
        [SerializeField] private Text m_counterText;


        bool m_isDirty;
        protected override void Process()
        {
            Damage(8);
            m_isDirty = true;
        }

        private void Update()
        {
            if (!m_isDirty)
            {
                return;
            }

            m_counterText.text = m_health.ToString();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_counterText.text = "dead";
        }
    }
}