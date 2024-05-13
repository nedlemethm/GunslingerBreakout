using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistActivation : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private KeyCode activatePersist = KeyCode.Mouse1;
    [SerializeField] private string movingGeoTag;
    [SerializeField] private string persistLayer;
    private int LayerNum;

    private void Start()
    {
        LayerNum = LayerMask.NameToLayer(persistLayer);
    }

    void Update()
    {
        if (Input.GetKeyDown(activatePersist))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == LayerNum)
            {
                Debug.Log(hit.collider.gameObject);
                Activate(hit.collider.gameObject);
            }
        }
    }

    private void Activate(GameObject target)
    {
        target.layer = 0;
        if (target.tag == movingGeoTag)
        {
            target.GetComponent<MovingGeometry>().SetMoving();
        }
        else
        {
            target.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
