using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerCharge : BulletBase
{
    [SerializeField] private GameObject electricOrb;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        //checks that what is what is hit is an electronic and if it is off
        //if (collision.collider.CompareTag(electronicTag) &&
        //    !collision.collider.GetComponent<Electronics>().GetStatus()) 
        //{
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        SpawnElectricOrb(contact, rotation);
        //}
        base.OnCollisionEnter(collision);
        Destroy(gameObject);
    }

    public void SpawnElectricOrb(ContactPoint contact, Quaternion rotation)
    {
        Instantiate(electricOrb, contact.point, rotation);
    }
}
