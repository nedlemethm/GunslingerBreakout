using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electronics : OnOff
{
    [SerializeField] private string _powerChargeTag;
    [SerializeField] private string _empTag;
    [SerializeField] private GameObject _particleSystem;
    private bool _alreadyOn;
    private bool _alreadyOff;

    // Start is called before the first frame update
    void Start()
    {
        _alreadyOn = GetStatus();
        _alreadyOff = !GetStatus();
        if (GetStatus())
        {
            _particleSystem.SetActive(true);
        }
        else
        {
            _particleSystem.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(_powerChargeTag) && !_alreadyOn) 
        {
            SetOn();
            _alreadyOn = true;
            _alreadyOff= false;
            _particleSystem.SetActive(true);
        }
        else if (other.gameObject.CompareTag(_empTag) && !_alreadyOff) 
        {     
            SetOff();
            _alreadyOn = false;
            _alreadyOff = true;
            _particleSystem.SetActive(false);
        } 
    }
}
