using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletViewAnimation : MonoBehaviour
{
    [Header("Animation Variables")]

    [Header("Inventory Entry Animation")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private float inventoryEnableTime;
    [SerializeField] private Vector3 inventoryStartLocation;
    [SerializeField] private Vector3 inventoryEndLocation;
    [SerializeField] private AnimationCurve inventoryLocationCurve;
    [SerializeField] private AnimationCurve inventoryRotationCurve;
    private Coroutine inventoryRoutine;

    [Header("Chamber Entry Animation")]
    [SerializeField] private GameObject chamber;
    [SerializeField] private float chamberEnableTime;
    [Header("Rotation")]
    [SerializeField] private AnimationCurve chamberRotationCurve;
    [SerializeField] private float chamberOffset;

    [Header("Position")]
    [SerializeField] private Vector3 chamberInactiveLocation;
    [SerializeField] private Vector3 chamberActiveLocation;
    [SerializeField] private AnimationCurve chamberMoveCurve;

    [Header("Scale")]
    [SerializeField] private Vector3 chamberActiveScale;
    [SerializeField] private AnimationCurve chamberScaleCurve;

    [Header("Chamber Fired Animation")]
    [SerializeField] private float chamberFireTime;
    [SerializeField] private AnimationCurve chamberFireCurve;

    [Header("Waifu Animation")]
    [SerializeField] private Image waifuImage;
    [SerializeField] private float waifuAnimTime;
    [SerializeField] private AnimationCurve waifuCurve;

    private Coroutine chamberRoutine;
    private Coroutine toggleRoutine;
    private Coroutine waifuRoutine;

    private void LoopThroughChildElements(bool enabled)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            childTransform.gameObject.SetActive(enabled);
        }
    }

    public void OnUiEnable()
    {
        if (toggleRoutine != null)
            StopCoroutine(toggleRoutine);

        toggleRoutine = StartCoroutine(ToggleUIAnimation(true));
    }

    public void OnUiDisable()
    {
        if (toggleRoutine != null)
            StopCoroutine(toggleRoutine);

        toggleRoutine = StartCoroutine(ToggleUIAnimation(false));
    }

    public void RotateChamber(int chamberIndex)
    {
        if (chamberRoutine != null)
            StopCoroutine(chamberRoutine);

        chamberRoutine = StartCoroutine(ChamberFireAnimation(chamberIndex));
    }

    public void ToggleWaifu(bool flag)
    {
        if (waifuRoutine != null)
            StopCoroutine(waifuRoutine);

        if (flag)
        {
            waifuRoutine = StartCoroutine(EnableWaifu());
        }
        else
        {
            waifuRoutine = StartCoroutine(DisableWaifu());
        }
    }

    private IEnumerator EnableWaifu()
    {
        float currentTime = 0f;
        Color currentColor = waifuImage.color;

        while (currentTime < waifuAnimTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            waifuImage.color = Color.Lerp(currentColor, Color.white, waifuCurve.Evaluate(currentTime / waifuAnimTime));
        }
    }

    private IEnumerator DisableWaifu()
    {
        float currentTime = 0f;
        Color currentColor = waifuImage.color;

        while (currentTime < waifuAnimTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            waifuImage.color = Color.Lerp(currentColor, Color.clear, waifuCurve.Evaluate(currentTime / waifuAnimTime));
        }
    }

    private IEnumerator ChamberFireAnimation(int chamberIndex)
    {
        float currentTime = 0f;

        float currentRotation = chamber.transform.localEulerAngles.z;
        float targetRotation = chamberIndex * 60f;

        if (chamberIndex == 0)
        {
            currentRotation = -60f;
        }

        while (currentTime < chamberFireTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            chamber.transform.localEulerAngles = new Vector3(chamber.transform.localEulerAngles.x, chamber.transform.localEulerAngles.y, Mathf.Lerp(currentRotation, targetRotation, chamberFireCurve.Evaluate(currentTime / chamberFireTime)));
        }
    }

    private IEnumerator ToggleUIAnimation(bool flag)
    {
        if (inventoryRoutine != null)
            StopCoroutine(inventoryRoutine);

        if (chamberRoutine != null)
            StopCoroutine(chamberRoutine);

        if (flag)
        {
            LoopThroughChildElements(true);
            inventoryRoutine = StartCoroutine(InventoryEnableAnimation());
            chamberRoutine = StartCoroutine(ChamberEnableAnimation());

            yield return inventoryRoutine;
        }
        else
        {
            inventoryRoutine = StartCoroutine(InventoryDisableAnimation());
            chamberRoutine = StartCoroutine(ChamberDisableAnimation());
            yield return inventoryRoutine;

            LoopThroughChildElements(false);
        }
    }

    private IEnumerator InventoryEnableAnimation()
    {
        inventory.transform.localPosition = inventoryStartLocation;
        inventory.transform.localEulerAngles = new Vector3(inventory.transform.localEulerAngles.x, inventory.transform.localEulerAngles.y, inventoryRotationCurve.Evaluate(1f));

        float currentTime = 0f;

        while (currentTime < inventoryEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            inventory.transform.localPosition = Vector3.Lerp(inventoryStartLocation, inventoryEndLocation, inventoryLocationCurve.Evaluate(currentTime / inventoryEnableTime));
            inventory.transform.localEulerAngles = new Vector3(inventory.transform.localEulerAngles.x, inventory.transform.localEulerAngles.y, inventoryRotationCurve.Evaluate(currentTime / inventoryEnableTime));
        }
    }

    private IEnumerator InventoryDisableAnimation()
    {
        float currentTime = 0f;

        while (currentTime < inventoryEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            inventory.transform.localPosition = Vector3.Lerp(inventoryEndLocation, inventoryStartLocation, inventoryLocationCurve.Evaluate(currentTime / inventoryEnableTime));
            inventory.transform.localEulerAngles = new Vector3(inventory.transform.localEulerAngles.x, inventory.transform.localEulerAngles.y, inventoryRotationCurve.Evaluate(1 - currentTime / inventoryEnableTime));
        }
    }

    private IEnumerator ChamberEnableAnimation()
    {
        float currentTime = 0f;
        float currentRotation = chamber.transform.eulerAngles.z;
        float offset = currentRotation + chamberOffset;
        Vector3 currentPosition = chamber.transform.localPosition;
        Vector3 currentScale = chamber.transform.localScale;

        while (currentTime < chamberEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            chamber.transform.localEulerAngles = new Vector3(chamber.transform.localEulerAngles.x, chamber.transform.localEulerAngles.y, Mathf.Lerp(offset, currentRotation, chamberRotationCurve.Evaluate(currentTime / chamberEnableTime)));
            chamber.transform.localPosition = Vector3.Lerp(currentPosition, chamberActiveLocation, chamberMoveCurve.Evaluate(currentTime / chamberEnableTime));
            chamber.transform.localScale = Vector3.Lerp(currentScale, chamberActiveScale, chamberMoveCurve.Evaluate(currentTime / chamberEnableTime));
        }
    }

    private IEnumerator ChamberDisableAnimation()
    {
        float currentTime = 0f;
        float currentRotation = chamber.transform.eulerAngles.z;
        float offset = currentRotation + chamberOffset;
        Vector3 currentPosition = chamber.transform.localPosition;
        Vector3 currentScale = chamber.transform.localScale;

        while (currentTime < chamberEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            chamber.transform.localEulerAngles = new Vector3(chamber.transform.localEulerAngles.x, chamber.transform.localEulerAngles.y, Mathf.Lerp(currentRotation, offset, chamberRotationCurve.Evaluate(1 - currentTime / chamberEnableTime)));
            chamber.transform.localPosition = Vector3.Lerp(currentPosition, chamberInactiveLocation, chamberMoveCurve.Evaluate(currentTime / chamberEnableTime));
            chamber.transform.localScale = Vector3.Lerp(currentScale, Vector3.one, chamberMoveCurve.Evaluate(currentTime / chamberEnableTime));
        }
    }
}
