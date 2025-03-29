using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Climb_EnableGrabbable : MonoBehaviour
{
    [SerializeField] private float durationTime = 1.0f;

    public bool isCollide;
    private float disableTime = 0f;

    void Start()
    {
        isCollide = false;
    }
    public void Update()
    {
        if (isCollide)
        {
            disableTime += Time.deltaTime;
            GameObject[] climbableObjects = GameObject.FindGameObjectsWithTag("Climbable");
            if (disableTime >= durationTime)
            {
                foreach (GameObject climbableObject in climbableObjects)
                {
                    XRGrabInteractable grabInteractable = climbableObject.GetComponent<XRGrabInteractable>();
                    if (grabInteractable != null && !grabInteractable.enabled)
                    {
                        grabInteractable.enabled = true;
                        disableTime = 0;
                        isCollide = false;
                    }
                }
            }
        }
    }
}
