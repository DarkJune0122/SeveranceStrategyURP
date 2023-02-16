using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Authored by DarkJune [August, 2022]
public static partial class Extentions
{
    /// <param name="transform">Transform of your object</param>
    /// <param name="required">Required parent of given transform in hierarchy</param>
    /// <returns>Returns full path (or path from required transform) of gameObject in hierarchy</returns>
    public static string GetTransformPath(this Transform transform, Transform required = null)
    {
        string path = transform.name;
        while (transform.parent != required)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }
    public static void StopCoroutines(this MonoBehaviour mono, params Coroutine[] coroutines)
    {
        foreach (Coroutine _c in coroutines)
        {
            if (_c is null) continue;
            mono.StopCoroutine(_c);
        }
    }
    public static void StopCoroutines(this MonoBehaviour mono, ref Coroutine[] coroutines)
    {
        foreach (Coroutine _c in coroutines)
        {
            if (_c is null) continue;
            mono.StopCoroutine(_c);
        }
        coroutines = null;
    }
    public static void StopCoroutines(this MonoBehaviour mono, ref List<Coroutine> coroutines)
    {
        foreach (Coroutine _c in coroutines)
        {
            if (_c is null) continue;
            mono.StopCoroutine(_c);
        }
        coroutines.Clear();
    }


    public static float GetTextWidth(this TextMesh mesh, string text)
    {
        int width = 0;
        foreach (char symbol in text)
        {
            if (mesh.font.GetCharacterInfo(symbol, out CharacterInfo info))
            {
                width += info.advance;
            }
        }
        if (width < 1) width = 1;
        return width * mesh.characterSize * 0.2f * mesh.transform.lossyScale.x;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMeshWidth(this MeshRenderer textMesh) => textMesh.bounds.extents.x * 4;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RadianToVector2(float radian) => new(Mathf.Cos(radian), Mathf.Sin(radian));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);


    #region Array Interactions
    /// <summary>
    /// Shuffle all items in this array.
    /// </summary>
    public static void Shuffle<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            T temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            (array[i], array[randomIndex]) = (array[randomIndex], temp);
        }
    }
    /// <summary>
    /// Shuffle items from this array in given range.
    /// </summary>
    public static void Shuffle<T>(this T[] array, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            T temp = array[i];
            int randomIndex = Random.Range(i, end);
            (array[i], array[randomIndex]) = (array[randomIndex], temp);
        }
    }
    /// <summary>
    /// Shuffle all items in this list.
    /// </summary>
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], temp);
        }
    }
    /// <summary>
    /// Shuffle items from this list in given range.
    /// </summary>
    public static void Shuffle<T>(this List<T> list, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, end);
            (list[i], list[randomIndex]) = (list[randomIndex], temp);
        }
    }
    #endregion

    #region Sphere colliders collisions
    /// <summary>
    /// Checks collisions between two sphere colliders
    /// </summary>
    /// <param name="this_collider">Your collider</param>
    /// <param name="other_collider">Other collider with what must be checked collision</param>
    /// <returns>True if there is any collision between two given collider</returns>
    public static bool SphereCollisions(this UnityEngine.SphereCollider this_collider, UnityEngine.SphereCollider other_collider)
    {
        return GetCollisionWith(this_collider, other_collider);
    }
    /// <summary>
    /// Checks collisions between your collider and given sphere colliders
    /// </summary>
    /// <param name="this_collider">Your collider</param>
    /// <param name="other_colliders"></param>
    /// <returns></returns>
    public static bool SphereCollisions(this UnityEngine.SphereCollider this_collider, UnityEngine.SphereCollider[] other_colliders)
    {
        foreach (UnityEngine.SphereCollider _collider in other_colliders)
        {
            bool _result = GetCollisionWith(this_collider, _collider);
            if (_result) return _result;
        }
        return false;
    }
    /// <summary>
    /// Checks, are two colliders collide with each other
    /// </summary>
    /// <param name="first_collider">First collider</param>
    /// <param name="second_collider">SecondCollider</param>
    /// <returns>True if collides, false if not</returns>
    private static bool GetCollisionWith(UnityEngine.SphereCollider first_collider, UnityEngine.SphereCollider second_collider)
    {
        float _discance = (second_collider.transform.position - first_collider.transform.position).magnitude;
        _discance -= first_collider.radius;
        _discance -= second_collider.radius;
        return _discance <= 0;
    }
    #endregion

    #region Float Extention
    public static Vector2 V2(this float _this)
    {
        return new Vector2(_this, _this);
    }
    public static Vector2 V2(this float _this, float scale)
    {
        return (_this * scale).V2();
    }
    public static Vector3 V3(this float _this)
    {
        return new Vector3(_this, _this, _this);
    }
    public static Vector3 V3(this float _this, float scale)
    {
        return (_this * scale).V3();
    }

    /// <summary>
    /// Using to create number with avable amount of numbers after dot. Use only for visualize variables!
    /// </summary>
    /// <param name="valueToParce">Your value</param>
    /// <returns>float number with some numbers after dot</returns>
    public static float FloatWithDot(this float valueToParce, int numberAfterDot = 1)
    {
        return Mathf.Round(numberAfterDot * 10 * valueToParce) / (numberAfterDot * 10);
    }
    #endregion

    #region Animation Extentions
    /// <summary>
    /// Returns total time of given animation curve
    /// </summary>
    /// <returns>total time</returns>
    public static float Duration(this AnimationCurve curve)
    {
        return curve[curve.length - 1].time;
    }
    /// <summary>
    /// Returns last keyframe of given curve
    /// </summary>
    /// <returns>Last keyframe in array</returns>
    public static Keyframe LastKey(this AnimationCurve curve)
    {
        return curve[curve.length - 1];
    }
    #endregion

    #region Color Intercations
    public static Color Set_R(this Color _this, float r)
    {
        _this.r = r;
        return _this;
    }
    public static Color Set_G(this Color _this, float g)
    {
        _this.g = g;
        return _this;
    }
    public static Color Set_B(this Color _this, float b)
    {
        _this.b = b;
        return _this;
    }
    public static Color Set_A(this Color _this, float a)
    {
        _this.a = a;
        return _this;
    }
    public static Color Set_RGB(this Color _this, Color other)
    {
        _this.r = other.r;
        _this.g = other.g;
        _this.b = other.b;
        return _this;
    }
    public static Color Set_RGB(this Color _this, float r, float g, float b)
    {
        _this.r = r;
        _this.g = g;
        _this.b = b;
        return _this;
    }

    public static Renderer SetColor(this Renderer render, Color color)
    {
        render.material.color = color;
        return render;
    }
    public static Renderer[] SetColor(this Renderer[] renders, Color color)
    {
        foreach (Renderer render in renders)
        {
            render.material.color = color;
        }
        return renders;
    }
    #endregion

    #region Vector2 Intercations
    public static Vector2 Set_X(this Vector2 _this, float x)
    {
        _this.x = x;
        return _this;
    }
    public static Vector2 Set_Y(this Vector2 _this, float y)
    {
        _this.y = y;
        return _this;
    }
    public static Vector3 Set_Z(this Vector2 _this, float z)
    => new(_this.x, _this.y, z);

    public static Vector2 Round(this Vector2 _this)
    {
        _this.x = Mathf.Round(_this.x);
        _this.y = Mathf.Round(_this.y);
        return _this;
    }
    public static Vector2Int RoundToInt(this Vector2 _this)
        => new(Mathf.RoundToInt(_this.x), Mathf.RoundToInt(_this.y));
    #endregion

    #region Vector3 Intercations
    public static Vector3 Set_X(this Vector3 _this, float x)
    {
        _this.x = x;
        return _this;
    }
    public static Vector3 Set_Y(this Vector3 _this, float y)
    {
        _this.y = y;
        return _this;
    }
    public static Vector3 Set_Z(this Vector3 _this, float z)
    {
        _this.z = z;
        return _this;
    }

    public static Vector3 Round(this Vector3 _this)
    {
        _this.x = Mathf.Round(_this.x);
        _this.y = Mathf.Round(_this.y);
        _this.z = Mathf.Round(_this.z);
        return _this;
    }
    public static Vector3Int RoundToInt(this Vector3 _this)
        => new(Mathf.RoundToInt(_this.x), Mathf.RoundToInt(_this.y), Mathf.RoundToInt(_this.z));
    #endregion

    #region Animation Curve
    public static AnimationCurve StandardCurve(float duration = 1) => new(new Keyframe[] { new Keyframe(0, 0), new Keyframe(duration, 1) });
    public static AnimationCurve StrokeCurve(float duration = 1) => new(new Keyframe[] { new Keyframe(0, 0) { outTangent = 1 / duration }, new Keyframe(duration, 1) { inTangent = 1 / duration } });
    public static AnimationCurve StrokeCurveInvert(float duration = 1) => new(new Keyframe[] { new Keyframe(0, 1) { outTangent = 1 / duration }, new Keyframe(duration, 0) { inTangent = 1 / duration } });
    #endregion

    #region Debugging
    public static string RandomBase64() => Convert.ToBase64String(BitConverter.GetBytes(Random.Range(int.MinValue, int.MaxValue)));
    public static void LogEnumerable<A, B>(this IDictionary<A, B> dictionary, string str = null)
    {
        if (!string.IsNullOrEmpty(str))
            str += "\n";
        foreach (var e in dictionary)
        {
            str += $"Key: {e.Key}  Value: {e.Value}\n";
        }

        Debug.Log(str);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LogEnumerable<A>(this ICollection<A> collection, string str = null)
        => Debug.Log(StringEnumerable(collection, str));
    public static string StringEnumerable<A>(this ICollection<A> collection, string str = null)
    {
        if (!string.IsNullOrEmpty(str))
            str += "\n";

        int i = 0;
        foreach (var e in collection)
        {
            str += $"[{i}]: {e}\n";
            i++;
        }

        return str;
    }
    public static void LogEnumerable<A>(this IEnumerable<A> enumerable, string str = null)
    {
        if (!string.IsNullOrEmpty(str))
            str += "\n";

        int i = 0;
        foreach (var e in enumerable)
        {
            str += $"[{i}]: {e}\n";
            i++;
        }

        Debug.Log(str);
    }
    #endregion

    #region String Interactions
    public static IEnumerable<string> EnumerableSplit(this string str, params char[] characters)
    {
        int l = str.Length;
        int i = 0, j = str.IndexOfAny(characters);
        if (j == -1) // No such substring
        {
            yield return str; // Return original and break
            yield break;
        }

        while (j != -1)
        {
            if (j - i > 0) // Non empty? 
            {
                yield return str[i..j]; // Return non-empty match
            }
            i = j + 1;
            j = str.IndexOfAny(characters, i, l - i);
        }

        if (i < l) // Has remainder?
        {
            yield return str[i..l]; // Return remaining trail
        }
    }
    #endregion
}