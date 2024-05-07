using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
	[SerializeField] private BulletObject _bullet;

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out BulletController bc))
		{
			bc.AddBulletToInventory(_bullet);
			Destroy(gameObject);
		}
	}
}
