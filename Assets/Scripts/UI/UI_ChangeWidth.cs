using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class UI_ChangeWidth : MonoBehaviour
    {
        public GridLayoutGroup grid;
        public RectTransform targetRect;

        public bool changeWidth;
        public bool changeHeight;

        public void ChangeSize(int count)
        {
            Vector2 targetSize = targetRect.sizeDelta;
            if (changeWidth)
            {
                targetSize.x = (grid.cellSize.x + grid.spacing.x) * count;
            }

            if (changeHeight)
            {
                targetSize.y = (grid.cellSize.y + grid.spacing.y) * count;
            }

            targetRect.sizeDelta = targetSize;
        }

    }
}