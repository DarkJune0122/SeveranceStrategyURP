﻿using SeveranceStrategy.Buildings;
using SeveranceStrategy.Core;
using SeveranceStrategy.Game.Generators;
using SeveranceStrategy.IO;
using SeveranceStrategy.Loading;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace SeveranceStrategy.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => m_instance;
        [SerializeField] private SceneField m_mainMenu;
        [SerializeField] private SceneField m_gameScene;

        [Header("Debug")]
        [SerializeField] private Vector2Int m_mapSize;
        [SerializeField] private int m_seed;


        private static GameManager m_instance;
        private GameManager() => m_instance = this;


        private bool m_isGenerating;
        private CancellationTokenSource m_token = new();
        public async void GenerateAndLoad()
        {
            Coroutine coroutine = null;
            if (m_isGenerating)
            {
                Debug.Log("Map generation abort requested. Be aware that allocations may not gone instantly!");
                m_token.Cancel();
                m_token = new CancellationTokenSource();
                m_isGenerating = false;
                if (coroutine != null) StopCoroutine(coroutine);
                return;
            }

            m_isGenerating = true;
            SaveFile file = new("testGame")
            {
                chunkSize = m_mapSize,
                seed = m_seed,
                generatable = true,
            };
            file.path = SaveFile.GetPath(file.name);
            LoadManager.Instant_StartLoading(false, true);
            await Generator.GenerateMap(file, nameof(TestGenerator));

            coroutine = StartCoroutine(Local_LoadGame(file));
            while (coroutine != null)
            {
                await Task.Delay(100);
            }

            if (m_isGenerating == false)
            {
                Debug.LogWarning("Map generation was aborted at final stage! File already created at path:\n"+ file.path);
                return;
            }

            m_isGenerating = false;
        }

        // Static entry:
        public static void LoadMainMenu() => LoadManager.Transition(() => SceneManager.LoadScene(m_instance.m_mainMenu));
        public static void LoadGame(SaveFile file) => m_instance.StartCoroutine(Local_LoadGame(file));
        private static IEnumerator Local_LoadGame(SaveFile file)
        {
            bool shouldWait = true;
            LoadManager.StartLoading(() => shouldWait = false, useBackground: true, useProgressBar: true, "SaveFile loading begins...");
            while (shouldWait) yield return null; // Waits until fade will be shown

            yield return SceneManager.LoadSceneAsync(m_instance.m_gameScene);
            using SaveFile.MapData mapData = file.LoadData();
            Map map = new(file.chunkSize.x, file.chunkSize.y);

            LoadManager.Progress("Loading environment...", 0.2f);
            yield return null;
#if DEBUG
            LoadEnvironment();
#else
            try { LoadEnvironment(); } catch (Exception ex) { Break(ex); }
#endif

            LoadManager.Progress("Loading instances...", 0.4f);
            yield return null;
            DictionaryPool<int, object> instancePool = new();
#if DEBUG
            LoadInstances();
#else
            try { LoadInstances(); } catch (Exception ex) { Break(ex); }
#endif

            LoadManager.Progress("Loading structures...", 0.6f);
            yield return null;
#if DEBUG
            LoadStructures();
#else
            try { LoadStructures(); } catch (Exception ex) { Break(ex); }
#endif

            LoadManager.Progress("Loading entities...", 0.8f);
            yield return null;
#if DEBUG
            LoadUnits();
#else
            try { LoadUnits(); } catch (Exception ex) { Break(ex); }
#endif

            LoadManager.Progress("Map loaded successfully!", 1f);
            LoadManager.StopLoading();
            Map.current = map;

            void LoadEnvironment()
            {
                if (mapData.environment == null) return;
                string[] all = mapData.environment.Split(Generator.DataSeparator);
                for (int i1 = 0; i1 < all.Length; i1++)
                {
                    string @class = null; // Class of environmental object.
                    string serializedData = null; // Serialized data of object instance.

                    string[] fields = all[i1].Split(Generator.ParamSeparator);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        switch (fields[i][0])
                        {
                            case SaveFile.MapData.ClassPrefix: @class = fields[i][2..]; break;
                            case SaveFile.MapData.SerializedDataPrefix: serializedData = fields[i][2..]; break;
                        }
                    }

                    if (!StaticObject.classPool.TryGetValue(@class, out StaticObject environmentalObject))
                        continue;

                    // Creates new object and overwrites default values with new from serialized data.
                    if (serializedData == null)
                        JsonUtility.FromJsonOverwrite(serializedData, environmentalObject.Create<StaticInstance>(Vector2.zero, null));
                    else JsonUtility.FromJsonOverwrite(serializedData, environmentalObject.Create<StaticInstance>(Vector2.zero, null));
                }
            }
            void LoadInstances()
            {
                if (mapData.instances == null) return;
                string[] all = mapData.environment.Split(Generator.DataSeparator);
                foreach (string str in all)
                {

                }
            }
            void LoadStructures()
            {
                if (mapData.structures == null) return;
                DictionaryPool<int, DynamicInstance> structurePool = new();
                string[] all = mapData.environment.Split(Generator.DataSeparator);
                foreach (string str in all)
                {

                }
            }
            void LoadUnits()
            {
                if (mapData.units == null) return;
                DictionaryPool<int, DynamicInstance> environemntPool = new();
                string[] all = mapData.environment.Split(Generator.DataSeparator);
                foreach (string str in all)
                {

                }
            }

#if !DEBUG
            void Break(Exception ex)
            {
                SceneManager.LoadScene(m_instance.m_mainMenu);
                LoadManager.StopLoading();
                throw ex;
            }
#endif
        }
#if UNITY_EDITOR
        private void OnValidate() => m_seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
#endif
    }
}