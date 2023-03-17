using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Dark.Animation.Coroutines
{
    /// <summary>
    /// Provides flexible operator to control animations just from your code.
    /// Uses <see cref="Coroutine"/>s for animating.
    /// </summary>
    [Serializable]
    public abstract class PointAnimation
    {
        public const float RealtimeTypeDelay = 1 / 60; // Delay for 60 frame

        public bool IsIdle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => coroutine is null;
        }
        public abstract float Duration { get; }

        /// <summary>
        /// Current time of animation.
        /// </summary>
        [NonSerialized] public float time;

        /// <summary>
        /// Speed of animation.
        /// </summary>
        /// <remarks>
        /// [Note] Negative values can not be supported for some type of animations.
        /// </remarks>
        [NonSerialized] public float speed = 1f;

        /// <summary>
        /// Animation executer
        /// </summary>
        public MonoBehaviour executer;

        /// <summary>
        /// Used animation coroutine.
        /// It's null if animation isn't active.
        /// See also <seealso cref="IsIdle"/>
        /// </summary>
        public Coroutine coroutine;

        /// <summary>
        /// Action, that called at the end of animation.
        /// </summary>
        public Action endLister;

        public PointAnimation() => executer = GetExecuter();
        public PointAnimation(MonoBehaviour executer)
        {
            this.executer = executer;
        }

        public virtual void Start()
        {
            if (coroutine != null)
                executer.StopCoroutine(coroutine);

            if (speed < 0)
                speed = -speed;

            time = 0;
        }

        /// <summary>
        /// Starts animation with negative speed.
        /// </summary>
        public virtual void StartReverse()
        {
            if (coroutine != null)
                executer.StopCoroutine(coroutine);

            if (speed > 0)
                speed = -speed;

            time = Duration;
        }

        /// <summary>
        /// Starts animation with negative speed.
        /// </summary>
        public void StartReverse(Action endAction)
        {
            StartReverse();
            endLister = endAction;
        }

        public void Stop(bool invokeEndListener = false)
        {
            executer.StopCoroutine(coroutine);
            coroutine = null;

            if (invokeEndListener)
            {
                endLister?.Invoke();
                endLister = null;
            }
        }

        protected void PerformAnimationEnd()
        {
            /// <see cref="coroutine"/> must be setted to null before calling <see cref="endLister"/>. 
            /// Othervise listener can possibly call <see cref="Start"/> action,
            /// <see cref="coroutine"/> will be overriden with new value and right after that - will be resetted to null.
            /// This will cause that animation will be in Idle state (<seealso cref="IsIdle"/>) DURING actual animation process.
            coroutine = null;

            endLister?.Invoke();
            endLister = null;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MonoBehaviour GetExecuter() => PointAnimationExecuter.Instance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(Coroutine coroutine) => GetExecuter().StopCoroutine(coroutine);
    }

    /// <inheritdoc cref="PointAnimation"/>
    [Serializable]
    public sealed class PointLerp : PointAnimation
    {
        public override float Duration => duration;

        /// <summary>
        /// Action, that are repeateosly executed.
        /// </summary>
        public Action action;

        /// <summary>
        /// Duration of current animation.
        /// </summary>
        public float duration;

        public PointLerp() : base() { }
        public PointLerp(MonoBehaviour executer) : base(executer) { }


        public void Start(Action action, float duration)
        {
            this.action = action;
            this.duration = duration;
            Start();
        }
        public void Start(float duration)
        {
            this.duration = duration;
            Start();
        }
        public override void Start()
        {
            base.Start();
            coroutine = executer.StartCoroutine(LinearEnumerator());
        }

        public override void StartReverse()
        {
            base.StartReverse();
            coroutine = executer.StartCoroutine(LinearReverseEnumerator());
        }


        IEnumerator LinearEnumerator()
        {
            do
            {
                yield return new WaitForEndOfFrame();
                time += Time.unscaledDeltaTime * speed;

                action();
            }
            while (time < duration);
            PerformAnimationEnd();
        }
        IEnumerator LinearReverseEnumerator()
        {
            do
            {
                yield return new WaitForEndOfFrame();
                time += Time.unscaledDeltaTime * speed;

                action();
            }
            while (time > 0);
            PerformAnimationEnd();
        }



        // Static animations
        public static Coroutine LerpAction(Action<float> action, float duration, Action endAction = null)
            => LerpAction(action, duration, GetExecuter(), endAction);
        public static Coroutine LerpAction(Action<float> action, float duration, MonoBehaviour executer, Action endAction = null)
        {
            return executer.StartCoroutine(LocalLerpCoroutine());
            IEnumerator LocalLerpCoroutine()
            {
                float time = 0;
                do
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.unscaledDeltaTime;

                    action(time);
                }
                while (time < duration);
                endAction?.Invoke();
            }
        }
        public static Coroutine Delay(Action endAction, float duration) => Delay(endAction, duration, GetExecuter());
        public static Coroutine Delay(Action endAction, float duration, MonoBehaviour executer)
        {
            return executer.StartCoroutine(LocalDelayCoroutine());
            IEnumerator LocalDelayCoroutine()
            {
                float time = 0;
                do
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.unscaledDeltaTime;
                }
                while (time < duration);
                endAction();
            }
        }
    }

    /// <inheritdoc cref="PointAnimation"/>
    [Serializable]
    public class PointCurve : PointAnimation
    {
        /// <inheritdoc cref="PointLerp.duration"/>
        public override float Duration
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => curve[curve.length - 1].time;
        }

        /// <inheritdoc cref="PointLerp.action"/>
        public Action<float> action;

        /// <summary>
        /// Curve, that used in animating.
        /// </summary>
        public AnimationCurve curve;

        public PointCurve() : base() { }
        public PointCurve(MonoBehaviour executer) : base(executer) { }


        public void Start(Action<float> action, AnimationCurve curve)
        {
            this.action = action;
            this.curve = curve;
            Start();
        }
        public void Start(AnimationCurve curve)
        {
            this.curve = curve;
            Start();
        }
        public override void Start()
        {
            base.Start();
#if DARK_SAVEEX
            if (action is null)
                throw new ArgumentNullException($"[{nameof(PointCurve)}] >> {nameof(action)}. It's makes no sence. Use other delay functions for this");
            if (curve is null)
                throw new ArgumentNullException($"[{nameof(PointCurve)}] >> {nameof(curve)}.");
#endif
            coroutine = executer.StartCoroutine(AnimationEnumerator());
        }

        public override void StartReverse()
        {
            base.StartReverse();
            coroutine = executer.StartCoroutine(AnimationReverseEnumerator());
        }


        IEnumerator AnimationEnumerator()
        {
            do
            {
                yield return new WaitForEndOfFrame();
                time += Time.unscaledDeltaTime * speed;

                action(curve.Evaluate(time));
            }
            while (time < Duration);
            PerformAnimationEnd();
        }
        IEnumerator AnimationReverseEnumerator()
        {
            do
            {
                yield return new WaitForEndOfFrame();
                time += Time.unscaledDeltaTime * speed;

                action(curve.Evaluate(time));
            }
            while (time > 0);
            PerformAnimationEnd();
        }


        // Static animations
        public static Coroutine CurveAction(Action<float> action, AnimationCurve curve, Action endAction = null)
            => CurveAction(action, curve, GetExecuter(), endAction);
        public static Coroutine CurveAction(Action<float> action, AnimationCurve curve, MonoBehaviour executer, Action endAction = null)
        {
            return executer.StartCoroutine(LocalCurveAction());
            IEnumerator LocalCurveAction()
            {
                float duration = curve[curve.length - 1].time;
                float time = 0;
                do
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.unscaledDeltaTime;

                    action(curve.Evaluate(time));
                }
                while (time < duration);
                endAction?.Invoke();
            }
        }
    }
}
