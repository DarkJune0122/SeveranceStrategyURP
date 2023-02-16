using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SeveranceStrategy.TickManagement
{
    /// <summary>
    /// Executes game object tick events in multiple threads.
    /// </summary>
    public static class TickManager
    {
        private const int TickDelay = 100; // In milliseconds.
        private const int MaxActiveThreadsAmount = 4;
        private static readonly bool AllowMultithreading = true;

        public static float TimeDelta => TickDelay; // Temporarly constant.


        private static Thread[] m_tickThread;
        private static List<ITicker>[] m_tickers;
        private static int m_index = 0;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            if (AllowMultithreading == false)
            {
                LoopTick();
                return;
            }

            m_tickThread = new Thread[Mathf.Clamp(Environment.ProcessorCount, 1, MaxActiveThreadsAmount)];
            m_tickers = new List<ITicker>[m_tickThread.Length];

            for (int i = 0; i < m_tickThread.Length; i++)
            {
                List<ITicker> tickers = m_tickers[i] = new();
                m_tickThread[i] = new Thread(new ThreadStart(ThreadAction));

                void ThreadAction()
                {
                    while (true)
                    {
                        tickers.ForEach(t => t.Tick());
                        Thread.Sleep(TickDelay);
                    }
                }
            }
        }
        private static async void LoopTick()
        {
            m_tickers = new List<ITicker>[1] { new() };
            List<ITicker> tickers = m_tickers[0];
            while (true)
            {
                try
                {
                    tickers.ForEach(t => t.Tick());
                }
                catch (Exception e) { Debug.LogException(e); }

                await Task.Delay(TickDelay);
            }
        }

        public static void Add(ITicker ticker)
        {
            if (AllowMultithreading)
            {
                m_index++;
                m_index %= m_tickThread.Length;
                m_tickers[m_index].Add(ticker);
            }
            else m_tickers[0].Add(ticker);
        }
        public static void Remove(ITicker ticker)
        {
            if (AllowMultithreading)
            {
                if (m_index <= 0)
                    m_index = m_tickers.Length - 1;

                m_tickers[m_index].Remove(ticker);
            }
            else m_tickers[0].Remove(ticker);
        }
        public static void Reset()
        {
            for (int i = 0; i < m_tickers.Length; i++)
            {
                m_tickers[i].Clear();
            }
        }
    }
}

