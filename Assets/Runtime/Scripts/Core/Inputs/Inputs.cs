using UnityEngine;

namespace SeveranceStrategy
{
    public static class Inputs
    {
        public static int GetAxis(KeyCode positive, KeyCode negative) => Input.GetKey(positive) ? Input.GetKey(negative) ? 0 : 1 : Input.GetKey(negative) ? -1 : 0;
    }
}
