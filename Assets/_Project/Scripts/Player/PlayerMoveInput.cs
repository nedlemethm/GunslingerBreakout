//Coder: Brandon Retana
using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


//Class is in charge of making the player move, jump and rotate.
public class PlayerController : MonoBehaviour, IGravityTunnelable
{
	[SerializeField] private CinemachineInputProvider _cip;
	[Header("Movement")]
	[SerializeField] private float playerSpeed;
	[SerializeField] private float iceMaxSpeedMult;
	[SerializeField] private float iceGrip;
    [SerializeField] private float jumpHeight;
	[SerializeField] private float jumpCooldown;
	[SerializeField] private float airMultiplier;
	[SerializeField] private float groundDrag;
	[SerializeField] private LayerMask layer;
	private bool readyToJump;
	private bool tunnelsIn = false;
	private Vector3 gravTunnelDir;

	[Header("Groud Check")]
	[SerializeField] private float playerHeight;
	[SerializeField] private GameObject groundCheck;
    [SerializeField] private float groundCheckRadius;
    private bool grounded;
	private bool _moving;
	private bool inIce;
	private Vector2 _moveDirection;

	private PlayerControls playerControls;
	private Rigidbody rb;
	private Transform cameraTransform;
	private Vector3 playerVelocity;

	[Header("Crouch")]
	[SerializeField] private float heightMultiplier;
	[SerializeField] private float speedMultiplier;
	[SerializeField] private bool holdKey;
	private bool isCrouched;

	private void Awake()
	{
		playerControls = new();
		playerControls.Player.Jump.started += PlayerJump;
		playerControls.Player.Movement.performed += HandleMove;
		playerControls.Player.Movement.started += HandleMove;
		playerControls.Player.Movement.canceled += HandleMove;
		playerControls.Player.Crouch.started += EnableCrouch;
		playerControls.Player.Crouch.canceled += DisableCrouch;
		playerControls.Enable();
		
		GameSignals.TOOLBAR_ENABLED.AddListener(DisableControls);
		GameSignals.TOOLBAR_DISABLED.AddListener(EnableControls);
	}

    private void DisableCrouch(InputAction.CallbackContext context)
    {
		if(holdKey && isCrouched && !Physics.Raycast(transform.position, Vector3.up, playerHeight/heightMultiplier)){
			playerHeight /= heightMultiplier;
			playerSpeed /= speedMultiplier;
        	transform.localScale = Vector3.one;
			rb.position = new Vector3(transform.position.x, transform.position.y + (1 - heightMultiplier), transform.position.z);
			isCrouched = false;
		}
    }

    private void EnableCrouch(InputAction.CallbackContext context)
    {
		if(!holdKey && isCrouched && !Physics.Raycast(transform.position, Vector3.up, playerHeight/heightMultiplier)){
			playerHeight /= heightMultiplier;
			playerSpeed /= speedMultiplier;
        	transform.localScale = Vector3.one;
			rb.position = new Vector3(transform.position.x, transform.position.y + (1 - heightMultiplier), transform.position.z);
			isCrouched = false;
		}
		else if (!isCrouched && grounded){
			playerHeight *= heightMultiplier;
			playerSpeed *= speedMultiplier;
			transform.localScale = new Vector3(1f, heightMultiplier, 1f);
			rb.position = new Vector3(transform.position.x, transform.position.y - (1-heightMultiplier), transform.position.z);
			isCrouched = true;
		}
    }

    private void OnDestroy()
	{
		GameSignals.TOOLBAR_ENABLED.RemoveListener(DisableControls);
		GameSignals.TOOLBAR_DISABLED.RemoveListener(EnableControls);
	}

	private void OnDisable()
	{
		playerControls.Disable();
	}

	public void SetInIce(bool inIce)
	{
		this.inIce = inIce;
	}
	
	private void EnableControls(ISignalParameters parameters)
	{
		playerControls.Enable();
		_cip.enabled = true;
		//Time.timeScale = 1f;
	}
	
	private void DisableControls(ISignalParameters parameters)
	{
		playerControls.Disable();
		_cip.enabled = false;
		//Time.timeScale = playerSlowdown;
	}
	
	private void HandleMove(InputAction.CallbackContext context)
	{
		_moveDirection = context.ReadValue<Vector2>();
		_moving = _moveDirection.magnitude > 0;
	}

	private void PlayerJump(InputAction.CallbackContext context)
	{
		if (grounded && !isCrouched)
		{
			rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
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
		Vector2 dir = _moveDirection * playerSpeed;
		// Vector3 horizontalVelocity = (transform.right * _horizontalInput.x + transform.forward * _horizontalInput.y) * playerSpeed;
		playerVelocity = new Vector3(dir.x, rb.velocity.y, dir.y);
		float temp = playerVelocity.y;
		playerVelocity = cameraTransform.forward * playerVelocity.z + cameraTransform.right * playerVelocity.x;
		playerVelocity.y = temp;

		if (tunnelsIn && !_moving)
		{
			rb.velocity = gravTunnelDir;
			playerVelocity.y = 0;
			rb.velocity += playerVelocity * .08f;
		}
		else
		{
			//Leo Script
			if (grounded)
			{
				if (!inIce)
				{
                    /*
                    if (!_moving)
                    {
                        rb.velocity = playerVelocity * playerSpeed;
                    }
                    else
                    {
                        rb.velocity = playerVelocity * playerSpeed;
                    }
					*/
                    rb.velocity = new Vector3(playerVelocity.x, 0, playerVelocity.z) * playerSpeed + Vector3.up * playerVelocity.y;
                }
				else
				{
					playerVelocity.y = 0;
                    rb.velocity += playerVelocity * iceGrip * Time.deltaTime;
					float newSpeed = Mathf.Min(rb.velocity.magnitude, playerSpeed * iceMaxSpeedMult * 2.5f);
					rb.velocity = rb.velocity.normalized * newSpeed;
                }
				// rb.MovePosition(playerVelocity * Time.deltaTime);
			}			
			else if (!grounded)
			{
                rb.velocity = new Vector3(playerVelocity.x, 0, playerVelocity.z) * playerSpeed * airMultiplier + Vector3.up * playerVelocity.y;
            }
		}
		

	}

	public void OnTunnelEnter(Vector3 dir)
	{
		tunnelsIn = true;
		gravTunnelDir = Vector3.zero;
		gravTunnelDir += dir;
		rb.useGravity = false;
	}

	public void OnTunnelExit(Vector3 dir)
	{
		tunnelsIn = false;
		gravTunnelDir -= dir;
		rb.useGravity = true;
	}

	private void SpeedControl()
	{
		if (!inIce)
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

	private void FixedUpdate()
	{
		SpeedControl();
		PlayerMovement();
		RaycastHit hit;
		//grounded = Physics.Raycast(rb.position, Vector3.down, playerHeight);
		grounded = Physics.SphereCast(rb.position, groundCheckRadius + .2f, Vector3.down, out hit, playerHeight);

		if (grounded)
		{
			if (!inIce)
            {
                rb.drag = groundDrag;
            }
			else
			{
				rb.drag = 0f;
			}
			readyToJump = true;
		}
		else
		{
			readyToJump = false;
		}
	}
}
