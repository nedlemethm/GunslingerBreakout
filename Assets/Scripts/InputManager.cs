//Coder: Brandon Retana
using UnityEngine;

//Class in charge of managing all the input data from the New Input System.
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance{ get { return _instance;}}
    private PlayerControls playerControls;

    //Creates a singleton and a playerControls instance.
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        playerControls = new PlayerControls();
    }

    //Helper methods to turn on and off the input system.
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    //Helper methods to obtain input data about the player actions' map.
    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return playerControls.Player.Jump.triggered;
    }
}
