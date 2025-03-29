using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsProjectile : Projectile
{
    [SerializeField] private float lifeTime;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);
        Destroy(gameObject, lifeTime);
    }

    public override void Launch()
    {
        base.Launch();
        rigidBody.AddRelativeForce(Vector3.forward * weapon.GetShootingForce(), ForceMode.Impulse);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            ITakeDamage[] damageTakers = hit.collider.GetComponentsInParent<ITakeDamage>();
            foreach (var taker in damageTakers)
            {
                taker.TakeDamage(weapon, this, hit.point);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        ITakeDamage[] damageTakers = other.GetComponentsInParent<ITakeDamage> ();

        foreach (var taker in damageTakers)
        {
            taker.TakeDamage(weapon, this, transform.position);
        }
    }
}
