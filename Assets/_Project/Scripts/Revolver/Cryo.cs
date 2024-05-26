using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryo : BulletBase
{
    [SerializeField] private PhysicMaterial icyMaterial;
    [SerializeField] private string objectTag;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == objectTag && collision.gameObject.GetComponent<Collider>() != null)
        {
            collision.gameObject.GetComponent<Collider>().material = icyMaterial;
        }
        Destroy(gameObject);
    }
}
