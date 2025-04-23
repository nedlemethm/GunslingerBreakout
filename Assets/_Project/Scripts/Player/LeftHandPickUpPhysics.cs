using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandPickUpPhysics : MonoBehaviour
{
    private float initialDrag;
    private PlayerControls playerInput;

    [Header("Pickup Settings")]

    [SerializeField] private Transform leftHand;
    [SerializeField] private GameObject player;
    private GameObject pickedObj;
    private Rigidbody pickedObjRB;

    [Header("Physics Settings")]

    [SerializeField] private float pickUpRange;
    [SerializeField] private float pickUpForce;
    [SerializeField] private float drag;
    [SerializeField] private float throwForce;

    private void Awake() {
        playerInput = new();
        playerInput.Player.PickUp.performed += PickUpObject;
        playerInput.Player.DropItem.performed += ThrowObject;
        OnEnable();
    }

    private void OnEnable()
    {
        playerInput.Player.PickUp.Enable();
        playerInput.Player.DropItem.Enable();
    }

    private void OnDisable(){
        playerInput.Player.PickUp.Disable();
        playerInput.Player.DropItem.Disable();
    }

    private void ThrowObject(InputAction.CallbackContext context)
    {
        pickedObjRB.useGravity = true;
        pickedObjRB.drag = initialDrag;
        pickedObjRB.freezeRotation = false;
        pickedObj.transform.parent = null;
        Physics.IgnoreCollision(pickedObj.GetComponent<Collider>(), player.GetComponent<CapsuleCollider>(), false);
        pickedObjRB.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        pickedObj = null;
        pickedObjRB = null;
    }

    private void PickUpObject(InputAction.CallbackContext context)
    {
        if(pickedObj == null){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange)){
                PickUpObject(hit.transform.gameObject);
            }
        }
        else{
            DropObject();
        }
    }

    private void Update()
    {
        if(pickedObj != null){
            MovePickedObj();
        }
    }

    private void MovePickedObj(){
        if(Vector3.Distance(pickedObj.transform.position, leftHand.position) > .1f){
            Vector3 direction =  leftHand.position - pickedObj.transform.position;
            pickedObjRB.AddForce(direction * pickUpForce);
        }
    }

    private void PickUpObject(GameObject obj){
        if(obj.GetComponent<Rigidbody>()){
            pickedObjRB = obj.GetComponent<Rigidbody>();
            pickedObjRB.useGravity = false;
            initialDrag = pickedObjRB.drag;
            pickedObjRB.drag = drag;
            pickedObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            pickedObj = obj;
            pickedObjRB.transform.parent = leftHand;
            Physics.IgnoreCollision(pickedObj.GetComponent<Collider>(), player.GetComponent<CapsuleCollider>(), true);
        }
    }

    private void DropObject(){
        pickedObjRB.useGravity = true;
        pickedObjRB.drag = initialDrag;
        pickedObjRB.freezeRotation = false;
        Physics.IgnoreCollision(pickedObj.GetComponent<Collider>(), player.GetComponent<CapsuleCollider>(), false);
        pickedObj.transform.parent = null;
        pickedObj = null;
        pickedObjRB = null;
    }
}
