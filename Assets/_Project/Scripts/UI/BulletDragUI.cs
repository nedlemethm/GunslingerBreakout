using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public BulletObject bulletObject;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private Sprite normalSprite;

    [SerializeField] private Sprite draggedSprite;
    [SerializeField] private Vector2 draggedDimensions;

    [HideInInspector] public SlotTypes currentSlot;
    [HideInInspector] public int parentBeforeDragIndex;

    //Drag Related Events
    public void OnBeginDrag(PointerEventData eventData)
    {
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
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.sprite = null;
        image.raycastTarget = true;
    }

    public void MovedSlot(SlotTypes slot, int newParentIndex)
    {
        parentBeforeDragIndex = newParentIndex;
        currentSlot = slot;
    }

    public void UpdateChamberVisuals()
    {
        image.sprite = draggedSprite;
        rectTransform.localEulerAngles = Vector3.zero;
    }

    public enum SlotTypes
    {
        Inventory,
        Chamber
    }
}
