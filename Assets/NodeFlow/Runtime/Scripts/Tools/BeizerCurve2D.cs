using System.Collections.Generic;
using UnityEngine;

namespace Dark.NodeFlow.Tools
{
    public static class BeizerCurve2D
    {
        public enum Curvings : byte
        {
            Cubic = 0,
            Direct = 1,
            Linear = 2,
        }

        public static Vector2[] DirectCurve(Vector2 source, Vector2 control1, Vector2 control2, Vector2 end, float step, float turningZone)
        {
#if DARK_SAVEEX
            if (step <= 0)
                throw new System.Exception("Curve step cannot be less or be equal to zero!");
#endif
            Vector2 endDelta = end - control2;
            Vector2 controlDelta = control2 - control1;
            Vector2 sourceDelta = control1 - source;
            float turningRate = 1 / turningZone * 0.5f;
            float endZone = 1 - turningZone;

            List<Vector2> points = new()
            {
                // Evaluate for time: 0
                source
            };

            // Iterates over all steps and evaluate curve points.
            for (float time = step; time < 1; time += step)
            {
                float scaledTime = time * turningRate;
                if (time < turningZone)
                {
                    points.Add(QuadraticPoint(source + sourceDelta * scaledTime, control1, control1 + controlDelta * scaledTime, scaledTime));
                }
                else if (time > endZone)
                {
                    points.Add(QuadraticPoint(control1 + controlDelta * scaledTime, control2, end + endDelta * scaledTime, scaledTime));
                }
                else
                {
                    points.Add(control1 + scaledTime * controlDelta);
                }
            }

            // Evaluate for time: 1
            points.Add(end);

            return points.ToArray();
        }



        /// <summary>
        /// Evaluates points on cubic curve between two points with two control points given.
        /// </summary>
        /// <remarks>
        /// Regardless of <paramref name="step"/> value, 0 and 1 points are allways evaluates.
        /// </remarks>
        /// <param name="source">Source of curve.</param>
        /// <param name="controlDelta1">First control point.</param>
        /// <param name="controlDelta2">Control point delta, relative to <paramref name="end"/>.</param>
        /// <param name="end">End of curve.</param>
        /// <returns>Array of points from curve.</returns>
        public static Vector2[] CubicCurve(Vector2 source, Vector2 control1, Vector2 control2, Vector2 end, float step)
        {
#if DARK_SAVEEX
            if (step <= 0)
                throw new System.Exception("Curve step cannot be less or be equal to zero!");
#endif
            /*Vector2 endDelta = end - control2;
            Vector2 controlDelta = control2 - control1;
            Vector2 sourceDelta = control1 - source;
            */
            List<Vector2> points = new()
            {
                // Evaluate for time: 0
                source
            };

            // Iterates over all steps and evaluate curve points.
            for (float time = step; time < 1; time += step)
            {
                points.Add(DirectCubicPoint(source, control1, control2, end, time));
                /*
                Vector2 quaSource = source + sourceDelta * time;
                Vector2 quaControl = control1 + controlDelta * time;
                Vector2 quaEnd = control2 + endDelta * time;

                points.Add(QuadraticPoint(quaSource, quaControl, quaEnd, time));*/
            }

            // Evaluate for time: 1
            points.Add(end);

            return points.ToArray();
        }

        /// <inheritdoc cref="CubicCurve(Vector2, Vector2, Vector2, Vector2, float)"/>
        public static Vector2[] CubicCurve(Transform source, Transform control1, Transform control2, Transform end, float step)
            => CubicCurve(source.position, control1.position - source.position, end.position, control2.position - end.position, step);

        /// <inheritdoc cref="CubicCurve(Vector2, Vector2, Vector2, Vector2, float)"/>
        public static Vector2[] CubicCurveLocal(Transform source, Transform control1, Transform control2, Transform end, float step)
            => CubicCurve(source.localPosition, control1.localPosition - source.localPosition, end.localPosition, control2.localPosition - end.localPosition, step);

        /// <summary>
        /// Evaluates position of point on curve in given position of time, using default interpolations.
        /// </summary>
        /// <remarks>
        /// Consider using <see cref="CubicCurve(Vector2, Vector2, Vector2, Vector2, float)"/> if you're trying to draw just curve.
        /// </remarks>
        /// <returns>Position on cubic curve.</returns>
        public static Vector2 CubicPoint(Vector2 source, Vector2 control1, Vector2 control2, Vector2 end, float time)
            => QuadraticPoint(source: source + (control1 - source) * time,
                              control: control1 + (control2 - control1) * time,
                              end: control2 + (end - control2) * time,
                              time: time);


        /// <summary>
        /// Evaluates position on curve in given time, using direct formula.
        /// </summary>
        /// <returns>Position on cubic curve.</returns>
        public static Vector2 DirectCubicPoint(Vector2 source, Vector2 control1, Vector2 control2, Vector2 end, float time)
        {
            float time2 = time * time;
            float time3 = time2 * time;
            float scaledTime = time * 3;
            time2 *= 3;

            return source * (-time3 + time2 - scaledTime + 1) + control1 * (3 * time3 - 2 * time2 + scaledTime) + control2 * (-3 * time3 + time2) + end * time3;
        }



        /// <summary>
        /// Evaluates points on quadratic curve.
        /// </summary>
        /// <remarks>
        /// See also: <seealso cref="CubicCurve(Vector2, Vector2, Vector2, Vector2, float)"/>
        /// </remarks>
        /// <param name="source">Source of curve.</param>
        /// <param name="control">Control point of curve.</param>
        /// <param name="end">End of curve.</param>
        public static Vector2[] QuadraticCurve(Vector2 source, Vector2 control, Vector2 end, float step)
        {
#if DARK_SAVEEX
            if (step <= 0)
                throw new System.Exception("Curve step cannot be less or be equal to zero!");
#endif
            Vector2 sourceDelta = control - source;
            Vector2 controlDelta = end - control;

            List<Vector2> points = new()
            {
                // Evaluate for time: 0
                source
            };

            // Iterates over all steps and evaluate curve points.
            for (float time = 0; time < 1; time += step)
            {
                points.Add(QuadraticPoint(source, sourceDelta, control, controlDelta, time));
            }

            // Evaluate for time: 1
            points.Add(end);

            return points.ToArray();
        }

        /// <summary>
        /// Evaluates point on quadratic curve.
        /// </summary>
        /// <param name="source">Curve source.</param>
        /// <param name="control">Control point of curve.</param>
        /// <param name="end">End point of curve.</param>
        /// <param name="time">Evaluating time.</param>
        /// <returns>Point on given curve in given time.</returns>
        public static Vector2 QuadraticPoint(Vector2 source, Vector2 control, Vector2 end, float time)
        {
            Vector2 sourceDelta = control - source;
            Vector2 controlDelta = end - control;

            return QuadraticPoint(source, sourceDelta, control, controlDelta, time);
        }

        /// <summary>
        /// Evaluates position of point on quadratic curve, using given delta values.
        /// </summary>
        /// <param name="source">Curve source.</param>
        /// <param name="control">Control point of curve.</param>
        /// <param name="sourceDelta">Delta between control and source point.</param>
        /// <param name="controlDelta">Delta between end and control point.</param>
        public static Vector2 QuadraticPoint(Vector2 source, Vector2 sourceDelta, Vector2 control, Vector2 controlDelta, float time)
        {
            Vector2 sourcePoint = source + sourceDelta * time;
            return sourcePoint + (control + controlDelta * time - sourcePoint) * time;
        }
    }
}
