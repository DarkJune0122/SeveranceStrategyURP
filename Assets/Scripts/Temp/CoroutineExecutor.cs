using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helps in easy execution lambda expretions trough coroutines
/// </summary>
public class CoroutineExecutor : MonoBehaviour
{
    private static CoroutineExecutor fix;
    public static CoroutineExecutor Fix
    {
        get
        {
            if (fix) return fix;
            GameObject _obj = new("Coroutine Executor");
            return fix = _obj.AddComponent<CoroutineExecutor>();
        }
    }

    #region Coroutines


    /// <summary>
    /// Null-safe method that safely stops all given coroutines that was called during CoroutineExecutor.Execute or other methods
    /// <para>Take a note that Animate.Methods() also using CoroutineExecutor for animations</para>
    /// </summary>
    public static void StopGivenCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
            Fix.StopCoroutine(coroutine);
    }

    /// <summary>
    /// Null-safe method that safely stops all given coroutines that was called during CoroutineExecutor.Execute or other methods
    /// <para>Take a note that Animate.Methods() also using CoroutineExecutor for animations</para>
    /// </summary>
    public static void StopGivenCoroutine(ref List<Coroutine> coroutines)
    {
        Fix.StopCoroutines(ref coroutines);
    }
    /// <summary>
    /// Not null-save analogue of <see cref="StopGivenCoroutine(ref Coroutine)"/>
    /// </summary>
    /// <param name="coroutine"></param>
    public static void StopGivenCoroutineUnsave(ref Coroutine coroutine)
    {
        Fix.StopCoroutine(coroutine);
    }
    public static void StopGivenCoroutine(ref Coroutine[] coroutines)
    {
        Fix.StopCoroutines(ref coroutines);
    }

    public enum ExecuteMode
    {
        PerFrameMode = 0,
        InRealTime = 1,
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="lateAction">Action that will be called after some delay.</param>
    /// <param name="repeats">Amount of repeats.</param>
    /// <param name="delay">DElay between calls.</param>
    public static Coroutine Repeat(System.Action lateAction, float repeats, float delay, ExecuteMode mode = ExecuteMode.PerFrameMode)
    {
        return Fix.StartCoroutine(Fix.RepeatLocal(lateAction, repeats, delay, mode));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="lateAction">Action that will be called after some delay.</param>
    /// <param name="repeats">Amount of repeats.</param>
    /// <param name="delay">DElay between calls.</param>
    public static Coroutine Repeat(System.Action lateAction, float repeats, float delay, System.Action lastAction, ExecuteMode mode = ExecuteMode.PerFrameMode)
    {
        return Fix.StartCoroutine(Fix.RepeatLocal(lateAction, repeats, delay, lastAction, mode));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="lateAction">Action that will be called after some delay.</param>
    /// <param name="repeats">Amount of repeats.</param>
    /// <param name="delay">DElay between calls.</param>
    public static Coroutine FixedRepeat(System.Action lateAction, float repeats, float yieldRepeats = 1)
    {
        return Fix.StartCoroutine(Fix.FixedRepeatLocal(lateAction, repeats, yieldRepeats));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="lateAction">Action that will be called after some delay.</param>
    /// <param name="delay">Duration of coroutine.</param>
    public static Coroutine Delay(System.Action lateAction, float delay)
    {
        return Fix.StartCoroutine(Fix.LateExecuteLocal(lateAction, delay));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="lateAction">Action that will be called after some delay.</param>
    /// <param name="value">Duration of coroutine (float) or amount of passed frames when WaitType <see cref="ExecuteWaitType.WaitForEndOfFrame"/></param>
    public static void Delay(System.Action lateAction, ExecuteWaitType _waitType, object value = null)
    {
        Fix.StartCoroutine(Fix.LateExecuteLocal(lateAction, _waitType, value));
    }
    public enum ExecuteWaitType
    {
        WaitForEndOfFrame = 0,
        WaitForScaledRealTime = 1,
        WaitForUnscaledRealTime = 2,
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="action">Your action.</param>
    /// <param name="delay">Delay between actions.</param>
    /// <param name="duration">Duration of coroutine.</param>
    /// <param name="endAction">This action will be called in end of coroutine</param>
    public static Coroutine Execute(System.Action action, float delay, float duration, System.Action endAction, ExecuteMode relativeTo = ExecuteMode.PerFrameMode)
    {
        return Fix.StartCoroutine(Fix.ExecuteLocal(action, delay, duration, endAction, relativeTo));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="action">Your action.</param>
    /// <param name="delay">Delay between actions.</param>
    /// <param name="duration">Duration of coroutine.</param>
    /// <param name="endAction">This action will be called in end of coroutine</param>
    public static Coroutine Execute(System.Action<float> action, AnimationCurve curve, System.Action<float> endAction, ExecuteMode relativeTo = ExecuteMode.PerFrameMode, float delay = 0.02f)
    {
        return Fix.StartCoroutine(Fix.ExecuteLocal(action, curve, endAction, relativeTo, delay));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="action">Your action.</param>
    /// <param name="delay">Delay between actions.</param>
    /// <param name="duration">Duration of coroutine.</param>
    /// <param name="endAction">This action will be called in end of coroutine</param>
    public static Coroutine Execute(System.Action<float> action, AnimationCurve curve, ExecuteMode relativeTo = ExecuteMode.PerFrameMode, float delay = 0.02f)
    {
        return Fix.StartCoroutine(Fix.ExecuteLocal(action, curve, relativeTo, delay));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="action">Your action.</param>
    /// <param name="delay">Delay between actions.</param>
    /// <param name="duration">Duration of coroutine.</param>
    public static Coroutine Execute(System.Action action, float delay, float duration, ExecuteMode relativeTo = ExecuteMode.PerFrameMode)
    {
        return Fix.StartCoroutine(Fix.ExecuteLocal(action, delay, duration, relativeTo));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="action">Your action.</param>
    /// <param name="delay">Delay between actions.</param>
    /// <param name="duration">Duration of coroutine.</param>
    public static Coroutine Execute(System.Action<float> action, float delay, float duration, System.Action endAction, ExecuteMode relativeTo = ExecuteMode.PerFrameMode)
    {
        return Fix.StartCoroutine(Fix.ExecuteLocal(action, delay, duration, endAction, relativeTo));
    }
    /// <summary>
    /// Execute given action every time after given delay for given duration.
    /// </summary>
    /// <param name="action">Your action. Float input - is total time of coroutine</param>
    /// <param name="delay">Delay between actions.</param>
    /// <param name="duration">Duration of coroutine.</param>
    public static Coroutine Execute(System.Action<float> action, float delay, float duration, ExecuteMode relativeTo = ExecuteMode.PerFrameMode)
    {
        return Fix.StartCoroutine(Fix.ExecuteLocal(action, delay, duration, relativeTo));
    }


    #region Local Executes


    // Local executors
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator LateExecuteLocal(System.Action lateAction, float duration)
    {
        yield return new WaitForSeconds(duration);
        lateAction.Invoke();
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator LateExecuteLocal(System.Action lateAction, ExecuteWaitType _waitType, object value)
    {
        if (value is null)
        {
            switch (_waitType)
            {
                case ExecuteWaitType.WaitForEndOfFrame:
                    yield return new WaitForEndOfFrame();
                    lateAction.Invoke();
                    break;
            }
        }
        else
        {
            switch (_waitType)
            {
                case ExecuteWaitType.WaitForEndOfFrame:
                    int i = 0;
                    while (i < (int)value)
                    {
                        yield return new WaitForEndOfFrame();
                        i++;
                    }
                    lateAction.Invoke();
                    break;
                case ExecuteWaitType.WaitForScaledRealTime:
                    yield return new WaitForSeconds((float)value);
                    lateAction.Invoke();
                    break;
                case ExecuteWaitType.WaitForUnscaledRealTime:
                    yield return new WaitForSecondsRealtime((float)value);
                    lateAction.Invoke();
                    break;
            }
        }
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator FixedRepeatLocal(System.Action action, float repeats, float yieldRepeats)
    {
        action.Invoke();
        int times = 1;
        while (times < repeats)
        {
            for (int i = 0; i < yieldRepeats; i++)
                yield return new WaitForFixedUpdate();

            action.Invoke();
            times++;
        }
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator RepeatLocal(System.Action action, float repeats, float delay, ExecuteMode relativeTo)
    {
        action.Invoke();
        float time = 0f;
        int times = 1;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (times < repeats)
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                    if (time > delay)
                    {
                        action.Invoke();
                        time %= delay;
                        times++;
                    }
                }
                break;
            case ExecuteMode.InRealTime:
                while (times < repeats)
                {
                    yield return new WaitForSecondsRealtime(delay);
                    action.Invoke();
                    times++;
                }
                break;
        }
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator RepeatLocal(System.Action action, float repeats, float delay, System.Action lastAction, ExecuteMode relativeTo)
    {
        action.Invoke();
        float time = 0f;
        int times = 1;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (times < repeats)
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                    if (time > delay)
                    {
                        action.Invoke();
                        time %= delay;
                        times++;
                    }
                }
                break;
            case ExecuteMode.InRealTime:
                while (times < repeats)
                {
                    yield return new WaitForSecondsRealtime(delay);
                    action.Invoke();
                    times++;
                }
                break;
        }
        lastAction.Invoke();
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator ExecuteLocal(System.Action action, float delay, float duration, System.Action endAction, ExecuteMode relativeTo)
    {
        float time = 0f;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (time < duration)
                {
                    action.Invoke();
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                break;
            case ExecuteMode.InRealTime:
                while (time < duration)
                {
                    action.Invoke();
                    yield return new WaitForSecondsRealtime(delay);
                    time += delay;
                }
                break;
        }
        endAction.Invoke();
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator ExecuteLocal(System.Action<float> action, AnimationCurve curve, System.Action<float> endAction, ExecuteMode relativeTo, float delay)
    {
        float time = 0f;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (time < curve[curve.length - 1].time)
                {
                    action.Invoke(curve.Evaluate(time));
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                break;
            case ExecuteMode.InRealTime:
                while (time < curve[curve.length - 1].time)
                {
                    action.Invoke(curve.Evaluate(time));
                    yield return new WaitForSecondsRealtime(delay);
                    time += delay;
                }
                break;
        }
        endAction.Invoke(curve[curve.length - 1].value);
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator ExecuteLocal(System.Action<float> action, AnimationCurve curve, ExecuteMode relativeTo, float delay)
    {
        float time = 0f;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (time < curve[curve.length - 1].time)
                {
                    action.Invoke(curve.Evaluate(time));
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                break;
            case ExecuteMode.InRealTime:
                while (time < curve[curve.length - 1].time)
                {
                    action.Invoke(curve.Evaluate(time));
                    yield return new WaitForSecondsRealtime(delay);
                    time += delay;
                }
                break;
        }
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator ExecuteLocal(System.Action<float> action, float delay, float duration, System.Action endAction, ExecuteMode relativeTo)
    {
        float time = 0f;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (time < duration)
                {
                    action.Invoke(Mathf.Clamp(time, 0f, duration));
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                break;
            case ExecuteMode.InRealTime:
                while (time < duration)
                {
                    action.Invoke(Mathf.Clamp(time, 0f, duration));
                    yield return new WaitForSecondsRealtime(delay);
                    time += delay;
                }
                break;
        }
        endAction.Invoke();
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator ExecuteLocal(System.Action action, float delay, float duration, ExecuteMode relativeTo)
    {
        float time = 0f;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (time < duration)
                {
                    action.Invoke();
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                break;
            case ExecuteMode.InRealTime:
                while (time < duration)
                {
                    action.Invoke();
                    yield return new WaitForSecondsRealtime(delay);
                    time += delay;
                }
                break;
        }
    }
    /// <summary>
    /// Execute coroutine by using real instance of script. Same inputs as have his parent.
    /// </summary>
    private IEnumerator ExecuteLocal(System.Action<float> action, float delay, float duration, ExecuteMode relativeTo)
    {
        float time = 0f;
        switch (relativeTo)
        {
            case ExecuteMode.PerFrameMode:
                while (time < duration)
                {
                    action.Invoke(Mathf.Clamp(time, 0f, duration));
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                break;
            case ExecuteMode.InRealTime:
                while (time < duration)
                {
                    action.Invoke(Mathf.Clamp(time, 0f, duration));
                    yield return new WaitForSecondsRealtime(delay);
                    time += delay;
                }
                break;
        }
        action.Invoke(duration);
    }

    #endregion
    #endregion
}
