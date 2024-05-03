//Coder: Brandon Retana
using System;
using UnityEngine;
using UnityEngine.InputSystem;


//Class is in charge of making the player move, jump and rotate.
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed, jumpHeight, playerHeight;
    [SerializeField] private LayerMask layer;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Transform cameraTransform;
    private Vector3 playerVelocity;

    private void Awake()
    {
        playerControls = new();
        playerControls.Player.Jump.started += PlayerJump;
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void PlayerJump(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(rb.position, Vector3.down, playerHeight))
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    //Accesses input manager and instantiates a character controller.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PlayerMovement()
    {
        Vector2 dir = playerControls.Player.Movement.ReadValue<Vector2>() * playerSpeed;
        playerVelocity = new Vector3(dir.x, rb.velocity.y, dir.y);
        float temp = playerVelocity.y;
        playerVelocity = cameraTransform.forward * playerVelocity.z + cameraTransform.right * playerVelocity.x;
        playerVelocity.y = temp;
        rb.velocity += playerVelocity * Time.deltaTime;
    }

    //Updates the player's position and jumping 
    void Update()
    {
        PlayerMovement();
    }
}