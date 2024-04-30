using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflective : MonoBehaviour
{
    private Rigidbody rb;
    private RaycastHit nextWallHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 dir)
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, LayerMask.GetMask("Reflective"), QueryTriggerInteraction.Collide);
        if (didHit)
        {
            nextWallHit = hit;
        }
    }


    private void OnDirectionChange()
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayerMask.GetMask("Reflective"), QueryTriggerInteraction.Collide);
        if (didHit)
        {
            nextWallHit = hit;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        Debug.Log(other.gameObject.layer == LayerMask.NameToLayer("Reflective"));
        if (other.gameObject.layer == LayerMask.NameToLayer("Reflective"))
        {
            Debug.Log("PLEASE WORK");
            Debug.Log(rb.velocity);
            Debug.Log(nextWallHit.normal);
            Debug.Log(Vector3.Reflect(rb.velocity, -nextWallHit.normal));
            Vector3 reflected = Vector3.Reflect(rb.velocity, -nextWallHit.normal);
            rb.velocity = Vector3.ProjectOnPlane(reflected, nextWallHit.transform.up).normalized * rb.velocity.magnitude;
            Debug.Log(nextWallHit.normal);
            SetDirection(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}