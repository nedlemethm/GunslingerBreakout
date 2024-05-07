using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChamberSlotView : MonoBehaviour, IDropHandler
{
	[SerializeField] private int _slotIndex;
	
	private BulletObject _bulletToDisplay;
	private Image _bulletImage;
	private BulletController _controller;

    [SerializeField] private BulletView bulletView;

    public int ChamberIndex => _slotIndex;
	public BulletController Controller { get { return _controller; } set { _controller = value; } }

    public void UpdateView(BulletObject bulletToDisplay)
	{
        //_bulletImage = GetComponent<Image>();
        //_bulletImage.color = _bulletToDisplay != null ? _bulletToDisplay.color : Color.white;
        _bulletToDisplay = bulletToDisplay;
	}

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            BulletDragUI draggedItem = dropped.GetComponent<BulletDragUI>();
            draggedItem.parentAfterDrag = transform;

            UpdateView(draggedItem.bulletObject);

            if (draggedItem.currentSlot == BulletDragUI.SlotTypes.Inventory)
            {
                bulletView.AddBulletToChamber(draggedItem.parentBeforeDragIndex, ChamberIndex);
            }
            else if (draggedItem.currentSlot == BulletDragUI.SlotTypes.Chamber)
            {
                bulletView.SwapBullets(draggedItem.parentBeforeDragIndex, ChamberIndex);
            }
        }
    }
}
