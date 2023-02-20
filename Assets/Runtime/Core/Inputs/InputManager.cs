using SeveranceStrategy.Inputs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SeveranceStrategy
{
    public static class InputManager
    {
        public static int GetAxis(KeyCode positive, KeyCode negative) => Input.GetKey(positive) ? Input.GetKey(negative) ? 0 : 1 : Input.GetKey(negative) ? -1 : 0;






        [Obsolete("Most likely, will not be supported further.")]
        public static Dictionary<KeyCode, Func<bool>[]> m_entries = new();

        [Obsolete("Most likely, will not be supported further.")]
        public static void Add(InputEntry entry)
        {
            if (m_entries.TryGetValue(entry.key, out Func<bool>[] actions))
            {
                Array.Resize(ref actions, actions.Length + 1);
                m_entries[entry.key] = actions;
            }
            else m_entries.Add(entry.key, new Func<bool>[] { entry.func });
        }
        [Obsolete("Most likely, will not be supported further.")]
        public static void Remove(InputEntry entry)
        {
            if (m_entries.TryGetValue(entry.key, out Func<bool>[] actions))
            {
                if (actions.Length > 1)
                {
                    Miscellaneous.RemoveClass(ref actions, entry.func);
                    m_entries[entry.key] = actions;
                }
                else m_entries.Remove(entry.key);

                Array.Resize(ref actions, actions.Length + 1);
                m_entries[entry.key] = actions;
            }
            else m_entries.Add(entry.key, new Func<bool>[] { entry.func });

            throw new Exception("All keybinds for alias {alias} is already deleted.");
        }

        [Obsolete("Most likely, will not be supported further.")]
        private static void KeyDown(KeyCode key)
        {
            if (!m_entries.TryGetValue(key, out Func<bool>[] entries))
                return;

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Invoke()) return;
            }
        }



        [Obsolete("Most likely, will not be supported further.")]
        private sealed class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
        {
            public bool Active
            {
                get => enabled;
                set => SetActiveState(value);
            }


            void IPointerDownHandler.OnPointerDown(PointerEventData ignored) => KeyDown(KeyCode.Mouse0);
            void IPointerUpHandler.OnPointerUp(PointerEventData ignored) { }

            private void SetActiveState(bool state)
            {
                enabled = state;
                // Should do something with clicked buttons.
            }

            private void Update()
            {
                Event @event = Event.current;
                if (@event == null || !@event.isKey) return;

                KeyDown(@event.keyCode);
            }
        }
    }

    public enum InputType
    {
        Undefined = 0,
        KeyDown = 1,
        KeyUp = 2,
    }
}
