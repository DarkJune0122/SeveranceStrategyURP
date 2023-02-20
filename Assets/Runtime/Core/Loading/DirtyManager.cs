using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Runtime.Core.Loading
{
    public sealed class DirtyManager : MonoBehaviour
    {
        private static readonly List<Action> m_actions = new();
        private static Coroutine m_dirtyDelay;
        private static DirtyManager m_instance;
        private void Awake() => m_instance = this;
        private static IEnumerator Dirty()
        {
            yield return null;
            m_actions.ForEach(action => action());
            m_actions.Clear();
        }

        /// <summary>
        /// Adds new <paramref name="action"/> to be executed at the end of frame.
        /// </summary>
        /// <param name="action"></param>
        public static void Add(Action action)
        {
            m_actions.Add(action);
            m_dirtyDelay ??= m_instance.StartCoroutine(Dirty());
        }

        /// <summary>
        /// Removing <paramref name="action"/> from executing list.
        /// <para>Note: your action can be already executed and deleted </para>
        /// </summary>
        /// <param name="action">Your action.</param>
        public static void Remove(Action action)
        {
            if (m_actions.Remove(action))
            {
                m_dirtyDelay ??= m_instance.StartCoroutine(Dirty());
            }
        }
    }
}
