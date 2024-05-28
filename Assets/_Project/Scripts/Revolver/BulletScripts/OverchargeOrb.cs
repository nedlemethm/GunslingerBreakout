using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargeOrb : Activation
{
    [SerializeField] private Color powerChargeColor;
    [SerializeField] private Color powerChargeEmission;
    [SerializeField] private Color empColor;
    [SerializeField] private Color empEmission;
    [SerializeField] private float intensity;
    [SerializeField] private string _overchargeTag;
    [SerializeField] private string _empTag;
    // currently kinda useless, just using to set up something for reflective bullets
    private bool overchargeMode;

    private void Awake()
    {
        overchargeMode = true;
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", powerChargeColor);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Emission", powerChargeEmission * intensity);
        gameObject.tag = _overchargeTag;
    }

    public override void ToggleActivation()
    {
        if (overchargeMode)
        {
            overchargeMode = false;
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", empColor);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Emission", empEmission * intensity);
            gameObject.tag = _empTag;
        }
        else
        {
            overchargeMode = true;
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", powerChargeColor);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Emission", powerChargeEmission * intensity);
            gameObject.tag = _overchargeTag;
        }
        base.ToggleActivation();
    }
    

    public bool IsOverchargeMode()
    {
        return overchargeMode;
    }
}
