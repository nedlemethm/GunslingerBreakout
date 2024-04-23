using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBustOrb : MonoBehaviour
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float upwardMod;
    [SerializeField] private float timeToDetonation;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("AirBurstExplosion", timeToDetonation);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AirBurstExplosion()
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
    }
}
