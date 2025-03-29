using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim_RuleManager : MonoBehaviour
{
    private Swim_SwimControll swimControll;

    public GameObject EndingPlace;
    float point;
    bool isFinish;
    // Start is called before the first frame update
    void Start()
    {
        swimControll = this.GetComponent<Swim_SwimControll>();
        point = 100;
        isFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
        while (point > 0)
        {
            point -= Time.deltaTime;
        }

        if (isFinish)
        {
            StartCoroutine(SwimEnd());
            isFinish = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Shark"))
        {
            //temp.PointUpdate(point - 40);
            ChangeScript();
            //Fail_UI.SetActive(true);
        }

        else if (other.collider.CompareTag("Goal"))
        {
            //temp.PointUpdate(point);
            ChangeScript();
            //Victory_UI.SetActive(true);
        }
    }

    public void ChangeScript()
    {
        swimControll.GameEnd();
        isFinish = true;
        this.gameObject.transform.position = EndingPlace.transform.position + new Vector3(0, 2, 0);
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    IEnumerator SwimEnd()
    {
        yield return new WaitForEndOfFrame();
        // ending txt
        yield return new WaitForSeconds(5);
        PhotonNetwork.LoadLevel("Lobby");
    }
}
