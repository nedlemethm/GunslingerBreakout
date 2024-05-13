using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingGeometry : MonoBehaviour
{
    private bool moving;

    public void SetMoving()
    {
        moving = true;
    }

    public void SetNotMoving()
    {
        moving = false;
    }

    public bool GetMoving()
    {
        return moving;
    }
}
