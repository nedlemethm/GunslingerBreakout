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

    public int ChamberIndex => _slotIndex;
	public BulletController Controller { get { return _controller; } set { _controller = value; } }

    public void UpdateView(BulletObject bulletToDisplay)
	{
        //_bulletImage = GetComponent<Image>();
        //_bulletImage.color = _bulletToDisplay != null ? _bulletToDisplay.color : Color.white;
        _bulletToDisplay = bulletToDisplay;

        if (_bulletToDisplay == null && bulletDragUI != null) //This chamber slot has been fired
        {
            bulletDragUI.draggable = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        BulletDragUI draggedItem = dropped.GetComponent<BulletDragUI>();

        if (transform.childCount == 0) // Add Bullet
        {
            bulletView.AddBulletToChamber(draggedItem.currentSlotIndex, ChamberIndex);

            SetCurrentDragUI(draggedItem);
            bulletDragUI.parentAfterDrag = this.transform;
            bulletDragUI.AddedToChamber(this);
        }
        else // Swap bullet
        {
            bulletView.SwapBullets(draggedItem.currentSlotIndex, ChamberIndex);

            if (bulletDragUI != null) //Moving Old Bullet
            {
                bulletDragUI.transform.SetParent(draggedItem.parentAfterDrag, false);
                bulletDragUI.currentSlotIndex = draggedItem.currentSlotIndex;
                draggedItem.currentChamber.SetCurrentDragUI(bulletDragUI);
            }

            //Adding New Bullet
            SetCurrentDragUI(draggedItem);
            bulletDragUI.parentAfterDrag = this.transform;
            bulletDragUI.AddedToChamber(this);
        }
    }

    public void SetCurrentDragUI(BulletDragUI dragUI)
    {
        bulletDragUI = dragUI;
        bulletDragUI.AddedToChamber(this);
    }
}
