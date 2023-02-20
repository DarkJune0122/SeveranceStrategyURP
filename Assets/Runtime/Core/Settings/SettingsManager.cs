using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Dark
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        /// <summary>
        /// After any change in settings - this delay will be used before sending values to server.
        /// </summary>
        private const float SavingDelay = 5f;
        /// <summary>
        /// Contains all, already initialized params.
        /// </summary>
        public static readonly Dictionary<string, AbstractParam> m_params = new();



        private static Coroutine m_dirtyCoroutine;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }

        public void SetDirty()
        {
            m_dirtyCoroutine ??= StartCoroutine(DirtyDelay());
            IEnumerator DirtyDelay()
            {
                float time = 0;
                while (time < SavingDelay)
                {
                    time += Time.deltaTime;
                    yield return null;
                }

                Save();
            }
        }
        public void Save()
        {
            if (m_dirtyCoroutine != null) StopCoroutine(m_dirtyCoroutine);

            // Connecting baked and current settings
            StringBuilder builder = new();
            foreach (AbstractParam values in m_params.Values)
            {
                builder.AppendLine(values.ToString());
            }

            File.WriteAllText(FilePath(), builder.ToString());
        }

        public static void Load()
        {
            m_params.LogEnumerable("Settings params:");
            string path = FilePath();
            if (!File.Exists(path))
            {
                return;
            }

            string[] lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                int index = lines[i].IndexOf(Apply);
                if (!m_params.TryGetValue(lines[i][..index], out AbstractParam param))
                {
                    Debug.LogWarning("Unable to find parameter: " + lines[i]);
                    continue;
                }

                param.FromString(lines[i][(index + 1)..]);
            }
        }
        private void OnApplicationQuit() => Save();
        private void OnApplicationPause(bool pause)
        {
            if (pause == true) Save();
        }
        private void OnApplicationFocus(bool focus)
        {
            if (focus == false) Save();
        }

        // Constants:
        private const char Apply = '=';
        private static string Bool(bool value) => value ? "true" : "false";
        private static bool Bool(string value) => value.StartsWith('t');
        private static string FilePath() => Application.persistentDataPath + "/settings.conf";



        /// <summary>
        /// An settings parameter.
        /// </summary>
        /// <remarks>
        /// Should be readonly! Because on initialization it will be aligned to SettingsManager.
        /// </remarks>
        public abstract class AbstractParam
        {
            public string name;

            public AbstractParam(string name)
            {
                this.name = name;
                m_params.Add(name, this);
            }

            /// <summary>
            /// Serializes data for Network sharing.
            /// </summary>
            /// <returns>Serialized in one line, <seealso cref="string"/>.</returns>
            public abstract override string ToString();
            /// <summary>
            /// Deserializes data from given string.
            /// </summary>
            /// <param name="value">All data after <seealso cref="Apply"/> symbol, serialized with <see cref="object.ToString"/></param>
            public abstract void FromString(string value);
        }

        public sealed class BooleanParam : AbstractParam
        {
            public bool Value
            {
                get => m_value;
                set
                {
                    m_value = value;
                    onValueChange?.Invoke(value);
                    Instance.SetDirty();
                }
            }

            public Action<bool> onValueChange;


            private bool m_value;
            public BooleanParam(string name, bool value) : base(name)
            {
                m_value = value;
            }

            public override string ToString() => name + Apply + Bool(m_value);
            public override void FromString(string value) => m_value = Bool(value);
        }

        public sealed class FloatParam : AbstractParam
        {
            public float Value
            {
                get => m_value;
                set
                {
                    m_value = value;
                    onValueChange?.Invoke(value);
                    Instance.SetDirty();
                }
            }

            public Action<float> onValueChange;


            private float m_value;
            public FloatParam(string name, float value) : base(name)
            {
                m_value = value;
            }

            public override string ToString() => name + Apply + m_value;
            public override void FromString(string value) => m_value = float.Parse(value);
        }

        public class IntegerParam : AbstractParam
        {
            public virtual int Value
            {
                get => m_value;
                set
                {
                    m_value = value;
                    onValueChange?.Invoke(value);
                    Instance.SetDirty();
                }
            }

            public Action<int> onValueChange;


            protected int m_value;
            public IntegerParam(string name, int value) : base(name)
            {
                m_value = value;
            }

            public override string ToString() => name + Apply + m_value;
            public override void FromString(string value) => m_value = int.Parse(value);
        }

        public sealed class EnumParam<TEnum> : AbstractParam where TEnum : unmanaged, Enum
        {
            public TEnum Value
            {
                get => m_value;
                set
                {
                    m_value = value;
                    onValueChange?.Invoke(value);
                    Instance.SetDirty();
                }
            }

            public Action<TEnum> onValueChange;


            private TEnum m_value;
            public EnumParam(string name, TEnum value) : base(name)
            {
                m_value = value;
            }

            public override string ToString() => name + Apply + m_value;
            public override void FromString(string value)
            {
#if DARK_DEBUGGING
                Debug.Log($"[Debug, {nameof(EnumParam<TEnum>)}] Trying to parse following string to Enum: {args[1]}");
#endif
                object enumValue = Enum.Parse(typeof(TEnum), value);
                m_value = UnsafeUtility.As<object, TEnum>(ref enumValue);
            }
        }

        public class IntRangeParam : IntegerParam
        {
            public override int Value
            {
                get => base.Value;
                set => base.Value = Mathf.Clamp(value, min, max);
            }

            public int min, max;
            public IntRangeParam(string name, int value, int min, int max) : base(name, value)
            {
                this.min = min;
                this.max = max;
            }
        }
    }
}
