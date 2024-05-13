using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGravityTunnelable
{
    public void OnTunnelEnter(Vector3 dir);
    public void OnTunnelExit(Vector3 dir);
}
