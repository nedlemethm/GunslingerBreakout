using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private string resultsScene;
    [SerializeField] private string playerTag;
    private RestartLevel instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = FindObjectOfType<RestartLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            SceneManager.LoadScene(resultsScene);
            Cursor.lockState = CursorLockMode.None;
            instance.StopTime();
        }
    }
}
