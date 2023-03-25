using Dark.Animation.Coroutines;
using SeveranceStrategy.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SeveranceStrategy.Game.Kinematics
{
    public sealed class CameraManager : MonoBehaviour, IDragHandler
    {
        public static CameraManager Instance => m_instance;
        public Camera Camera => m_camera;
        public float Offset => -m_camera.transform.localPosition.z;

        [SerializeField] private Transform m_movementAnchor;
        [SerializeField] private Transform m_rotationAnchor;
        [SerializeField] private Camera m_camera;
        [SerializeField] private Transform m_pointer;
        [SerializeField] private Vector2 m_rotationSpeed = new(40f, 24f);
        [SerializeField] private Vector2 m_slideSpeed = new(1f, 1f);
        //[SerializeField, Range(0.5f, 1f)] private float m_altitideChangeSpeed = 0.9f;
        [Header("Animating")]
        [SerializeField] private AnimationCurve m_zoomValueCurve = AnimationCurve.Linear(0, 2, 10, 30);



        private static CameraManager m_instance;
        private bool m_rotationKeyState;
        private bool m_slideKeyState;
        private float m_zoomTime = 2;
        private CameraManager() => m_instance = this;
        private void Awake() => Settings.FoV.onValueChange += SettingsFoV_OnValueChanged;

        private void OnDestroy()
        {
            Settings.FoV.onValueChange -= SettingsFoV_OnValueChanged;
            if (m_instance == this) m_instance = null;
        }

        // Delegates
        private void SettingsFoV_OnValueChanged(int fov) => m_camera.fieldOfView = fov;


        // Input handling
        private void OnEnable() => m_camera.transform.localPosition = m_camera.transform.localPosition.Set_Z(-m_zoomValueCurve.Evaluate(m_zoomTime));
        private void Update()
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta != 0)
            {
                // Handling inputs.
                m_zoomTime = Mathf.Clamp(m_zoomTime - scrollDelta * (Settings.InvertZoom.Value ? -1 : 1), min: m_zoomValueCurve.FirstKey().value, max: m_zoomValueCurve.DurationUnsafe());
                m_camera.transform.localPosition = m_camera.transform.localPosition.Set_Z(-m_zoomValueCurve.Evaluate(m_zoomTime));
            }

            // Key check to avoid overheat.
            if (Input.anyKey == false)
            {
                m_rotationKeyState = false;
                return;
            }

            /// Camera rotation (<seealso cref="OnDrag"/> further down):
            if (m_slideKeyState = Input.GetKey(Keybinds.CameraSlideKey.Value))
            {
                m_rotationKeyState = false;
            }
            else m_rotationKeyState = Input.GetKey(Keybinds.CameraRotationKey.Value);

            // Cemera movement:
            Vector2 input = new(Inputs.GetAxis(Keybinds.MoveLeft.Value, Keybinds.MoveRight.Value), Inputs.GetAxis(Keybinds.MoveUp.Value, Keybinds.MoveDown.Value));
            if (input != Vector2.zero)
            {
                input = -m_camera.transform.localPosition.z * Time.unscaledDeltaTime * input.RotateAngle(-m_rotationAnchor.localEulerAngles.y);
                Vector3 position = m_movementAnchor.localPosition;
                position.x += input.x;
                position.z += input.y;
                position.y = GetHeight(position);
                m_pointer.position = m_pointer.position.Set_Y(position.y);
                m_movementAnchor.localPosition = position;

                // Returns height in current point on game surface.
                static float GetHeight(Vector3 point) => Map.DefaultY;
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (m_slideKeyState)
            {
                Vector3 slide = Vector3.zero;
                slide += eventData.delta.y * (Settings.InvertYSlide.Value ? -1 : 1) * m_slideSpeed.y * Time.unscaledDeltaTime * m_rotationAnchor.up;
                slide += eventData.delta.x * (Settings.InvertXSlide.Value ? -1 : 1) * m_slideSpeed.x * Time.unscaledDeltaTime * m_rotationAnchor.right;
                m_movementAnchor.Translate(slide);
            }
            else if (m_rotationKeyState)
            {
                Vector3 rotation = m_rotationAnchor.localEulerAngles;
                rotation.x += eventData.delta.y * Time.unscaledDeltaTime * m_rotationSpeed.y * (Settings.InvertYRotation.Value ? -1 : 1);
                rotation.y += eventData.delta.x * Time.unscaledDeltaTime * m_rotationSpeed.x * (Settings.InvertXRotation.Value ? -1 : 1);
                m_rotationAnchor.localEulerAngles = rotation;
            }
        }


#if UNITY_EDITOR
        private void Reset() => TryGetComponent(out m_camera);
#endif
    }
}