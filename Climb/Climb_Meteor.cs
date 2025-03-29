using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

public class Climb_Meteor : MonoBehaviour
{
    [SerializeField] private ParticleSystem ExplosionFX; // 폭발 특수 효과
    public float disableDuration = 1.0f; // 스크립트 비활성화 시간

    private bool isCollide;
    public float disableTime = 0f;

    private Climb_EnableGrabbable enableGrabbable;

    void Start()
    {
        isCollide = false;
        enableGrabbable = GameObject.Find("FunctionManager").GetComponent<Climb_EnableGrabbable>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MainCamera"))
        {
            Vector3 contactPoint = collision.contacts[0].point; // destroy 위치
            Destroy(gameObject);

            // 터지는 이펙트 실행
            ParticleSystem effect = Instantiate(ExplosionFX, contactPoint, Quaternion.identity);
            effect.Stop();
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);

            if (collision.gameObject.CompareTag("MainCamera"))
            {
                GameObject[] climbableObjects = GameObject.FindGameObjectsWithTag("Climbable");
                // 'climbable' 태그를 가진 오브젝트들의 'grabbable' 스크립트 비활성화
                foreach (GameObject climbableObject in climbableObjects)
                {
                    XRGrabInteractable grabInteractable = climbableObject.GetComponent<XRGrabInteractable>();
                    if (grabInteractable != null && grabInteractable.enabled)
                    {
                        grabInteractable.enabled = false;
                        //disableTime = 0f;
                    }
                }
                enableGrabbable.isCollide = true;
            }
        }
    }
}
