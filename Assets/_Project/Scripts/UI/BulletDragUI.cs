using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public BulletObject bulletObject;
    [HideInInspector] public ChamberSlotView currentChamber;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private Sprite normalSprite;

    [SerializeField] private Sprite draggedSprite;
    [SerializeField] private Vector2 draggedDimensions;

    [HideInInspector] public int inventorySlotIndex;
    [HideInInspector] public int currentSlotIndex;

    [HideInInspector] public bool draggable;
    private bool loaded;

    //Drag Related Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        image.sprite = draggedSprite;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, draggedDimensions.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, draggedDimensions.y);
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

        if (!loaded)
            image.sprite = null;
    }

    //Visual Functions
    public void Setup(BulletObject bulletObj, int index)
    {
        draggable = true;
        loaded = false;

        bulletObject = bulletObj;
        currentSlotIndex = index;
        inventorySlotIndex = index;

        image.color = bulletObject.color;
    }

    public void AddedToChamber(ChamberSlotView chamber)
    {
        image.sprite = draggedSprite;
        rectTransform.localEulerAngles = Vector3.zero;
        loaded = true;
        currentChamber = chamber;
    }
}
