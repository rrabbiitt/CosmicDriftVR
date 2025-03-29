using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dunk_PlayerInfo : MonoBehaviour
{
    public int playerID;
    public int personalPoint;
    
    public void localUpdate(GameObject id, int po)
    {
        if (id == this.gameObject)
        {
            personalPoint += po;
        }
    }
    public void ServerUpdate()
    {
        // update data to server
    }
}
