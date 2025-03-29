using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

public class Climb_Meteor : MonoBehaviour
{
    [SerializeField] private ParticleSystem ExplosionFX; // ���� Ư�� ȿ��
    public float disableDuration = 1.0f; // ��ũ��Ʈ ��Ȱ��ȭ �ð�

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
            Vector3 contactPoint = collision.contacts[0].point; // destroy ��ġ
            Destroy(gameObject);

            // ������ ����Ʈ ����
            ParticleSystem effect = Instantiate(ExplosionFX, contactPoint, Quaternion.identity);
            effect.Stop();
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);

            if (collision.gameObject.CompareTag("MainCamera"))
            {
                GameObject[] climbableObjects = GameObject.FindGameObjectsWithTag("Climbable");
                // 'climbable' �±׸� ���� ������Ʈ���� 'grabbable' ��ũ��Ʈ ��Ȱ��ȭ
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
