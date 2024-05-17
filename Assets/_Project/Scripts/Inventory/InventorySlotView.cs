using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotView : MonoBehaviour, IDropHandler
{
	[SerializeField] private int _slotIndex;
    [SerializeField] private GameObject bulletDragPrefab;

    [SerializeField] private BulletView bulletView;
	
	private BulletObject _bulletToDisplay;
	private Image _bulletImage;
	private BulletController _controller;
	
	public int InventoryIndex => _slotIndex;
	public BulletController Controller { get { return _controller; } set { _controller = value; } }

    public void UpdateView(BulletObject bulletToDisplay)
	{
        //_bulletImage = GetComponent<Image>();
        //_bulletImage.color = _bulletToDisplay != null ? _bulletToDisplay.color : Color.white;

        _bulletToDisplay = bulletToDisplay;

        if (transform.childCount == 0 && bulletToDisplay != null)
        {
            GameObject bulletUI = Instantiate(bulletDragPrefab);
            bulletUI.transform.SetParent(this.transform, false);

            BulletDragUI bulletDrag = bulletUI.GetComponent<BulletDragUI>();
            bulletDrag.parentBeforeDragIndex = InventoryIndex;

            bulletDrag.UpdateChamberVisuals();
        }
	}

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            BulletDragUI draggedItem = dropped.GetComponent<BulletDragUI>();

            UpdateView(draggedItem.bulletObject);
            

            /*
             * No longer needed I think? This was for dragging bullets from Chamber->Inventory
             * 
            if (draggedItem.currentSlot == BulletDragUI.SlotTypes.Chamber)
            {
                //Destroy draggedItem bc the BulletView loop should handle creating a new UI element
                int bulletIndex = draggedItem.parentBeforeDragIndex;
                Destroy(draggedItem.gameObject);

                bulletView.RemoveBulletFromChamber(bulletIndex);
            }
            */
        }
    }
}
