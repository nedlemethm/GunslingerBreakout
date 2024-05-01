using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflective : BulletBase
{
    private RaycastHit nextWallHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 dir)
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, LayerMask.GetMask("Reflective"), QueryTriggerInteraction.Collide);
        if (didHit)
        {
            nextWallHit = hit;
        }
    }

    public static Vector3[] GetPoints(Vector3 startPos, Vector3 dir)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 hitPos = startPos;
        for (int i = 0; i < 50; i++)
        {
            RaycastHit hit;
            bool didHit = Physics.Raycast(hitPos, dir, out hit, Mathf.Infinity, LayerMask.GetMask("Reflective"), QueryTriggerInteraction.Collide);
            if (didHit)
            {
                hitPos = hit.point;
            }
            else
            {
                break;
            }
        }
        return points.ToArray();
    }


    public override void OnShoot(Vector3 direction, float speed)
    {
        SetDirection(direction);
        base.OnShoot(direction, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Reflective"))
        {
            rb.velocity = ReflectOnPlane(rb.velocity, nextWallHit.normal, nextWallHit.transform.up);
            SetDirection(rb.velocity);
        }
    }

    private static Vector3 ReflectOnPlane(Vector3 inVec, Vector3 normalVec, Vector3 upVec)
    {
        Vector3 reflected = Vector3.Reflect(inVec, -normalVec);
        return Vector3.ProjectOnPlane(reflected, upVec).normalized * inVec.magnitude;
    }
}