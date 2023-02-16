using SeveranceStrategy.Blocks;
using SeveranceStrategy.Turrets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy
{
    internal class GameManager : MonoBehaviour
    {
        public static readonly List<Turret> Turrets = new();
        public static readonly Dictionary<ushort, Team> Teams = new();
        public static GameManager Current { get; private set; }
        public string GameMode = "allways";


        /*private void Awake()
        {
            Current = this;
            //GameUI.Instance.UpdateToolUI(GameMode);
            //GameUI.Instance.Show();

            StartCoroutine(DetectorUpdate());
        }
        private IEnumerator DetectorUpdate()
        {
            while (true)
            {
                foreach (Turret turret in Turrets)
                {
                    turret.DetectorUpdate();
                }
                yield return new WaitForFixedUpdate();
            }
        }*/


        public static void AddToTeam(ushort teamID, DestroyableObject.DestroyableInstance obj)
        {
            if (!Teams.TryGetValue(teamID, out Team team))
            {
                team = new(teamID);
                Teams.Add(teamID, team);
            }
            team.Units.Add(obj);
        }
        public static void RemoveObject(DestroyableObject.DestroyableInstance obj)
        {
            if (Teams.TryGetValue(obj.Team, out Team team))
            {
                team.Units.Remove(obj);

                if (team.Units.Count < 1) Teams.Remove(team.TeamID);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            GameMode = GameMode.ToLower();
        }
#endif
    }
}