using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SO
{
    public class SessionManager : MonoBehaviour
    {
        public GameSettings gameSettings;
        public AllMyEvents mEvents;


        private void Awake()
        {
            //Check version
            //Update databases, asset bundles, characters, items ...
            //Connect to master server
            //if connected, allow multiplayer
            //if not allow only single

            if (mEvents == null)
                mEvents = Resources.Load("AllMyEvents") as AllMyEvents;

            gameSettings.Init();
            gameSettings.r_manager.Init();
        }

        private void Start()
        {
            if (mEvents.OnGameStart != null)
                mEvents.OnGameStart.Raise();
        }

        public void LoadSceneAsyncAdditive(string lvl)
        {
            StartCoroutine(LoadSceneAsyncAdditive_Actual(lvl));
        }

        public void LoadSceneAsyncSingle(string lvl)
        {
            StartCoroutine(LoadSceneAsyncSingle_Actual(lvl));
        }

        public void LoadLevelFromGameSettings()
        {
            LoadSceneAsyncSingle(gameSettings.uiSettings.curLvl.targetLevel);
        }

        IEnumerator LoadSceneAsyncAdditive_Actual(string lvl)
        {
            yield return SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
            if (mEvents.OnSceneLoadedAdditive != null)
            {
                mEvents.OnSceneLoadedAdditive.Raise();
                mEvents.OnSceneLoadedAdditive = null;
            }
        }

        IEnumerator LoadSceneAsyncSingle_Actual(string lvl)
        {
            yield return SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Single);
            if (mEvents.OnSceneLoadedSingle != null)
            {
                mEvents.OnSceneLoadedSingle.Raise();
                mEvents.OnSceneLoadedSingle = null;
            }
        }
    }
}