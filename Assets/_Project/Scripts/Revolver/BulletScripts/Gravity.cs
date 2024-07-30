using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : BulletBase
{
    [SerializeField] private GameObject gravTunnel;

    protected override void OnCollisionEnter(Collision collision)
    {
        Vector3 norm = collision.contacts[0].normal;
        Vector3 loc = collision.contacts[0].point;
        Instantiate(gravTunnel, loc, Quaternion.LookRotation(norm), null);
        base.OnCollisionEnter(collision);
    }
}