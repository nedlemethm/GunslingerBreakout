using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericGravityTunnelable : MonoBehaviour, IGravityTunnelable
{
    private Rigidbody rb;
    private int tunnelsIn = 0;
    private Vector3 gravTunnelDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravTunnelDir = Vector3.zero;
    }

    void IGravityTunnelable.OnTunnelEnter(Vector3 dir)
    {
        tunnelsIn++;
        gravTunnelDir += dir;
        rb.useGravity = false;
        rb.velocity = gravTunnelDir;
    }

    void IGravityTunnelable.OnTunnelExit(Vector3 dir)
    {
        tunnelsIn--;
        gravTunnelDir -= dir;
        if (tunnelsIn <= 0)
        {
            rb.useGravity = true;
        }
        rb.velocity = gravTunnelDir;
    }
}
