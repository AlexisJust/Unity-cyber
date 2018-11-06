using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Weapons / IK Position")]
    public class IKPositions : ScriptableObject
    {
        public Vector3 pos;
        public Vector3 rot;

    }
}