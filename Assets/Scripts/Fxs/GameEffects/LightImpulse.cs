using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightImpulse : MonoBehaviour
{
    [SerializeField] private Light2D[] m_lightSources;
    [SerializeField] private SpriteRenderer[] m_lightSprites;
    [SerializeField] private AnimationCurve m_lightRangeCurve;
    [SerializeField] private Gradient m_gradient;
    [SerializeField] private float m_timeScale = 1f;

    float time;
    private void Update()
    {
        time += Time.deltaTime * m_timeScale;
        if (time > m_lightRangeCurve[m_lightRangeCurve.length - 1].time)
        {
            time -= m_lightRangeCurve[m_lightRangeCurve.length - 1].time;
        }


        float rangeEvaluate = m_lightRangeCurve.Evaluate(time);
        Color colorEvaluate = m_gradient.Evaluate(time);
        for (int i = 0; i < m_lightSources.Length; i++)
        {
            m_lightSources[i].pointLightOuterRadius = rangeEvaluate;
            m_lightSources[i].color = colorEvaluate;
            m_lightSprites[i].color = colorEvaluate;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        float rangeEvaluate = m_lightRangeCurve.Evaluate(time);
        Color colorEvaluate = m_gradient.Evaluate(0);
        for (int i = 0; i < m_lightSources.Length; i++)
        {
            m_lightSources[i].pointLightOuterRadius = rangeEvaluate;
            m_lightSources[i].color = colorEvaluate;
            m_lightSprites[i].color = colorEvaluate;
        }
    }
#endif
}
