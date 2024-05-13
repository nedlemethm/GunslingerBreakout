using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persist : BulletBase
{
    [SerializeField] private string persistLayer;
    [SerializeField] private string movingTag;
    private int LayerNum;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LayerNum = LayerMask.NameToLayer(persistLayer);
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            collision.gameObject.layer = LayerNum;
        }
        else if (collision.gameObject.tag == movingTag)
        {
            collision.gameObject.GetComponent<MovingGeometry>().SetNotMoving();
            collision.gameObject.layer = LayerNum;
        }
        Destroy(gameObject);
    }
}
