using Dark.Animation.Coroutines;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using Image = UnityEngine.UI.Image;

namespace SeveranceStrategy.Loading
{
    /// <summary>
    /// Controls visuals of loading... something.
    /// </summary>
    public sealed class LoadManager : MonoBehaviour
    {
        public static Slider ProgressBar => m_instance.m_progressBar;


        // Instance fields
        [Tooltip("CanvasGroup, that controls ALL LoadManager's UI elements")]
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private Image m_background;
        [SerializeField] private Text m_processName;
        [SerializeField] private Slider m_progressBar;
        [Tooltip("Curve time bounds: [0:Inf] Value bounds: [0:1]")]
        [SerializeField] private PointCurve m_alphaAnimation;


        private static LoadManager m_instance;
        private static float m_timeScale;
        private void Awake()
        {
            m_instance = this;

            // Animation initialization
            m_alphaAnimation.action = alpha => m_canvasGroup.alpha = alpha;
        }





        // Statics:
        /// <summary>
        /// Stops game execution until <see cref="ResumeGame"/> will be called.
        /// <para>Force load doesn't suppose to use game fade, consider using <see cref="Transition(Action)"/>
        /// in ally with <see cref="StopGame"/> to hide something you're loading.</para>
        /// </summary>
        public static void StopGame()
        {
            m_timeScale = Time.deltaTime;
            Time.timeScale = 0;
        }

        /// <summary>
        /// Resuming game.
        /// </summary>
        public static void ResumeGame() => Time.timeScale = m_timeScale;



        // Loading Visualization:
        /// <summary>
        /// Shows all UI elements, relevant for loading.
        /// </summary>
        /// <param name="endAction">Animation completion callback.</param>
        /// <param name="useBackground">Whether animation should show background.</param>
        public static void StartLoading(Action endAction, bool useBackground, bool useProgressBar, string process = null)
        {
            m_instance.m_alphaAnimation.endLister = endAction;
            StartLoading(useBackground, useProgressBar, process);
        }
        /// <inheritdoc cref="StartLoading(Action, bool)"/>
        public static void StartLoading(bool useBackground, bool useProgressBar, string process = null)
        {
            Internal_StartLoading(useBackground, useProgressBar, process);
            m_instance.m_alphaAnimation.Start();
        }
        public static void Instant_StartLoading(bool useBackground, bool useProgressBar, string process = null)
        {
            Internal_StartLoading(useBackground, useProgressBar, process);
            m_instance.m_canvasGroup.alpha = 1;
        }
        private static void Internal_StartLoading(bool useBackground, bool useProgressBar, string process = null)
        {
            m_instance.m_background.gameObject.SetActive(useBackground);
            m_instance.m_progressBar.gameObject.SetActive(useProgressBar);
            m_instance.m_canvasGroup.gameObject.SetActive(true);
            m_instance.m_processName.text = process;
            if (useProgressBar) m_instance.m_progressBar.value = 0;
        }

        /// <summary>
        /// Hides relevant loading UI elenemts.
        /// </summary>
        /// <param name="endAction">Animation completion callback.</param>
        public static void StopLoading(Action endAction)
        {
            StopLoading();
            m_instance.m_alphaAnimation.endLister += endAction;
        }
        /// <inheritdoc cref="StopLoading(Action)"/>
        public static void StopLoading() => m_instance.m_alphaAnimation.StartReverse(
            endAction: () => m_instance.m_canvasGroup.gameObject.SetActive(false));

        public static void Instant_StopLoading() => m_instance.m_canvasGroup.gameObject.SetActive(false);


        /// <summary>
        /// Starting show & hide animation sequence for relative Loading UI elements,
        /// fires an <paramref name="inTransition"/> event when game fully hidden beneath game fade.
        /// </summary>
        /// <param name="inTransition">Callback, that will be fired when Load UI will be fully shown.</param>
        public static void Transition(Action inTransition)
        {
            if (Settings.UIAnimations.Value)
            {
                Instant_StartLoading(useBackground: true, useProgressBar: false);
                inTransition();
                Instant_StopLoading();
                return;
            }

            StartLoading(inTransition + StopLoading, useBackground: true, useProgressBar: false);
        }

        public static void Progress(string process, float progress)
        {
            m_instance.m_processName.text = process;
            m_instance.m_progressBar.value = progress;
        }
    }
}