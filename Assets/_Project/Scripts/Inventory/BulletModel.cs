using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel
{
	private Bullet[] _equippedBullets;
	private int _maxShots;
	
	public BulletModel(int maxShots)
	{
		_maxShots = maxShots;
		_equippedBullets = new Bullet[_maxShots];
	}
	
	public void ShootBullet(Bullet bullet)
	{
		// Maybe something like bullet.ShootPrefab?
	}
	
	
	public void AddBullet(int slotIndex, Bullet bulletType)
	{
		if(_equippedBullets[slotIndex] == null)
		{
			_equippedBullets[slotIndex] = bulletType;
		}
	}
	
}
