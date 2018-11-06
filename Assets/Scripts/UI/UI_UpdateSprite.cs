using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class UI_UpdateSprite : MonoBehaviour
    {
        public Image img;
        public SpriteVariable spriteVariable;

        public void UpdateSprite()
        {
            img.sprite = spriteVariable.sprite;
        }

    }
}