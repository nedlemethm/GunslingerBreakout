//Coder: Brandon Retana
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class is in charge of making the player move, jump and rotate.
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityValue;

    private InputManager inputManager;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    //Accesses input manager and instantiates a character controller.
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
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

        Vector3 move = (Vector3)inputManager.GetPlayerMovement();
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}