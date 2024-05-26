using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public BulletObject bulletObject;

    [SerializeField] private Image image;

    [HideInInspector] public SlotTypes currentSlot;
    [HideInInspector] public int parentBeforeDragIndex;

    //Drag Related Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void MovedSlot(SlotTypes slot, int newParentIndex)
    {
        parentBeforeDragIndex = newParentIndex;
        currentSlot = slot;
    }

    public enum SlotTypes
    {
        Inventory,
        Chamber
    }
}
