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
        if (grounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
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
        
        Vector2 dir = playerControls.Player.Movement.ReadValue<Vector2>();
        playerVelocity = new Vector3(dir.x, rb.velocity.y, dir.y);
        float temp = playerVelocity.y;
        playerVelocity = cameraTransform.forward * playerVelocity.z + cameraTransform.right * playerVelocity.x;
        playerVelocity.y = temp;
        //rb.velocity += playerVelocity * Time.deltaTime;
        
        //Leo Script
        if (grounded)
        {
            rb.AddForce(playerVelocity * playerSpeed * 10f, ForceMode.Force);
        }
        else if (grounded)
        {
            rb.AddForce(playerVelocity * playerSpeed * 10f * airMultiplier, ForceMode.Force);
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
        grounded = Physics.Raycast(rb.position, Vector3.down, playerHeight);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
    }

    //Updates the player's position and jumping 
    void FixedUpdate()
    {
        PlayerMovement();
        SpeedControl();
    }


}