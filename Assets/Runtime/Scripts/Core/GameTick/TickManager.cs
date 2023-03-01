using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SeveranceStrategy.TickManagement
{
    /// <summary>
    /// Executes game object tick events.
    /// </summary>
    public static class TickManager
    {
        private const int TickDelay = 100; // In milliseconds.
        public static float TimeDelta => m_tickDelta; // Temporarly constant.



        private static float m_tickDelta;
        private static readonly List<ITicker> m_tickers = new();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize() => LoopTick();
        private static async void LoopTick()
        {
            m_tickDelta = TickDelay; // Change: define dynamically
            while (true)
            {
                m_tickers.ForEach(t => t.Tick());
                await Task.Delay(TickDelay);
            }
        }

        public static void Add(ITicker ticker) => m_tickers.Add(ticker);
        public static void Remove(ITicker ticker) => m_tickers.Remove(ticker);
        public static void Reset() => m_tickers.Clear();
    }
}

