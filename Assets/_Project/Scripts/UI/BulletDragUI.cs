using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class BulletDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public BulletObject bulletObject;
    [HideInInspector] public ChamberSlotView currentChamber;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private TextMeshProUGUI bulletName;

    [SerializeField] private Sprite draggedSprite;
    [SerializeField] private Vector2 draggedDimensions;

    private Vector3 originalRotation;
    private Color originalColor;
    private string originalText;

    [HideInInspector] public int inventorySlotIndex;
    [HideInInspector] public int currentSlotIndex;

    [HideInInspector] public bool draggable;
    private bool loaded;

    //Drag Related Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;

        originalRotation = rectTransform.localEulerAngles;
        originalColor = image.color;
        originalText = bulletName.text;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        bulletName.text = null;

        image.sprite = draggedSprite;
        image.color = Color.white;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, draggedDimensions.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, draggedDimensions.y);
        rectTransform.localEulerAngles = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;
        
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        rectTransform.localScale = Vector3.one;

        if (!loaded)
        {
            image.sprite = normalSprite;
            image.color = originalColor;
            rectTransform.localEulerAngles = originalRotation;
            bulletName.text = originalText;
        }
    }

    //Visual Functions
    public void Setup(BulletObject bulletObj, int index)
    {
        draggable = true;   
        loaded = false;

        bulletObject = bulletObj;
        currentSlotIndex = index;
        inventorySlotIndex = index;

        draggedSprite = bulletObject.icon;

        image.color = bulletObject.color;
        bulletName.text = bulletObject.name.ToUpper();
    }

    public void AddedToChamber(ChamberSlotView chamber)
    {
        image.sprite = draggedSprite;
        rectTransform.localEulerAngles = Vector3.zero;
        loaded = true;
        currentChamber = chamber;
    }
}
