using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTunnel : Activation
{
    [SerializeField] private Material _gravMat;
    private bool _isUp;
    private bool _push;
    private bool _pull;
    private bool _reset;

    private void Awake()
    {
        _isUp = true;
        _push = true;
        _pull = false;
        _reset = false;
        _gravMat.SetFloat("_Is_Up", 1.0f);
        gameObject.GetComponent<MeshRenderer>().material = _gravMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.gameObject.GetComponent<IGravityTunnelable>() != null)
            {
                if (_isUp)
                {
                    rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelEnter(transform.up);
                }
                else
                {
                    rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelEnter(-transform.up);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.gameObject.GetComponent<IGravityTunnelable>() != null && _reset)
            {
                if (_isUp)
                {
                    rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelEnter(transform.up);
                }
                else
                {
                    rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelEnter(-transform.up);
                }
                _reset = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.gameObject.GetComponent<IGravityTunnelable>() != null)
            {
                if (_isUp)
                {
                    rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelExit(transform.up);
                }
                else
                {
                    rb.gameObject.GetComponent<IGravityTunnelable>().OnTunnelExit(-transform.up);
                }
            }
        }
    }

    public override void ToggleActivation()
    {
        if (_isUp)
        {
            _isUp = false;
            _gravMat.SetFloat("_Is_Up", 0.0f);
        }
        else
        {
            _isUp = true;
            _gravMat.SetFloat("_Is_Up", 1.0f);
        }
        _reset = true;
        base.ToggleActivation();
    }

}
