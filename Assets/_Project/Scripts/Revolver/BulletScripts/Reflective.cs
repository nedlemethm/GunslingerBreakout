using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Reflective : BulletBase
{
    [SerializeField] private Material overchargeMat;
    [SerializeField] private Material empMat;
    [SerializeField] private string _overchargeTag;
    [SerializeField] private string _empTag;
    private RaycastHit nextWallHit;
    private bool hitSet = false;
    private bool hasOvercharge;
    private bool hasEmp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 dir)
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position + .1f * dir.normalized, dir, out hit, Mathf.Infinity, LayerMask.GetMask("Reflective"), QueryTriggerInteraction.Collide);
        if (didHit)
        {
            Debug.Log("hit a wall " + hit.normal + " " + hit.collider.gameObject.name);
            nextWallHit = hit;
        }
        hitSet = didHit;
    }

    public static List<Vector3> GetPoints(Vector3 startPos, Vector3 dir)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 hitPos = startPos;
        Vector3 outHitDir = dir;
        for (int i = 0; i < 100; i++)
        {
            RaycastHit hit;
            bool didHit = Physics.Raycast(hitPos, outHitDir, out hit, Mathf.Infinity, LayerMask.GetMask("Reflective"), QueryTriggerInteraction.Collide);
            if (didHit)
            {
                hitPos = hit.point;
                outHitDir = ReflectOnPlane(outHitDir, hit.normal, hit.transform.up);
                points.Add(hitPos);
            }
            else
            {
                Debug.DrawRay(hitPos, outHitDir * 5, Color.green);
                didHit = Physics.Raycast(hitPos, outHitDir, out hit, Mathf.Infinity);
                if (didHit)
                {
                    points.Add(hit.point);
                }
                else
                {
                    points.Add(hitPos + outHitDir * 20f);
                }
                break;
            }
        }
        //Debug.Log(points.Count);
        return points;
    }


    public override void OnShoot(Vector3 direction, float speed)
    {
        SetDirection(direction);
        base.OnShoot(direction, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        OverchargeOrb orb;
        if (other.gameObject.layer == LayerMask.NameToLayer("Reflective"))
        {
            if (!hitSet)
            {
                Destroy(gameObject);
                return;
            }
            rb.velocity = ReflectOnPlane(rb.velocity, nextWallHit.normal, nextWallHit.transform.up);
            SetDirection(rb.velocity);
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            transform.Rotate(Vector3.right, 90f);
            TryElectronicsStuff(other.gameObject);
        }
        else if ((orb = other.gameObject.GetComponent<OverchargeOrb>()) != null)
        {
            if (orb.IsOverchargeMode())
            {
                hasOvercharge = orb.IsOverchargeMode();
                GetComponent<MeshRenderer>().material = overchargeMat;
                gameObject.tag = _overchargeTag;
            }
            else
            {
                hasEmp = !orb.IsOverchargeMode();
                GetComponent<MeshRenderer>().material = empMat;
                gameObject.tag = _empTag;
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        TryElectronicsStuff(collision.gameObject);
        base.OnCollisionEnter(collision);
    }

    private void TryElectronicsStuff(GameObject go)
    {
        Electronics electro;
        if ((hasOvercharge || hasEmp) && (electro = go.GetComponent<Electronics>()) != null)
        {
            if (hasOvercharge)
            {
                electro.SetOn();
            }
            if (hasEmp)
            {
                electro.SetOff();
            }
        }
    }

    private static Vector3 ReflectOnPlane(Vector3 inVec, Vector3 normalVec, Vector3 upVec)
    {
        Vector3 reflected = Vector3.Reflect(inVec, -normalVec);
        return Vector3.ProjectOnPlane(reflected, upVec).normalized * inVec.magnitude;
    }
}