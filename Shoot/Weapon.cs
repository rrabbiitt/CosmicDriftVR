using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class Weapon : MonoBehaviourPun
{
    [SerializeField] protected float shootingForce;
    [SerializeField] protected Transform bulletSpawn;
    [SerializeField] private float recoilForce;
    [SerializeField] private float damage;

    private Rigidbody rigidBody;
    private XRGrabInteractable interactableWeapon;

    protected virtual void Awake()
    {
        interactableWeapon = GetComponent<XRGrabInteractable>();
        rigidBody = GetComponent<Rigidbody>();
        SetupInteractableWeaponEvents();
    }

    private void SetupInteractableWeaponEvents()
    {
        interactableWeapon.onSelectEntered.AddListener(PickUpWeapon);
        interactableWeapon.onSelectExited.AddListener(DropWeapon);
        interactableWeapon.onActivate.AddListener(StartShooting);
        interactableWeapon.onDeactivate.AddListener(StopShooting);
    }

    private void PickUpWeapon(XRBaseInteractor interactor)
    {
        interactor.GetComponent<MeshHidder>().Hide();
    }

    private void DropWeapon(XRBaseInteractor interactor)
    {
        interactor.GetComponent<MeshHidder>().Show();
    }

    protected virtual void StartShooting(XRBaseInteractor interactor)
    {
        photonView.RPC("ShootRPC", RpcTarget.All);
    }

    [PunRPC]
    private void ShootRPC()
    {
        Shoot();
    }

    protected virtual void StopShooting(XRBaseInteractor interactor)
    {

    }

    protected virtual void Shoot()
    {
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        rigidBody.AddRelativeForce(Vector3.back * recoilForce, ForceMode.Impulse);
    }

    public float GetShootingForce()
    {
        return shootingForce;
    }

    public float GetDamage()
    {
        return damage;
    }

}
