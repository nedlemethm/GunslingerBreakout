using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RicochetTarget : MonoBehaviour
{
    public virtual void RicochetTriggered()
    {
        Debug.Log("Ricochet Activated");
    }
}
