using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Authored by DarkJune [August, 2022]  Extentions > Miscellaneous
public static partial class Miscellaneous
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
        for (int i = 0; i < coroutines.Length; i++)
        {
            if (coroutines[i] is null) continue;
            mono.StopCoroutine(coroutines[i]);
        }
    }
    public static void StopCoroutines(this MonoBehaviour mono, ref Coroutine[] coroutines)
    {
        for (int i = 0; i < coroutines.Length; i++)
        {
            if (coroutines[i] is null) continue;
            mono.StopCoroutine(coroutines[i]);
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
    public static void StartCoroutine(this MonoBehaviour mono, ref Coroutine coroutine, IEnumerator enumerator)
    {
        if (coroutine != null)
            mono.StopCoroutine(coroutine);

        coroutine = mono.StartCoroutine(enumerator);
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
    public static void Shuffle<T>(this T[] array) => Shuffle(array, 0, array.Length);
    /// <summary>
    /// Shuffle items from this array in given range.
    /// </summary>
    public static void Shuffle<T>(this T[] array, int start, int end)
    {
        for (; start < end; start++)
        {
            int randomIndex = Random.Range(start, end);
            (array[start], array[randomIndex]) = (array[randomIndex], array[start]);
        }
    }
    /// <summary>
    /// Shuffle all items in this list.
    /// </summary>
    public static void Shuffle<T>(this List<T> list) => Shuffle(list, 0, list.Count);
    /// <summary>
    /// Shuffle items from this list in given range.
    /// </summary>
    public static void Shuffle<T>(this List<T> list, int start, int end)
    {
        for (; start < end; start++)
        {
            int randomIndex = Random.Range(start, end);
            (list[start], list[randomIndex]) = (list[randomIndex], list[start]);
        }
    }

    /// <summary>
    /// Trying to get value from <paramref name="array"/> in given <paramref name="index"/>.
    /// </summary
    /// <typeparam name="T">Type of items in your array.</typeparam>
    /// <param name="array">Your beautiful array.</param>
    /// <param name="index">Your persize index.</param>
    /// <returns><typeparamref name="T"/> value.</returns>
    public static T Get<T>(this T[] array, int index) => array[index < array.Length ? index : (array.Length - 1)];

    /// <inheritdoc cref="Get{T}(T[], int)"/>
    /// <param name="def">Default <paramref name="index"/> in case when index is out of bounds.</param>
    public static T Get<T>(this T[] array, int index, int def) => array[index < array.Length ? index : def];

    public static void Copy<T>(T[] array1, T[] array2, int length, int start = 0)
    {
        for(; start < length; start++)
        {
            array2[start] = array1[start];
        }
    }
    public static void Copy<T>(T[] array1, T[] array2) => Copy(array1, array2, array1.Length > array2.Length ? array2.Length : array1.Length);

    /// <summary>
    /// Moves item from given index (<paramref name="from"/>) to target index (<paramref name="to"/>)
    /// </summary>
    public static void Move<T>(this T[] array, int from, int to)
    {
        T item = array[from];
        if (from < to)
        {
            for (; from < to; from++)
            {
                array[from] = array[from + 1];
            }
        }
        else if (from > to)
        {
            for (; from > to; from--)
            {
                array[from] = array[from - 1];
            }
        }
        array[to] = item;
    }

    public static bool HasValue<T>(this T[] array, T value) where T : class
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value)
                return true;
        }
        return false;
    }

    public static void Add<T>(ref T[] array, T item)
    {
        Array.Resize(ref array, array.Length + 1);
        array[^1] = item;
    }

    public static bool Predict<T>(this T[] array, Predicate<T> predicate)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (predicate(array[i]))
                return true;
        }
        return false;
    }

    public static bool PredictLast<T>(this T[] array, Predicate<T> predicate)
    {
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (predicate(array[i]))
                return true;
        }
        return false;
    }

    public static T Find<T>(this T[] array, Predicate<T> predicate)
    {
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (predicate(array[i]))
                return array[i];
        }

        throw new Exception("Required item doesn't present in array.");
    }

    /// <summary>
    /// Removes item from array in specified index
    /// </summary>
    /// <remarks>
    /// Method allocates new array and fills it with items from old array, avoiding given index.
    /// </remarks>
    /// <param name="array">Your array. Reference will be replaced with new one.</param>
    /// <param name="index">Index of item, that will be removed.</param>
    public static void RemoveAt<T>(ref T[] array, int index)
    {
        // Allocates new array.
        T[] newArray = new T[array.Length];

        // Iterating over left and right sides in array relative to given index
        int i = 0;
        for (; i < index; i++)
        {
            newArray[i] = array[i];
        }
        for (; i < newArray.Length; i++)
        {
            newArray[i] = array[i + 1];
        }

        // Changes reference to required one.
        array = newArray;
    }

    /// <summary>
    /// Removes item from array.
    /// </summary>
    /// <remarks>
    /// Using <seealso cref="RemoveAt{T}(ref T[], int)"/> for removing item from specific point of array.
    /// </remarks>
    /// <param name="array"> <inheritdoc cref="RemoveAt{T}(ref T[], int)"/> </param>
    /// <param name="item">Item that will be removed.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveEquatible<T>(ref T[] array, T item) where T : IEquatable<T> => RemoveAt(ref array, IndexOfEquatible(array, item));

    /// <inheritdoc cref="RemoveEquatible{T}(ref T[], T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveClass<T>(ref T[] array, T item) where T : class => RemoveAt(ref array, IndexOfClass(array, item));

    /// <summary>
    /// Iterates over <paramref name="array"/> to get index of required <paramref name="item"/>.
    /// </summary>
    /// <param name="array">Your fancy array.</param>
    /// <param name="item">Target item.</param>
    public static int IndexOfEquatible<T>(T[] array, T item) where T : IEquatable<T>
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(item))
            {
                return i;
            }
        }

        return -1;
    }

    /// <inheritdoc cref="IndexOfEquatible{T}(T[], T)"/>
    public static int IndexOfClass<T>(T[] array, T item) where T : class
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == item)
            {
                return i;
            }
        }

        return -1;
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

    #region Animation Extentions
    /// <summary>
    /// Returns total time of given animation curve
    /// </summary>
    /// <returns>total time</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Duration(this AnimationCurve curve)
    {
        return curve[curve.length - 1].time;
    }
    /// <summary>
    /// Returns last keyframe of given curve
    /// </summary>
    /// <returns>Last keyframe in array</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    public static float LerpUnclamped(float a, float b, float t)
    {
        t = Mathf.Clamp01(t);
        return a * (1 - t) + b * t;
    }

    public static float Lerp(float a, float b, float t) => a * (1 - t) + b * t;
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
    public static Vector2 RotateAngle(this Vector2 vector, float angle) => Rotate(vector, Mathf.Deg2Rad * angle);
    public static Vector2 Rotate(this Vector2 vector, float rad)
    {
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        float x = vector.x;
        vector.x = x * cos - vector.y * sin;
        vector.y = x * sin + vector.y * cos;
        return vector;
    }
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

    #region Graphic interations
    public static void Set_Red(this Graphic graphic, float r)
    {
        graphic.color = graphic.color.Set_R(r);
    }
    public static void Set_Red(this Graphic[] graphics, float r)
    {
        foreach (Graphic graphic in graphics)
            graphic.color = graphic.color.Set_R(r);
    }
    public static void Set_Green(this Graphic graphic, float g)
    {
        graphic.color = graphic.color.Set_G(g);
    }
    public static void Set_Green(this Graphic[] graphics, float g)
    {
        foreach (Graphic graphic in graphics)
            graphic.color = graphic.color.Set_G(g);
    }
    public static void Set_Blue(this Graphic graphic, float b)
    {
        graphic.color = graphic.color.Set_B(b);
    }
    public static void Set_Blue(this Graphic[] graphics, float b)
    {
        foreach (Graphic graphic in graphics)
            graphic.color = graphic.color.Set_B(b);
    }
    public static void Set_Alpha(this Graphic graphic, float a)
    {
        graphic.color = graphic.color.Set_A(a);
    }
    public static void Set_Alpha(this Graphic[] graphics, float a)
    {
        foreach (Graphic graphic in graphics)
            graphic.color = graphic.color.Set_A(a);
    }
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
        if (collection == null)
            return "[Collection is null] " + str;

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
        if (enumerable == null)
        {
            Debug.Log("[Enumerable is null] " + str);
            return;
        }

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
    /*public static IEnumerable<string[]> EnumerableSplit(this string str, int amount, params char[] characters)
    {
        string[] split = new string[amount];
        int splitLength = 0;
        int len = str.Length;
        int index = str.IndexOfAny(characters);
        if (index == -1) // No such substring
        {
            yield return str; // Return original and break
            yield break;
        }

        int i = 0;
        while (index != -1)
        {
            if (index - i > 0) // Non empty? 
            {
                yield return str[i..index]; // Return non-empty match
            }
            i = index + 1;
            index = str.IndexOfAny(characters, i, len - i);
        }

        if (i < len) // Has remainder?
        {
            yield return str[i..len]; // Return remaining trail
        }
    }*/
    public static IEnumerable<string> EnumerableSplit(this string str, params char[] characters)
    {
        int len = str.Length;
        int i = 0, index = str.IndexOfAny(characters);
        if (index == -1) // No such substring
        {
            yield return str; // Return original and break
            yield break;
        }

        while (index != -1)
        {
            if (index - i > 0) // Non empty? 
            {
                yield return str[i..index]; // Return non-empty match
            }
            i = index + 1;
            index = str.IndexOfAny(characters, i, len - i);
        }

        if (i < len) // Has remainder?
        {
            yield return str[i..len]; // Return remaining trail
        }
    }
    public static IEnumerable<string> EnumerableSplit(this string str, string separator)
    {
        int len = str.Length;
        int i = 0, index = str.IndexOf(separator);
        if (index == -1) // No such substring
        {
            yield return str; // Return original and break
            yield break;
        }

        do
        {
            if (index - i > 0) // Non empty? 
            {
                yield return str[i..index]; // Return non-empty match
            }
            i = index + separator.Length;
            index = str.IndexOf(separator, i, len - i);
        }
        while (index != -1);

        if (i < len) // Has remainder?
        {
            yield return str[i..len]; // Return remaining trail
        }
    }

    /// <summary>
    /// Splits string to array of characters.
    /// </summary>
    /// <param name="str">Your string.</param>
    /// <returns>Array of characters in string format.</returns>
    public static string[] Cut(this string str)
    {
        string[] result = new string[str.Length];
        for (int i = 0; i < str.Length; i++)
        {
            result[i] = str[i].ToString();
        }
        return result;
    }
    #endregion

    #region Layout Helpers
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetScreenSizeInUnits() => GetScreenSizeInUnits(Camera.main);
    public static Vector2 GetScreenSizeInUnits(Camera camera)
    {
        float worldHeight = camera.orthographicSize * 2;
        return new((float)Screen.width / Screen.height * worldHeight, worldHeight);
    }
    #endregion

    #region Networking
    /// <summary>
    /// Mixes two hash values.
    /// </summary>
    /// <returns>Total hash.</returns>
    public static int HashMix(params int[] hashes)
    {
        int result = 0;
        for (int i = 1; i < hashes.Length; i++)
            result = HashMix(hashes[i - 1], hashes[i]);
        return result;
    }
    /// <inheritdoc cref="HashMix(int[])"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int HashMix(int hash1, int hash2)
        => (hash1 / 2) + (hash2 / 2);
    #endregion
}