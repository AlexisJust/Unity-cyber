using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Characters / Mask")]
    public class Mask : ScriptableObject
    {
        
        public bool enableHair;
        public bool enableEyebrows;
        public CharObject obj;

        public int cost = 500;
    }
}