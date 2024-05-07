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
	[SerializeField] private float gravityValue;
    [SerializeField] private float airMultiplier; 
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask layer;
    private bool readyToJump;

    [Header("Groud Check")]
    [SerializeField] private float playerHeight;
    private bool grounded;

    private Rigidbody rb;
	private PlayerControls _playerInput;
	private Transform cameraTransform;
	private Vector3 playerVelocity;
	private Vector3 _moveDirection;

	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Movement.started += MovementAction;
		_playerInput.Player.Movement.performed += MovementAction;
		_playerInput.Player.Movement.canceled += MovementAction;
		_playerInput.Player.Jump.started += JumpAction;
		_playerInput.Enable();
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
	}
	
	//Accesses input manager and instantiates a character controller.
	private void Start()
	{
		cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
	}

	//Updates the player's position and jumping conditions getting the input of the user
	//from the inputManager.
	void Update()
	{
		PlayerMovement();
        grounded = Physics.Raycast(rb.position, Vector3.down, playerHeight);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
	}

    void FixedUpdate()
    {
        if (grounded && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

        SpeedControl();
    }

    private void PlayerMovement()
    {
        
        Vector3 move = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
		move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
		move.Normalize();

        //Leo Script
        if (grounded)
        {
            rb.velocity += playerVelocity * Time.deltaTime;
        }
        else if (!grounded)
        {
            rb.velocity += playerVelocity * airMultiplier * Time.deltaTime;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        
    }
	
	private void MovementAction(InputAction.CallbackContext context)
	{
		_moveDirection = context.ReadValue<Vector2>();
	}
	
	private void JumpAction(InputAction.CallbackContext context)
	{
		if(grounded)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            readyToJump = false;
            Invoke("ResetJump", jumpCooldown);
		}
	}

    private void ResetJump()
    {
        readyToJump = true;
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
}
}