using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]

public class Swim_SwimControll : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] float swimForce = 10f;
    [SerializeField] float dragForce = 5f;
    [SerializeField] float minForce;
    [SerializeField] float minTimeBetweenStrokes;

    [Header("Reference")]
    [SerializeField] InputActionReference leftControllerSwimReference;
    [SerializeField] InputActionReference leftControllerVelocity;
    [SerializeField] InputActionReference rightControllerSwimReference;
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] Transform trackingReference;


    public ContinuousMoveProviderBase locomoMove;
    public ContinuousTurnProviderBase locomoTurn;

    Rigidbody _rigidbody;
    float _cooldownTimer;
    bool isGaming;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
        locomoMove.enabled = false;
        locomoTurn.enabled = false;
        isGaming = true;

        swimForce = 30.0f;
        dragForce = 1.0f;
    }

    void FixedUpdate()
    {
        if (isGaming)
        {
            _cooldownTimer = Time.fixedDeltaTime;
            if (_cooldownTimer > minTimeBetweenStrokes
                && leftControllerSwimReference.action.IsPressed()
                && rightControllerSwimReference.action.IsPressed())
            {
                var leftHandVelocity = leftControllerVelocity.action.ReadValue<Vector3>();
                var rightHandVelocity = rightControllerVelocity.action.ReadValue<Vector3>();
                Vector3 localVelocity = leftHandVelocity + rightHandVelocity;
                localVelocity *= -1;

                if (localVelocity.sqrMagnitude > minForce * minForce)
                {
                    Vector3 worldVelocity = trackingReference.TransformDirection(localVelocity);
                    _rigidbody.AddForce(worldVelocity * swimForce, ForceMode.Acceleration);
                    _cooldownTimer = 0f;
                }
            }

            if (_rigidbody.velocity.sqrMagnitude > 0.01f)
            {
                _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
            }
        }
    }

    public void GameEnd()
    {
        isGaming = false;

        swimForce = 0;
        dragForce = 0;
        //this.enabled = false;
    }
}
