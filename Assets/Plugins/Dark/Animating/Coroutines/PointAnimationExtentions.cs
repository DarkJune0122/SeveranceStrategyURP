using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dark.Animation.Coroutines
{
    public static class PointAnimationExtentions
    {
        #region Behaviour Extentions
        /// <summary>
        /// Moves animation point to "FadeOut" part of animation, if it's required
        /// </summary>
        /// <param name="bound">Out bounds of animation</param>
        /// <param name="fadeHandler">Action, that will be called when this animation was ended or instanly if PointAnimation is Idle (<seealso cref="PointAnimation.IsIdle"/>)</param>
        public static void FadeOut(this PointAnimation animation, float bound, Action fadeHandler)
        {
            if (animation.IsIdle)
            {
                fadeHandler();
                animation.Start();
                return;
            }

            if (animation.time < animation.Duration - bound)
                animation.time = animation.Duration - bound;

            animation.endLister = fadeHandler;
        }

        /// <summary>
        /// Moves animation point to "FadeOut" part of animation, if it's required.
        /// <para>Consider using <see cref="PointAnimation.Start"/> within <paramref name="fadeHandler"/>'s lambda braces if you want to continue animation after invoking action.</para>
        /// </summary>
        /// <param name="bound">In and Out bounds of animation</param>
        /// <param name="fadeHandler">Action, that will be called when this animation was ended or instanly if PointAnimation is Idle (<seealso cref="PointAnimation.IsIdle"/>)</param>
        public static void FadeBounds(this PointAnimation animation, float bound, Action fadeHandler)
        {
            if (animation.IsIdle)
            {
                fadeHandler();
                return;
            }

            if (animation.time < bound)
            {
                animation.time = animation.Duration - animation.time;
            }
            else if (animation.time < animation.Duration - bound)
            {
                animation.time = animation.Duration - bound;
            }
            animation.endLister = fadeHandler;
        }

        /// <summary>
        /// Keeps animation in active bounds.
        /// </summary>
        /// <param name="bound">In and Out bound of animation.</param>
        public static void KeepInBounds(this PointAnimation animation, float bound)
        {
            if (animation.IsIdle)
            {
                animation.Start();
                return;
            }

            if (animation.time < bound)
                return;

            if (animation.time > animation.Duration - bound)
                animation.time = animation.Duration - animation.time;
            else animation.time = bound;
        }

        /// <inheritdoc cref="KeepInBounds(PointAnimation, float)"/>
        /// <param name="endAction">Setups action, that will be played at the end of animation (See also <seealso cref="PointAnimation.endLister"/>)</param>
        public static void KeepInBounds(this PointAnimation animation, float bound, Action endAction)
        {
            KeepInBounds(animation, bound);
            animation.endLister = endAction;
        }

        #endregion


        #region Component Extentions
        public static Coroutine Apperance(CanvasGroup group, AnimationCurve curve, Action endAction = null)
            => PointCurve.CurveAction((alpha) => { group.alpha = alpha; }, curve, endAction);

        public static Coroutine Apperance(Graphic graphic, AnimationCurve curve, Action endAction = null)
            => PointCurve.CurveAction((alpha) => { graphic.color = graphic.color.Set_A(alpha); }, curve, endAction);
        
        #endregion
    }
}
