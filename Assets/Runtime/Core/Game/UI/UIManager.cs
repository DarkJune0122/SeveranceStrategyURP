using UnityEngine;

namespace SeveranceStrategy.UI
{
    public sealed class UIManager : MonoBehaviour
    {
        public const string Bool_KinematicMode = "kinematic";
        public static UIManager Instance => m_instance;
        public static bool KinematicMode
        {
            get => m_instance.m_animator.GetBool(Bool_KinematicMode);
            set => m_instance.m_animator.SetBool(Bool_KinematicMode, value);
        }


        [Tooltip("Reference to an main UI animator.")]
        [SerializeField] private Animator m_animator;


        private static UIManager m_instance;
        private void Awake() => m_instance = this;
    }
}