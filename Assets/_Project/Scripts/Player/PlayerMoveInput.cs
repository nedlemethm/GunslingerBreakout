//Coder: Brandon Retana
using UnityEngine;
using UnityEngine.InputSystem;

//Will not run unless a CharacterController component is added to the gameObject.
[RequireComponent(typeof(CharacterController))]

//Class is in charge of making the player move, jump and rotate.
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float playerSpeed;
	[SerializeField] private float jumpHeight;
	[SerializeField] private float gravityValue;

	private PlayerControls _playerInput;
	private CharacterController controller;
	private Transform cameraTransform;
	private Vector3 playerVelocity;
	private Vector3 _moveDirection;
	private bool groundedPlayer;

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
		controller = GetComponent<CharacterController>();
		cameraTransform = Camera.main.transform;
	}

	//Updates the player's position and jumping conditions getting the input of the user
	//from the inputManager.
	void Update()
	{
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

		Vector3 move = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
		move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
		move.y = 0f;
		move.Normalize();
		controller.Move(move * Time.deltaTime * playerSpeed);

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}
	
	private void MovementAction(InputAction.CallbackContext context)
	{
		_moveDirection = context.ReadValue<Vector2>();
	}
	
	private void JumpAction(InputAction.CallbackContext context)
	{
		if(groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		}
	}
}