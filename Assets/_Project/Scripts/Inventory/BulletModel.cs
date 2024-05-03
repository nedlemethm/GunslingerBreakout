using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BulletModel
{
	public event Action OnInventoryUpdate;
	public event Action OnChamberUpdate;
	
	// NOTE: Use an array and track the bullet shots with an index that is probably a lot easier
	private BulletObject[] _chamber= new BulletObject[_maxShots];
	private BulletObject[] _inventory = new BulletObject[_inventorySize];
	private static int _maxShots = 6;
	private static int _inventorySize = 5;
	private int _shotIndex;
	
	public BulletObject[] ChamberBullets => _chamber;
	public BulletObject[] InventoryBullets => _inventory;
	
	public BulletModel()
	{
		
	}
	
	public void FireBullet()
	{
		// Fire Bullet!
		
		_shotIndex++; // Need to make this loop back around
		
		OnChamberUpdate?.Invoke();
	}
	
	public void AddBulletToInventory(BulletObject bullet)
	{
		for (int i = 0; i < _inventorySize; i++)
		{
			if(_inventory[i] == null) // If Slot is null, that means it is empty and it should be populated with the bullet
			{
				_inventory[i] = bullet;
				OnInventoryUpdate?.Invoke();
				return;
			}
		}
		
		// Inventory is full if this line is reached
	}
	
	public void AddBulletToChamber(int inventoryIndex, int chamberIndex)
	{
		BulletObject bullet = _inventory[inventoryIndex];
		
		_chamber[chamberIndex] = bullet;
		
		OnChamberUpdate?.Invoke();
	}
	
	public void SwapBullets(int chamberIndex1, int chamberIndex2)
	{
		BulletObject bullet1 = _chamber[chamberIndex1];
		BulletObject bullet2 = _chamber[chamberIndex2];
		BulletObject temp = bullet1;
		
		_chamber[chamberIndex1] = bullet2;
		_chamber[chamberIndex2] = temp;
		
		OnChamberUpdate?.Invoke();
	}
	
	public void RemoveBulletFromChamber(int chamberIndex)
	{
		BulletObject bullet = _chamber[chamberIndex];
		
		// NOTE: Need to add if inventory is full check later
		
		AddBulletToInventory(bullet);
	}
}
