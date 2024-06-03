using UnityEngine;
using UnityEngine.InputSystem;

public class PushObjects : MonoBehaviour
{
    [SerializeField] private float reachRange;
    [SerializeField] private string pushTag, material;
    [SerializeField] private Camera cam;
    private PlayerControls playerInput; 
    private bool isConnected;
    private GameObject target;

    private void Awake(){
        playerInput = new();
        playerInput.Player.PickUp.started += ConnectWithObject;
        OnEnable();
    }

    private void OnEnable()
    {
        playerInput.Player.PickUp.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.PickUp.Disable();
    }

    private void ConnectWithObject(InputAction.CallbackContext context)
    {
        //Check if the player is already connected to an object
        if(!isConnected){
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit; 
            //Check if there is an object in front of the player.
            if(Physics.Raycast(ray, out hit, reachRange)){
                target = hit.collider.gameObject;
                //Check if the object has the tag to push objects and if it has an icy physics material
                if(target.GetComponent<Collider>().material.name == material && target.tag == pushTag)
                {
                    target.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
                    Physics.IgnoreCollision(target.GetComponent<Collider>(), GetComponent<Collider>(), true);
                    //Interpolation is to make the movement of the object look smooth
                    target.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; 
                    isConnected = true;
                }
            }
        }
        else{
            target.GetComponent<FixedJoint>().connectedBody = null;
            Physics.IgnoreCollision(target.GetComponent<Collider>(), GetComponent<Collider>(), false);
            //Interpolation should get deactivated because it can be very expensive.
            target.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            isConnected = false;
        }
    }
}
