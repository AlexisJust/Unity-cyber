using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class ReplaceEvent : MonoBehaviour
    {
        public GameEvent targetEvent;
        public GameEventListener listener;

        public void Replace()
        {
            listener.gameEvent.UnRegister(listener);
            listener.gameEvent = targetEvent;
            targetEvent.Register(listener);
        }
    }
}