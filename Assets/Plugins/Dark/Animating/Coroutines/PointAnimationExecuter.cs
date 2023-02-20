using UnityEngine;

namespace Dark.Animation.Coroutines
{
    public class PointAnimationExecuter : MonoBehaviour
    {
        public static PointAnimationExecuter Instance;

#if DARK_SAVEEX
        public void Awake()
        {
            if (Instance != null)
                Debug.LogWarning($"[{nameof(PointAnimationExecuter)}] Executer alread existing! Old one replaced with new one.");

            DontDestroyOnLoad(Instance = this);
        }
#else
        public void Awake() => DontDestroyOnLoad(Instance = this);
#endif
    }
}
