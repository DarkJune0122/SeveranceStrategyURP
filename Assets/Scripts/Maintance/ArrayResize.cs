using System.Runtime.CompilerServices;

namespace SeveranceStrategy.Extra
{
    public static partial class Extentions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove<T>(this T[] array, T obj) => array.Remove(obj, 0, array.Length);
        public static void Remove<T>(this T[] array, T obj, int start, int end)
        {
            for (; start < end; start++)
            {
                if (!array[start].Equals(obj))
                    continue;

                array.RemoveAt(start);
                break;
            }
        }
        public static void RemoveAt<T>(this T[] array, int index)
        {
            index++;
            for(; index < array.Length; index++)
                array[index - 1] = array[index];
            System.Array.Resize(ref array, index);
        }

        public static void Add<T>(this T[] array, T obj)
        {
            System.Array.Resize(ref array, array.Length + 1);
            array[^1] = obj;
        }
        public static void Add<T>(this T[] array, params T[] objs)
        {
            System.Array.Resize(ref array, array.Length + objs.Length);
            System.Array.Copy(objs, 0, array, array.Length - objs.Length, objs.Length);
        }
    }
}
