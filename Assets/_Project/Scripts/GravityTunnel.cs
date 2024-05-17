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
            if (rb.gameObject.GetComponent<IGravityTunnelable>() != null)
            {
                rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelEnter(transform.up);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.gameObject.GetComponent<IGravityTunnelable>() != null)
            {
                rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelExit(transform.up);
            }
        }
    }
}
