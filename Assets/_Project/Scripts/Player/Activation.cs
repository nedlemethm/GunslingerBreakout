using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activation : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame

    public virtual void ToggleActivation() 
    {
        Debug.Log("Activation");
    }
}
