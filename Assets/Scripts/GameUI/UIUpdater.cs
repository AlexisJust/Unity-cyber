using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.UI
{
    public class UIUpdater : MonoBehaviour
    {
        public List<UIElements> elements = new List<UIElements>();

        public static UIUpdater singleton;

        private void Awake()
        {
            if (UIUpdater.singleton == null)
            {
                singleton = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            for(int i=0; i < elements.Count; i++)
            {
                elements[i].Tick(delta);
            }
        }
    }
}