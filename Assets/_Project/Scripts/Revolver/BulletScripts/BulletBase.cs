using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;


    public virtual void OnShoot(Vector3 direction, float speed)
    {
        rb.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
