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
	[SerializeField] private float jumpHeight;
	[SerializeField] private float jumpCooldown;
	[SerializeField] private float airMultiplier;
	[SerializeField] private float groundDrag;
	[SerializeField] private LayerMask layer;
	private bool readyToJump;
	private int tunnelsIn = 0;
	private Vector3 gravTunnelDir;

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
		
		GameSignals.TOOLBAR_ENABLED.AddListener(DisableControls);
		GameSignals.TOOLBAR_DISABLED.AddListener(EnableControls);
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

	private void PlayerJump(InputAction.CallbackContext context)
	{
		if (grounded)
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

		Vector2 dir = playerControls.Player.Movement.ReadValue<Vector2>() * playerSpeed;
		playerVelocity = new Vector3(dir.x, rb.velocity.y, dir.y);
		float temp = playerVelocity.y;
		playerVelocity = cameraTransform.forward * playerVelocity.z + cameraTransform.right * playerVelocity.x;
		playerVelocity.y = temp;

		if (tunnelsIn > 0)
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
				rb.velocity += playerVelocity * Time.deltaTime;
			}
			else if (!grounded)
			{
				rb.velocity += playerVelocity * airMultiplier * Time.deltaTime;
			}
		}
		

	}

	public void OnTunnelEnter(Vector3 dir)
	{
		tunnelsIn++;
		gravTunnelDir += dir;
		rb.useGravity = false;
	}

	public void OnTunnelExit(Vector3 dir)
	{
		tunnelsIn--;
		gravTunnelDir -= dir;
		if (tunnelsIn <= 0)
		{
			rb.useGravity = true;
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

	private void FixedUpdate()
	{
		SpeedControl();
		PlayerMovement();
		grounded = Physics.Raycast(rb.position, Vector3.down, playerHeight);

		if (grounded)
		{
			rb.drag = groundDrag;
			readyToJump = true;
		}
		else
		{
			readyToJump = false;
		}
	}
}
