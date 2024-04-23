using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AirBurst : MonoBehaviour
{
    [SerializeField] private GameObject airBurstOrb;
    private GameObject spawnedOrb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        SpawnAirBurstOrb(contact, rotation);

        Destroy(gameObject);
    }

    public void SpawnAirBurstOrb(ContactPoint contact, Quaternion rotation)
    {
        spawnedOrb = Instantiate(airBurstOrb, contact.point, rotation);
    }
}
