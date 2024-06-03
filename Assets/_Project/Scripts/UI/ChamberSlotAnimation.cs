using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChamberSlotAnimation : MonoBehaviour
{
    private BulletObject currentBullet;

    [Header("Waifu Animation")]
    [SerializeField] private Image waifuImage;
    [SerializeField] private float waifuAnimTime;
    [SerializeField] private AnimationCurve waifuCurve;
    private Coroutine waifuRoutine;

    public void UpdateBullet(BulletObject newBullet)
    {
        currentBullet = newBullet;
    }

    public void ToggleWaifu(bool flag)
    {
        if (waifuRoutine != null)
            StopCoroutine(waifuRoutine);

        if (flag && currentBullet != null && currentBullet.artwork != null)
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
        waifuImage.sprite = currentBullet.artwork;
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
}
