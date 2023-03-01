using Dark;
using SeveranceStrategy.Core;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SeveranceStrategy.Game.Kinematics
{
    public sealed class CameraManager : MonoBehaviour, IDragHandler
    {
        public static CameraManager Instance => m_instance;
        public static Camera Camera => m_instance.m_camera;

        [SerializeField] private Transform m_movementAnchor;
        [SerializeField] private Transform m_rotationAnchor;
        [SerializeField] private Camera m_camera;
        [SerializeField] private Transform m_pointer;
        [SerializeField] private Vector2 m_rotationSpeed = new(40f, 24f);
        //[SerializeField, Range(0.5f, 1f)] private float m_altitideChangeSpeed = 0.9f;



        private static CameraManager m_instance;
        private bool m_rightMouseState;
        private float m_scrollDelta;
        private CameraManager() => m_instance = this;
        private void Awake() => Settings.FoV.onValueChange += SettingsFoV_OnValueChanged;
        private void OnDestroy()
        {
            Settings.FoV.onValueChange -= SettingsFoV_OnValueChanged;
            if (m_instance == this)
            {
                m_instance = null;
            }
        }

        private void SettingsFoV_OnValueChanged(int fov) => m_camera.fieldOfView = fov;


        // Input handling
        private void Update()
        {
            // Camera Z offset handling and changing.
            m_scrollDelta = Input.mouseScrollDelta.y;
            const float MinOffset = -33f, MaxOffset = -3f;
            if (m_scrollDelta != 0)
            {
                Debug.Log(m_scrollDelta);
                m_camera.transform.localPosition = m_camera.transform.localPosition.Set_Z(Mathf.Clamp(
                    value: m_camera.transform.localPosition.z - m_scrollDelta * 3 * (Settings.InvertZoom.Value ? -1 : 1),
                    min: MinOffset, max: MaxOffset));
            }

            // Key check to avoid overheats.
            if (Input.anyKey == false)
            {
                m_rightMouseState = false;
                return;
            }

            /// Camera rotation (<seealso cref="OnDrag"/> further down):
            m_rightMouseState = Input.GetMouseButton(1);

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
            if (!m_rightMouseState) return;
            Vector3 rotation = m_rotationAnchor.localEulerAngles;
            rotation.x += eventData.delta.y * Time.unscaledDeltaTime * m_rotationSpeed.y * (Settings.InvertY.Value ? -1 : 1);
            rotation.y += eventData.delta.x * Time.unscaledDeltaTime * m_rotationSpeed.x * (Settings.InvertX.Value ? -1 : 1);
            m_rotationAnchor.localEulerAngles = rotation;
        }




#if UNITY_EDITOR
        private void Reset() => TryGetComponent(out m_camera);
#endif
    }
}