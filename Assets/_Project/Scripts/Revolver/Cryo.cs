using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Cryo : BulletBase
{
    [SerializeField] private PhysicMaterial icyMaterial;
    [SerializeField] private string objectTag;
    [SerializeField] private float downCastPeriod;
    [SerializeField] private GameObject iceFloor;
    private float downCastClock;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        downCastClock = downCastPeriod;
    }


    private void FixedUpdate()
    {
        if (downCastClock < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ~LayerMask.GetMask("IceFloor")))
            {
                Instantiate(iceFloor, hit.point, Quaternion.LookRotation(Vector3.Cross(hit.normal, new Vector3(1, 1, 1)), hit.normal));
            }
            downCastClock += downCastPeriod;
        }
        else
        {
            downCastClock -= Time.deltaTime;
        }
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == objectTag && collision.gameObject.GetComponent<Collider>() != null)
        {
            collision.gameObject.GetComponent<Collider>().material = icyMaterial;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            collision.gameObject.GetComponent<Rigidbody>().mass /= 2;
        }
        Destroy(gameObject);
    }
}
