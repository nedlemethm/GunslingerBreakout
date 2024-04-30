using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletModel
{
	private ExtendedQueue<BulletObject> _chamber;
	private List<BulletObject> _bulletInventory;
	private int _maxShots = 6;
	
	public BulletModel()
	{
		for (int i = 0; i < 6; i++)
		{
			_chamber.Enqueue(null);
		}
	}
	
	public void AddBulletToInventory(BulletObject bullet)
	{
		_bulletInventory.Add(bullet);
	}
	
	public void SwapBullets(int index1, int index2)
	{
		_chamber.Swap(index1, index2);
	}
	
	public void AddBulletToChamber(BulletObject bullet, int index)
	{
		
	}
}
