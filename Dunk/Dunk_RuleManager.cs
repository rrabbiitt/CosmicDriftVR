using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Dunk_RuleManager : MonoBehaviour
{
    Dunk_PlayerInfo playerInfo;

    GameObject goalFx;
    public GameObject BallPrefab;
    public Material[] mat = new Material[4];

    public float leftTime;
    float spownDelay;
    bool isDunk = false;
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.FindWithTag("Player").GetComponent<Dunk_PlayerInfo>();
        goalFx = GameObject.FindWithTag("Effect");
        goalFx.SetActive(false);
        leftTime = 100;
        StartCoroutine(WaitForStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDunk)
        {
            leftTime -= Time.deltaTime;
            spownDelay += Time.deltaTime;

            if (spownDelay > 0.5)
            {
                int ran = Random.Range(0, 4);
                BallSpown(ran);
                spownDelay = 0;
            }

            if (leftTime < 0)
            {
                isDunk = false;
                StartCoroutine (WaitForEnd());
            }
        }
    }

    IEnumerator WaitForStart()
    {
        // get ready for the next battle;
        yield return new WaitForSeconds(5);
        isDunk = true;
    }

    IEnumerator WaitForEnd()
    {
        // get ready for the next battle;
        yield return new WaitForSeconds(5);
        playerInfo.ServerUpdate();

        yield return new WaitForFixedUpdate();
        PhotonNetwork.LoadLevel("Lobby");
    }

    IEnumerator GoalFx()
    {
        yield return new WaitForFixedUpdate();
        goalFx.SetActive(true);
        yield return new WaitForSeconds(2);
        goalFx.SetActive(false);
        yield return new WaitForFixedUpdate();
    }

    public void UpdatePoint(GameObject last, int point)
    {
        if (isDunk)
        {
            playerInfo.localUpdate(last, point);
            StartCoroutine(GoalFx());
        }
    }

    public void BallSpown(int ran)
    {
        if (this.transform.childCount < 60)
        {
            BallPrefab.GetComponent<MeshRenderer>().material = mat[ran];
            BallPrefab.GetComponent<Dunk_BallManager>().ball_Point = ran * 10;

            float tempX = Random.Range(-4.0f, 4.0f);
            Vector3 temp = new Vector3(tempX, 0, 0);
            GameObject.Instantiate(BallPrefab, gameObject.transform.position + temp, Quaternion.identity).transform.parent = this.gameObject.transform;
        }
    }
}
