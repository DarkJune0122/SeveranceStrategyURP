using UnityEngine;

namespace SeveranceStrategy.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public abstract void Show();
        public abstract void Hide();
    }
}