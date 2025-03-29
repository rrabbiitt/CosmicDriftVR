using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Rifle : Weapon
{
    [SerializeField] private float fireRate; // 발사 속도
    private Projectile projectile;

    private WaitForSeconds wait; // 발사 간격

    protected override void Awake()
    {
        base.Awake();
        // 자식 오브젝트 중에서 Projectile 컴포넌트를 가져와 projectile 변수에 할당
        projectile = GetComponentInChildren<Projectile>();
    }

    private void Start()
    {
        wait = new WaitForSeconds(1 / fireRate);
        projectile.Init(this);
    }

    protected override void StartShooting(XRBaseInteractor interactor)
    {
        base.StartShooting(interactor);
        StartCoroutine(ShootingCO());
    }

    private IEnumerator ShootingCO()
    {
        while (true)
        {
            Shoot();
            yield return wait;
        }
    }

    protected override void Shoot()
    {
        base.Shoot();
        projectile.Launch();
    }

    protected override void StopShooting(XRBaseInteractor interactor)
    {
        base.StopShooting(interactor);
        StopAllCoroutines();
    }
}
