using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChamberSlotView : MonoBehaviour
{
	[SerializeField] private int _slotIndex;
	
	private BulletObject _bulletToDisplay;
	private Image _bulletImage;
	private BulletController _controller;
	
	public int ChamberIndex => _slotIndex;
	public BulletController Controller { get { return _controller; } set { _controller = value; } }
	
	public void UpdateView(BulletObject bulletToDisplay)
	{
		_bulletImage = GetComponent<Image>();
		_bulletToDisplay = bulletToDisplay;
		
		_bulletImage.color = _bulletToDisplay != null ? _bulletToDisplay.color : Color.white;
	}
}
