using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim_ObjectMove : MonoBehaviour
{
    float SharkSpeed = 0.05f;
    float sharkRandX, sharkRandY, sharkRandZ;
    float currentTime = 0f;
    float isFoward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (this.tag == "Shark")
        {
            this.transform.Translate(Vector3.forward * (SharkSpeed * isFoward));

            if (currentTime > 3)
            {
                //isFoward = 0;
                if (currentTime > 5)
                {
                    sharkRandX = Random.Range(-90f, 90f);
                    sharkRandY = Random.Range(-90f, 90f);
                    sharkRandZ = Random.Range(-90f, 90f);
                    this.transform.rotation = Quaternion.Euler(sharkRandX, sharkRandY, sharkRandZ);
                    isFoward = 1;
                    currentTime = 0f;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
