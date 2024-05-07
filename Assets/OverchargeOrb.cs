using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeOrb : MonoBehaviour
{
    // currently kinda useless, just using to set up something for reflective bullets
    private bool overchargeMode = true;

    public void ToggleMode()
    {
        overchargeMode = !overchargeMode;
    }

    public bool IsOverchargeMode()
    {
        return overchargeMode;
    }
}
