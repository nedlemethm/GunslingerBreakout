using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChamberSlotView : MonoBehaviour, IDropHandler
{
	[SerializeField] private int _slotIndex;
    [SerializeField] private GameObject bulletDragPrefab;

    private BulletObject _bulletToDisplay;
    private BulletDragUI bulletDragUI;
	private BulletController _controller;

    [SerializeField] private BulletView bulletView;

    private bool swapping;

    public int ChamberIndex => _slotIndex;
	public BulletController Controller { get { return _controller; } set { _controller = value; } }

    public void UpdateView(BulletObject bulletToDisplay)
	{
        //_bulletImage = GetComponent<Image>();
        //_bulletImage.color = _bulletToDisplay != null ? _bulletToDisplay.color : Color.white;
        _bulletToDisplay = bulletToDisplay;


        // This is causing the empty swap issue
        if (_bulletToDisplay == null && bulletDragUI != null) //This chamber slot has been fired
        {
            bulletDragUI.draggable = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        BulletDragUI draggedItem = dropped.GetComponent<BulletDragUI>();

        if (transform.childCount == 0 && draggedItem.currentChamber == null) // Add Bullet
        {
            bulletView.AddBulletToChamber(draggedItem.inventorySlotIndex, ChamberIndex);

            AddBulletUI(draggedItem);
        }
        else // Swap bullet
        {
            if (bulletDragUI != null && !bulletDragUI.draggable) // Block swapping with a fired bullet
                return;

            if (draggedItem.currentChamber == null) // Swapping with bullet from inventory
            {
                // Destroy the old BulletUI
                Destroy(bulletDragUI.gameObject);

                // Adding the new bullet
                bulletView.AddBulletToChamber(draggedItem.inventorySlotIndex, ChamberIndex);

                AddBulletUI(draggedItem);
            }
            else // Swapping with bullet from chamber
            {
                int index = draggedItem.currentSlotIndex;

                if (bulletDragUI != null) //Moving Old Bullet
                {
                    bulletDragUI.transform.SetParent(draggedItem.parentAfterDrag, false);
                    draggedItem.currentChamber.SetBulletUI(bulletDragUI);
                }
                else
                {
                    draggedItem.currentChamber.SetBulletUI(null);
                }

                //Adding New Bullet
                AddBulletUI(draggedItem);

                bulletView.SwapBullets(index, ChamberIndex);
            }
        }
    }

    public void SetBulletUI(BulletDragUI dragUI)
    {
        bulletDragUI = dragUI;

        if (bulletDragUI == null)
            return;

        bulletDragUI.AddedToChamber(this);
        bulletDragUI.currentSlotIndex = ChamberIndex;
    }

    private void AddBulletUI(BulletDragUI draggedItem)
    {
        SetBulletUI(draggedItem);
        bulletDragUI.parentAfterDrag = this.transform;
        bulletDragUI.AddedToChamber(this);
        draggedItem.OnEndDrag(null);
    }
}
