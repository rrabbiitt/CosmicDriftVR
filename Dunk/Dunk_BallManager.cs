using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dunk_BallManager : MonoBehaviour
{
    Dunk_RuleManager ruleManager;
    
    public GameObject LastPlayer;
    public int ball_Point;

    void Start()
    {
        ruleManager = GameObject.FindWithTag("Manager").GetComponent<Dunk_RuleManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LeftHand" || other.gameObject.tag == "RightHand")
        {
            LastPlayer = other.gameObject.transform.root.gameObject;
        }

        if (other.gameObject.tag == "Destroyer")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Goal")
        {
            if (LastPlayer != null)
            {
                ruleManager.UpdatePoint(LastPlayer, ball_Point);
            }
        }
    }
}
