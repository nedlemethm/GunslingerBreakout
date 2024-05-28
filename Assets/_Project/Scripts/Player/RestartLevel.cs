using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    [SerializeField] private string playerScene, uiScene;
    [SerializeField] private int _numOfRestarts = 0;
    [SerializeField] private float _timer = 0;
    public static RestartLevel instance;
    [SerializeField] private bool _isRunning;

    private PlayerControls _playerInput;
    private static bool _controlsMade = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        if (!_controlsMade) 
        {
            Debug.Log("another one");
            _playerInput = new();
            _controlsMade = true;
            _playerInput.Player.ResetScene.performed += LevelRestart;
            _playerInput.Enable();
            _isRunning = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            _timer += Time.deltaTime;
        }
    }

    private void LevelRestart(InputAction.CallbackContext context)
    {
        SceneManager.UnloadSceneAsync(playerScene);
        SceneManager.UnloadSceneAsync(uiScene);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        SceneManager.LoadScene(playerScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(uiScene, LoadSceneMode.Additive);

        Debug.Log("restart");
        _numOfRestarts++;
        
    }

    public int GetRestarts()
    {
        return _numOfRestarts;
    }

    public float GetTime() 
    {  
        return _timer; 
    }

    public void StopTime()
    {
        _isRunning = false;
        _playerInput.Disable();
    }

}
