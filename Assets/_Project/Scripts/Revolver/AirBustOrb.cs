using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBustOrb : Activation
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float upwardMod;

    public override void ToggleActivation()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardMod);
            }
        }
        Destroy(gameObject);
        base.ToggleActivation();
    }
}
