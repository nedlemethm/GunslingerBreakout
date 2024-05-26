using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevelTransition : MonoBehaviour
{
    [SerializeField] private CaptureScreen captureScreen;
    private Coroutine transitionRoutine;

    [Header("Animation Variables")]
    [SerializeField] private float animTime;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private AnimationCurve doorAnimCurve;

    void Start()
    {
        // Move the captureCamera to the player's position, and take a "screenshot" of their current view
        captureScreen.CaptureView(GameObject.FindGameObjectWithTag("MainCamera").transform.position);
        StartCoroutine(ExitRoutine());
    }

    private IEnumerator ExitRoutine()
    {
        yield return StartCoroutine(Animation());
        // Call the next scene load here
    }

    private IEnumerator Animation()
    {
        float currentTime = 0f;
        while (currentTime < animTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            rightDoor.transform.localEulerAngles = new Vector3(rightDoor.transform.localEulerAngles.x, doorAnimCurve.Evaluate(currentTime / animTime), rightDoor.transform.localEulerAngles.z);
            leftDoor.transform.localEulerAngles = new Vector3(leftDoor.transform.localEulerAngles.x, doorAnimCurve.Evaluate(currentTime / animTime), leftDoor.transform.localEulerAngles.z);
        }
    }
}
