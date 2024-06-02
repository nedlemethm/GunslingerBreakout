using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBustOrb : Activation
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float upwardMod;

    [SerializeField] Animator animator;

    [SerializeField] SphereCollider sphereCollider;

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

        StartCoroutine(Explode());

        base.ToggleActivation();
    }

    IEnumerator Explode()
    {
        animator.SetTrigger("Explode");

        sphereCollider.enabled = false;

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }
}
