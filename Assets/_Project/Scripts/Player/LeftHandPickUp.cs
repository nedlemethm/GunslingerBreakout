using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandPickUp : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform holdingPosition;
    [SerializeField] private float throwForce, pickUpRange, rotationSensitivity;
    [SerializeField] private string objectTag, layerName;
    [SerializeField] private KeyCode pickKey = KeyCode.E, throwKey = KeyCode.T;

    private GameObject heldObject; //object which we pick up
    private Rigidbody heldObjectRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index

    private void Start()
    {
        LayerNumber = LayerMask.NameToLayer(layerName);
    }
    void Update()
    {
        if (Input.GetKeyDown(pickKey))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.tag == objectTag)
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }
        if (heldObject != null)
        {
            MoveObject();
            //RotateObject();
            if (Input.GetKeyDown(throwKey) && canDrop)
            {
                StopClipping();
                ThrowObject();
            }

        }
    }

    private void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObject = pickUpObj;
            heldObjectRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjectRb.isKinematic = true;
            heldObjectRb.transform.parent = holdingPosition.transform;
            heldObject.layer = LayerNumber;
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
    {
        Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObject.layer = 0;
        heldObjectRb.isKinematic = false;
        heldObject.transform.parent = null;
        heldObject = null;
    }
    void MoveObject()
    {
        heldObject.transform.position = holdingPosition.transform.position;
    }

    /*
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            //mouseLookScript.verticalSensitivity = 0f;
            //mouseLookScript.lateralSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObject.transform.Rotate(Vector3.down, XaxisRotation);
            heldObject.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
        }
    }
    */

    private void ThrowObject()
    {
        Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObject.layer = 0;
        heldObjectRb.isKinematic = false;
        heldObject.transform.parent = null;
        heldObjectRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        heldObject = null;
    }

    private void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObject.transform.position, transform.position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObject.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
