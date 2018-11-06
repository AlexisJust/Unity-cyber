using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Variables / Float")]
    public class FloatVariable : ScriptableObject
    {
        public float value;

        public void Apply(FloatVariable v)
        {
            value += v.value;
        }

        public void Apply(float v)
        {
            value += v;
        }
    }
}