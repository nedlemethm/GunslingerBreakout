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
	
	public void CollectBullet() // Used for picking up bullets and dragging bullets from chamber back into inverntory
	{
		
	}
	
	public void LoadBullet() // Used for moving bullets into chamber
	{
		
	}
	
	public void SwapBulletOrder() // Used for swapping bullets in the chamber
	{
		
	}
}
