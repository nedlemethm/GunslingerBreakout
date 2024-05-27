using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    private bool _isOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetOn()
    {
        _isOn = true;
        Debug.Log("Turned On");
    }

    public virtual void SetOff()
    {
        _isOn = false;
        Debug.Log("Turned Off");
    }

    public bool GetStatus()
    {
        return _isOn;
    }
}
