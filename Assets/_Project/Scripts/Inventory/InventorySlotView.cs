using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotView : MonoBehaviour
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

        if (transform.childCount == 0 && _bulletToDisplay != null)
        {
            GameObject bulletUI = Instantiate(bulletDragPrefab);
            bulletUI.transform.SetParent(this.transform, false);

            BulletDragUI bulletDrag = bulletUI.GetComponent<BulletDragUI>();
            bulletDrag.Setup(_bulletToDisplay, _slotIndex);
        }
	}
}
