using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private bool isPersistent = false;

    void Start()
    {
        if (!isPersistent)
        {
            DontDestroyOnLoad(this.gameObject);
            isPersistent = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
