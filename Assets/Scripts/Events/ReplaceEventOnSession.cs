using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class ReplaceEventOnSession : MonoBehaviour
    {
        public SessionManager sess;
        public GameEvent targetEvent;

        public void ReplaceSceneSingleEvent()
        {
            sess.mEvents.OnSceneLoadedSingle = targetEvent;
        }

        public void ReplaceSceneAdditiveEvent()
        {
            sess.mEvents.OnSceneLoadedSingle = targetEvent;
        }
    }
}