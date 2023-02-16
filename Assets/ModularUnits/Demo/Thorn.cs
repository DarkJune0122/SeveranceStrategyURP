using Dark.ModularUnits;
using System.Collections;
using UnityEngine;

namespace Assets.ModularUnits.Demo
{
    public class Thorn : MonoBehaviour, IModule
    {
        public IModule Parent => throw new System.NotImplementedException();

        public IModule[] Modules => throw new System.NotImplementedException();

        public Vector2 RelativePosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    }
}