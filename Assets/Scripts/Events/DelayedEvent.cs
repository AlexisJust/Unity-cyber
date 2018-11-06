using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class DelayedEvent : MonoBehaviour
    {
        public GameEvent gameEvent;
        bool isWaiting;


        public void DelayedRaise(float v)
        {
            if (gameEvent == null)
                return;

            if (isWaiting)
                return;
            isWaiting = true;

            StartCoroutine(Delayed(v));
        }

        IEnumerator Delayed(float v)
        {
            yield return new WaitForSeconds(v);
            gameEvent.Raise();
            isWaiting = false;

        }
    }
}