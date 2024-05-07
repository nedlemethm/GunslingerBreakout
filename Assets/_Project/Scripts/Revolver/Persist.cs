using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persist : BulletBase
{
    [SerializeField] private string persistTag;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //collision.gameObject.GetComponent<MovingPlatform>().StartMoving = false;
            collision.gameObject.tag = persistTag;
        }
        Destroy(gameObject);
    }
}
