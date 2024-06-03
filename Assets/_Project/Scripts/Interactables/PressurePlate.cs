using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : OnOff
{
    [SerializeField] private Material _onColor;
    [SerializeField] private Material _offColor;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetStatus())
        {
            gameObject.GetComponent<MeshRenderer>().material = _onColor;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = _offColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            SetOn();
            gameObject.GetComponent<MeshRenderer>().material = _onColor;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            SetOff();
            gameObject.GetComponent<MeshRenderer>().material = _offColor;
        }
    }
}
