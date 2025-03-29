using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject networkPre;
    int point;
    float timer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        point = 10;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            timer = 0;
            UpdatePoint();
        }
    }

    public void UpdatePoint()
    {
        networkPre.GetComponent<NetworkPlayer>().personalPoint = point;
        PhotonNetwork.LoadLevel("Lobby");
    }
}
