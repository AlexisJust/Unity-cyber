using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class Crosshair : UIElements
    {
        public FloatVariable targetSpread;
        public float maxSpread = 80;
        public float defaultSpread;
        public float spreadSpeed = 5;
        public Parts[] parts;

        float t;
        float curSpread;


        public override void Tick(float delta)
        {
            t = delta * spreadSpeed;

            if (targetSpread.value > maxSpread)
                targetSpread.value = maxSpread;

            curSpread = Mathf.Lerp(curSpread, targetSpread.value, t);

            for(int i = 0; i < parts.Length; i++)
            {
                Parts p = parts[i];
                p.trans.anchoredPosition = p.pos * curSpread;
            }

            targetSpread.value = Mathf.Lerp(targetSpread.value, defaultSpread, delta);

        }

        public void AddSpread(float v)
        {
            targetSpread.value = v;
        }

        [System.Serializable]
        public class Parts
        {
            public RectTransform trans;
            public Vector2 pos;
        }
    }
}
