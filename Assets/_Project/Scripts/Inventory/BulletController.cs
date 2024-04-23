using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This controller will link internal bullet selection
public class BulletController : MonoBehaviour
{
	private static int _maxBulletCount = 6;
	
	private BulletModel _bulletModel;
	private BulletView _bulletView;
	
	private void Awake()
	{
		_bulletModel = new(_maxBulletCount);
		_bulletView = FindObjectOfType<BulletView>();
		_bulletView.Initialize();
	}
	
	public void ShootBullet()
	{
		_bulletModel.ShootBullet();
		_bulletView.UpdateView();
	}
	
	public void AddBullet(int slotIndex, Bullet bulletType)
	{
		_bulletModel.addBullet(slotIndex, bulletType);
		_bulletView.UpdateView();
	}
}
