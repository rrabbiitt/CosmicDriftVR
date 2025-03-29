using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerSpawner : MonoBehaviour
{
    private GameObject spawnedPlayerPrefab;

    void Start()
    {
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
    }
}
