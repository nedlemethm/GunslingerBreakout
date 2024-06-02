using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingGeometry : OnOff
{
    [SerializeField] protected bool startMoving;
    [SerializeField] private bool _powerSources;
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