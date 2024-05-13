using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTunnel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.up;
            rb.useGravity = false;
            if (rb.gameObject.GetComponent<PlayerController>() != null)
            {
                rb.gameObject.GetComponent<PlayerController>().OnTunnelEnter(transform.up);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            if (rb.gameObject.GetComponent<PlayerController>() != null)
            {
                rb.gameObject.GetComponent<PlayerController>().OnTunnelExit();
            }
        }
    }
}
