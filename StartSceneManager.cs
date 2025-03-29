using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    public string gameSceneName = "spider";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }
}
