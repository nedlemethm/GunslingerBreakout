using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electronics : MonoBehaviour
{
    [SerializeField] private bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOn() 
    {
        isOn = true;
        Debug.Log("Turned On");
    }

    public void SetOff()
    {
        isOn = false;
    }

    public bool GetStatus()
    {
        return isOn;
    }
}
