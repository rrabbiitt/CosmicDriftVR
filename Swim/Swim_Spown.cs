using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim_Spown : MonoBehaviour
{
    public GameObject sharkPre;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (this.transform.childCount < 10)
        {
            timer += Time.deltaTime;
            if (timer > 5.0f)
            {
                float tempX = Random.Range(-7f, 7f);
                float tempY = Random.Range(-7f, 7f);
                float tempZ = Random.Range(-7f, 7f);
                Vector3 temp3 = new Vector3(tempX, tempY, tempZ);

                float spownRandX = Random.Range(-90f, 90f);
                float spownRandY = Random.Range(-90f, 90f);
                float spownRandZ = Random.Range(-90f, 90f);
                GameObject.Instantiate(sharkPre, gameObject.transform.position + temp3, Quaternion.Euler(spownRandX, spownRandY, spownRandZ)).transform.parent = this.gameObject.transform;
                timer = 0;
            }
        }
    }
}
