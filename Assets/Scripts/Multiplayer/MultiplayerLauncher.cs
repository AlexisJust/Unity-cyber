using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class MultiplayerLauncher : Photon.PunBehaviour
    {

        public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
        public int gameVersion;
        public AllMyEvents mEvents;
        public BoolVariable isConnected;

        #region Init
        private void Start()
        {
            Init();
            ConnectToServer();
        }

        public void Init()
        {
            isConnected.value = false;
            PhotonNetwork.logLevel = this.logLevel;
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = false;
        }

        public void ConnectToServer()
        {
            isConnected.value = false;
            PhotonNetwork.ConnectUsingSettings(this.gameVersion.ToString());
            mEvents.OnTryToConnectToServer.Raise();
        }
        #endregion

        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            isConnected.value = true;
            mEvents.OnConnectToServer.Raise();
            
        }

        public override void OnDisconnectedFromPhoton()
        {
            base.OnDisconnectedFromPhoton();
            isConnected.value = false;
        }

        private void OnApplicationQuit()
        {
            isConnected.value = false;
        }

        #endregion
    }
}