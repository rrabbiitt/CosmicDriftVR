using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Climb_CharacterControllerGravity : MonoBehaviour
{
    private CharacterController _characterController;
    private bool _climbing = false;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Climb_ClimbProvider.ClimbActive += ClimbActive;
        Climb_ClimbProvider.ClimbInActive += ClimbInActive;
    }
    private void OnDestroy()
    {
        Climb_ClimbProvider.ClimbActive -= ClimbActive;
        Climb_ClimbProvider.ClimbInActive -= ClimbInActive;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_characterController.isGrounded && !_climbing)
        {
            _characterController.SimpleMove(new Vector3());
        }
    }
    private void ClimbActive()
    {
        _climbing = true;
    }

    private void ClimbInActive()
    {
        _climbing = false;
    }
}
