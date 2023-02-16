using SeveranceStrategy.Scripts.Game;
using SeveranceStrategy.FXs.Independent;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SeveranceStrategy.User
{
    public sealed class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        public ShakeFx ShakeFx => m_shake;


        [SerializeField] private Camera m_camera;
        [SerializeField] private FOVController m_fovController;
        [SerializeField] private ShakeFx m_shake;
        [SerializeField] private float m_moveSpeed = 1f;
        [SerializeField] private float m_velocityRezistance = 0.6f;
        [Space(4f)]
        [SerializeField] private Transform m_leftBottomMapCorner;
        [SerializeField] private Transform m_rightTopMapCorner;

        public bool IsKinematic = false;
        public Vector3 Velocity => m_velocity;


        private const float VelocityMinimumValue = 0.0001f;
        private Vector3 m_velocity;
        private void Awake() => Instance = this;
        private void Update()
        {
            if (IsKinematic) return;

            PerformCameraMovement();

            // Performs camera movements and process user inputs
            void PerformCameraMovement()
            {
                // Зумит камеру если игрок двигает колёсиком мыши
                if (Input.mouseScrollDelta.y != 0)
                {
                    m_fovController.SetFOV(m_fovController.TargetFOV - Input.mouseScrollDelta.y);
                }

                Vector2 input = HandleInput();
                if (input == Vector2.zero)
                {
                    // Если ускорение нулевое - прерывает скрипт без каких либо действий
                    if (m_velocity.x == 0f && m_velocity.y == 0f) return;

                    m_velocity -= m_velocity * (m_velocityRezistance * Time.deltaTime);
                    if (Mathf.Abs(m_velocity.x) < VelocityMinimumValue && Mathf.Abs(m_velocity.y) < VelocityMinimumValue)
                        m_velocity = Vector2.zero;
                }
                else
                {
                    // Определяет скорость изменения позиции относительно текущей зоны видимости
                    float localMovingSpeed = m_camera.orthographicSize * m_moveSpeed * Time.deltaTime;
                    if (Input.GetKey(KeyCode.LeftControl))
                        localMovingSpeed *= 0.2f;
                    m_velocity = input * localMovingSpeed;
                }

                // Двигает камеру
                transform.localPosition = ChangeCameraPosition();
            }

            // Возвращает позицию камеры внутри игровой коробки в текущий момент времени
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            Vector3 ChangeCameraPosition()
            {
                return new Vector3(
                    x: transform.localPosition.x + Velocity.x,
                    y: transform.localPosition.y + Velocity.y,
                    z: transform.localPosition.z);
            }
            /*Vector3 ChangeCameraPosition_Clamp()
            {
                return new Vector3(
                    x: Mathf.Clamp(m_camera.transform.localPosition.x + Velocity.x, m_leftBottomMapCorner.localPosition.x, m_rightTopMapCorner.localPosition.x),
                    y: Mathf.Clamp(m_camera.transform.localPosition.y + Velocity.y, m_leftBottomMapCorner.localPosition.y, m_rightTopMapCorner.localPosition.y),
                    z: m_camera.transform.localPosition.z);
            }*/
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 HandleInput()
            => new(x: (Input.GetKey(Keybinds.Global.MoveLeft) ? -1 : 0) + (Input.GetKey(Keybinds.Global.MoveRight) ? 1 : 0),
                   y: (Input.GetKey(Keybinds.Global.MoveDown) ? -1 : 0) + (Input.GetKey(Keybinds.Global.MoveUp) ? 1 : 0));


#if UNITY_EDITOR
        private void Reset()
        {
            if (TryGetComponent(out FOVController controller))
            {
                m_fovController = controller;
            }
            else m_fovController = gameObject.AddComponent<FOVController>();
            m_fovController.Camera = m_camera;
        }
        private void OnDrawGizmosSelected()
        {
            if (m_rightTopMapCorner == null || m_leftBottomMapCorner == null || m_leftBottomMapCorner.parent == null)
                return;

            // Рисует зону, в которой камера будет двигатся
            Gizmos.color = new(0.4f, 0.4f, 1f, 0.4f);
            Gizmos.matrix = Matrix4x4.TRS(m_leftBottomMapCorner.parent.position, m_leftBottomMapCorner.parent.rotation, m_leftBottomMapCorner.parent.lossyScale);
            Gizmos.DrawCube(center: m_rightTopMapCorner.localPosition + m_leftBottomMapCorner.localPosition,
                              size: m_rightTopMapCorner.localPosition - m_leftBottomMapCorner.localPosition);
        }
#endif
    }
}