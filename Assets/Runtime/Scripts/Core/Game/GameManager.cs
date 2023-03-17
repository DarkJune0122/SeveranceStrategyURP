using SeveranceStrategy.Core;
using SeveranceStrategy.Game.Generators;
using SeveranceStrategy.Game.Kinematics;
using SeveranceStrategy.IO;
using SeveranceStrategy.Loading;
using SeveranceStrategy.Prototypes.Sources;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
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
        public static readonly List<OreInstance> ores = new();


        private static GameManager m_instance;
        private GameManager() => m_instance = this;



        private static bool m_isGenerating;
        private static CancellationTokenSource m_token = new();
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
                Debug.LogWarning("Map generation was aborted at final stage! File already created at path:\n" + file.path);
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

#if UNITY_EDITOR
            m_sceneLoadingOperation = SceneManager.LoadSceneAsync(m_instance.m_gameScene);
            yield return m_sceneLoadingOperation;
#else
            yield return SceneManager.LoadSceneAsync(m_instance.m_gameScene);
#endif
            using SaveFile.MapData mapData = file.LoadData(); // Preloading map data
            Map map = new(file.chunkSize.x, file.chunkSize.y);

            LoadManager.Progress("Map loading skiped successfully!", 1f);
            LoadManager.StopLoading();
            Map.current = map;
            /*LoadManager.Progress("Loading environment...", 0.2f);
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

                    if (!StaticData.classPool.TryGetValue(@class, out StaticData environmentalObject))
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
                DictionaryPool<int, ObjectInstance> structurePool = new();
                string[] all = mapData.environment.Split(Generator.DataSeparator);
                foreach (string str in all)
                {

                }
            }
            void LoadUnits()
            {
                if (mapData.units == null) return;
                DictionaryPool<int, ObjectInstance> environemntPool = new();
                string[] all = mapData.environment.Split(Generator.DataSeparator);
                foreach (string str in all)
                {

                }
            }*/

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
        public static bool PreventGizmosDraw => !Application.isPlaying || (m_sceneLoadingOperation is not null && !m_sceneLoadingOperation.isDone);
        private static AsyncOperation m_sceneLoadingOperation;
        private void OnValidate() => m_seed = Random.Range(int.MinValue, int.MaxValue);
        private void OnDrawGizmos()
        {
            if (PreventGizmosDraw || !CameraManager.Instance) return;

            if (!Physics.Raycast(CameraManager.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                return;
            }

            const float LineScale = 2f;
            float oreMagnitudeDistance = CameraManager.Instance.Offset * 0.08f;
            if (GetClosestOre(hit.point, out OreInstance ore) > oreMagnitudeDistance)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hit.point, hit.point + LineScale * hit.normal);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(ore.transform.position, ore.transform.position + LineScale * ore.transform.up);
            }


            // Determine closest ore from ore array.
            static float GetClosestOre(Vector3 source, out OreInstance instance)
            {
                instance = null;
                float minDistance = float.PositiveInfinity;

                // Finsing closest to source ore
                foreach (OreInstance ore in ores)
                {
                    float dist = (ore.transform.position - source).sqrMagnitude;
                    if (minDistance > dist)
                    {
                        minDistance = dist;
                        instance = ore;
                    }
                }
                return minDistance;
            }
        }
#endif
    }
}