using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO
{
    public class UI_UpdateText : MonoBehaviour
    {
        public Text target;

        public FloatVariable floatVariable;
        public IntVariable intVariable;
        public StringVariable stringVariable;

        private void OnEnable()
        {
            if (target == null)
                target = GetComponentInChildren<Text>();
        }

        public void UpdateTextFromFloatVariable()
        {
            target.text = floatVariable.value.ToString();
        }

        public void UpdateTextFromFloat(float v)
        {
            target.text = v.ToString();
        }

        public void UpdateTextFromIntVariable()
        {
            target.text = intVariable.value.ToString();
        }

        public void UpdateTextFromInt(int v)
        {
            target.text = v.ToString();
        }

        public void UpdateTextFromStringVariable()
        {
            target.text = stringVariable.value;
        }

        public void UpdateTextFromString(string r)
        {
            target.text = r;
        }

    }
}