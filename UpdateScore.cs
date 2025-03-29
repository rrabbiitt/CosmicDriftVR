using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    public int serverScore;
    public GameObject networkPre;
    // Start is called before the first frame update
    void Awake()
    {
        serverScore += networkPre.GetComponent<NetworkPlayer>().personalPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
