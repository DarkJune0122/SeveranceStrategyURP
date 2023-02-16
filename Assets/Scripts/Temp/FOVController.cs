using UnityEngine;

namespace SeveranceStrategy.Scripts.Game
{
    /// <summary>
    /// Управляет зоной видимости камеры. отключается когда анимация заканчивается.
    /// </summary>
    public class FOVController : MonoBehaviour
    {
        [SerializeField] protected Camera m_camera;
        [SerializeField] protected float m_minimalFOVValue = 2f;
        [SerializeField] protected float m_maximalFOVValue = 16f;
        [Tooltip("Значение анимационной кривой должно быть в пределах [0:1]")]
        [SerializeField] protected AnimationCurve m_zoomCurve = AnimationCurve.Linear(0, 0, 0.3f, 1f);

        public Camera Camera { get => m_camera; set => m_camera = value; }
        public float TargetFOV { get; private set; }


        float time;
        float initialFOV, deltaFOV;
        private void Awake()
        {
            initialFOV = TargetFOV = m_camera.orthographicSize;
        }
        /// <summary>
        /// Определяет целевое значение зоны видимости камеры, запускает анимацию её изменения.
        /// </summary>
        /// <param name="requiredFOV">Необходимое значение зоны видимости</param>
        public void SetFOV(float requiredFOV)
        {
            requiredFOV = Mathf.Clamp(requiredFOV, m_minimalFOVValue, m_maximalFOVValue);
            if (requiredFOV == TargetFOV) return;
            time = 0; enabled = true;

            TargetFOV = requiredFOV;
            initialFOV = m_camera.orthographicSize;
            deltaFOV = requiredFOV - initialFOV;
        }
        private void Update()
        {
            time += Time.deltaTime;

            if (time > m_zoomCurve[m_zoomCurve.length - 1].time)
            {
                enabled = false;
                m_camera.orthographicSize = initialFOV + deltaFOV;
            }
            else m_camera.orthographicSize = initialFOV + deltaFOV * m_zoomCurve.Evaluate(time);
        }
    }
}