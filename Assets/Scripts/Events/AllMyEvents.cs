using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Single Instances / All My Events")]
    public class AllMyEvents : ScriptableObject
    {
        public GameEvent CloseAnnouncer;
        public GameEvent OpenAnnouncer;
        public GameEvent OnConnectToServer;
        public GameEvent OnCreateRoom;
        public GameEvent OnJoinRoom;
        public GameEvent OnTryToConnectToServer;
        public GameEvent FadeIn;
        public GameEvent FadeOut;
        public GameEvent OnCurrentItemChange;
        public GameEvent OnGameStart;
        public GameEvent OnInventory;
        public GameEvent OnLevelChanged;
        public GameEvent OnJoinLevel;
        public GameEvent OnMainMenu;
        public GameEvent OnMatchmakingMode;
        public GameEvent OnMatchPlay;
        public GameEvent OnProfileUpdate;
        public GameEvent OnSceneLoadedSingle;
        public GameEvent OnSceneLoadedAdditive;
        public GameEvent OnWeaponShoot;
        public GameEvent UpdateGameUI;

    }
}