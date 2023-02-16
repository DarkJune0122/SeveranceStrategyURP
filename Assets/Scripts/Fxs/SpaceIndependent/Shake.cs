using System.Runtime.CompilerServices;
using UnityEngine;

namespace SeveranceStrategy.FXs.Independent
{
    public class ShakeFx : MonoBehaviour
    {
        [SerializeField] private AnimationCurve m_shakePowerCurve;
        [SerializeField] private float m_shakeResistance = 10f;
        [SerializeField] private float m_shakeIntensity = 0.1f;

        private float shakePowerTime;
        private void Update()
        {
            if (shakePowerTime <= 0)
            {
                transform.localPosition = Vector2.zero;
                enabled = false;
                return;
            }

            shakePowerTime -= Time.deltaTime * m_shakeResistance;

            float powerEvaluate = m_shakePowerCurve.Evaluate(Mathf.Abs(shakePowerTime)) * m_shakeIntensity;
            transform.localPosition = new Vector2(RandomRange(), RandomRange());

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            float RandomRange() => Random.Range(-powerEvaluate, powerEvaluate);
        }

        public void Shake(float power)
        {
            if (power > shakePowerTime)
            {
                shakePowerTime = power;

                enabled = true;
            }
        }
    }
}