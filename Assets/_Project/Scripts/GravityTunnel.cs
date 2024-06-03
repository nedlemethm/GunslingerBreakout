using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTunnel : Activation
{
    [SerializeField] private Material _gravMat;
    private bool _isUp;
    private bool _reset;
    private Material _mat;

    private void Awake()
    {
        _isUp = true;
        _reset = true;
        _gravMat.SetFloat("_Is_Up", 1.0f);
        _mat = Material.Instantiate(_gravMat);
        gameObject.GetComponent<MeshRenderer>().material = _mat;
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
            _reset = true;
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
            }
            _reset = true;
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
            _reset = false;
        }
    }

    public override void ToggleActivation()
    {
        if (_isUp)
        {
            _isUp = false;
            _mat.SetFloat("_Is_Up", 0.0f);
        }
        else
        {
            _isUp = true;
            _mat.SetFloat("_Is_Up", 1.0f);
        }
        //_reset = true;
        base.ToggleActivation();
    }

}
