using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Contains meth functions, that are simplier that thing from <see cref="UnityEngine.Mathf"/> (but can be worse)
/// </summary>
public static class SoftMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DivideToUpper(int x, int y) => (x + y - 1) / y;



    #region Angle Compartion
    public static bool AngleDiffLarger(float current, float target, float maxDiff)
    {
        current = Mathf.DeltaAngle(current, target);
        return current > maxDiff || current < -maxDiff;
    }
    public static bool AngleDiffLess(float current, float target, float maxDiff)
    {
        current = Mathf.DeltaAngle(current, target);
        return maxDiff > current && current > -maxDiff;
    }
    #endregion

    #region Approximately
    public const float ApproximatelyConst = 0.002f;

    /// <summary>
    /// Analogue of == operator for float compartion
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(float a, float b)
        => a - ApproximatelyConst < b && b < a + ApproximatelyConst;

    /// <summary>
    /// Analogue of == operator for float compartion
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(double a, double b)
        => a - ApproximatelyConst < b && b < a + ApproximatelyConst;

    /// <summary>
    /// Analogue of != operator for float compartion
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InverceApproximately(float a, float b)
        => a + ApproximatelyConst < b && b < a - ApproximatelyConst;

    /// <summary>
    /// Analogue of != operator for float compartion
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InverceApproximately(double a, double b)
        => a + ApproximatelyConst < b && b < a - ApproximatelyConst;
    #endregion
}