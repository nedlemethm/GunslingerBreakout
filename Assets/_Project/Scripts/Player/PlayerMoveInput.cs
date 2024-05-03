//Coder: Brandon Retana
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


//Class is in charge of making the player move, jump and rotate.
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier; 
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask layer;
    private bool readyToJump;

    [Header("Groud Check")]
    [SerializeField] private float playerHeight;
    private bool grounded;

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
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (grounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            readyToJump = false;
            Invoke("ResetJump", jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    //Accesses input manager and instantiates a character controller.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform = Camera.main.transform;
        rb.freezeRotation = true;
    }

    private void PlayerMovement()
    {
        
        Vector2 dir = playerControls.Player.Movement.ReadValue<Vector2>() * playerSpeed;
        playerVelocity = new Vector3(dir.x, rb.velocity.y, dir.y);
        float temp = playerVelocity.y;
        playerVelocity = cameraTransform.forward * playerVelocity.z + cameraTransform.right * playerVelocity.x;
        playerVelocity.y = temp;

        //Leo Script
        if (grounded)
        {
            rb.velocity += playerVelocity * Time.deltaTime;
        }
        else if (grounded)
        {
            rb.velocity += playerVelocity * airMultiplier * Time.deltaTime;
        }
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > playerSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * playerSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Update()
    {
        PlayerMovement();
        grounded = Physics.Raycast(rb.position, Vector3.down, playerHeight);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
    }

    //Updates the player's position and jumping 
    void FixedUpdate()
    {
        SpeedControl();
    }
}