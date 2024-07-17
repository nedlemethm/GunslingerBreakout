using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isActive = false;

    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Debug.LogWarning("YERRRR");
        GameSignals.PAUSE_TOGGLED.AddListener(ToggleMenu);
    }

    private void ToggleMenu(ISignalParameters parameters)
    {
        isActive = !isActive;

        if (pauseMenu != null)
            pauseMenu.SetActive(isActive);

        if (isActive)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
