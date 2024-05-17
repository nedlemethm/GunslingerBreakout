using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeOrb : Activation
{
    [SerializeField] private Material powerChargeMat;
    [SerializeField] private Material empMat;
    // currently kinda useless, just using to set up something for reflective bullets
    private bool overchargeMode = true;

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Electronics>() != null)
        {
            if (collision.gameObject.GetComponent<Electronics>().GetStatus())
            {
                collision.gameObject.GetComponent<Electronics>().SetOff();
            }
            else
            {
                collision.gameObject.GetComponent<Electronics>().SetOff();
            }
        }
    }

    
    public override void ToggleActivation()
    {
        if (overchargeMode)
        {
            overchargeMode = false;
            gameObject.GetComponent<MeshRenderer>().material = empMat;
        }
        else
        {
            overchargeMode = true;
            gameObject.GetComponent<MeshRenderer>().material = powerChargeMat;
        }
    }
    

    public bool IsOverchargeMode()
    {
        return overchargeMode;
    }
}
