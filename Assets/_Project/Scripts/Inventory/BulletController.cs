using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

// This controller will link internal bullet selection
public class BulletController : MonoBehaviour
{
	[SerializeField] private Transform _bulletPoint;
	[SerializeField] private Camera _revolverCam;
	
	private BulletModel _bulletModel;
	private BulletView _bulletView;
	private PlayerControls _playerInput;
	private bool _toolbarEnabled;

	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Fire.started += FireBullet;
		_playerInput.Player.Toolbar.started += ToggleToolbar;
		
		_bulletModel = new();
	}
	
	private void OnEnable()
	{
		_bulletModel.OnChamberUpdate += OnChamberUpdate;
		_bulletModel.OnInventoryUpdate += OnInventoryUpdate;
		_playerInput.Enable();
	}
	
	private void OnDisable()
	{
		_bulletModel.OnChamberUpdate -= OnChamberUpdate;
		_bulletModel.OnInventoryUpdate -= OnInventoryUpdate;
		_playerInput.Disable();
	}
	
	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		_bulletView = FindObjectOfType<BulletView>();
		_bulletView.Initialize(this);
	}
	
	private void FireBullet(InputAction.CallbackContext context) // When the player Fires a Bullet
	{
		Debug.Log(_bulletModel.BulletToShoot);
		if(_bulletModel.BulletToShoot != null)
		{
			BulletObject bulletToShoot = _bulletModel.BulletToShoot;
			GameObject bullet = Instantiate(bulletToShoot.model, _bulletPoint.transform.position, Quaternion.identity);
			Debug.Log($"Firing {bulletToShoot.name}!");
			Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
			bulletRb.AddForce(CalcDirection() * bulletToShoot.bulletSpeed, ForceMode.VelocityChange); // Note to future self: change transform.forward into actual bullet direction
		}
		
		_bulletModel.AfterFireHandle();
	}
	
	private Vector3 CalcDirection()
	{
		Ray ray = _revolverCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		Vector3 targetPoint;
		
		if (Physics.Raycast(ray, out hit))
		{
			targetPoint = hit.point;
		}
		else
		{
			targetPoint = ray.GetPoint(float.MaxValue); //player is pointing in the air
		}
		
		Vector3 direction = targetPoint - _bulletPoint.position;
		
		return direction.normalized;
	}
	
	private void ToggleToolbar(InputAction.CallbackContext context)
	{
		_toolbarEnabled = !_toolbarEnabled;
		if(_toolbarEnabled)
		{
			// Update Views before enabling inventory
			OnChamberUpdate();
			OnInventoryUpdate();
			
			GameSignals.TOOLBAR_ENABLED.Dispatch();
		}
		else
		{
			GameSignals.TOOLBAR_DISABLED.Dispatch();
		}
	}
	
	public void AddBulletToInventory(BulletObject bullet) // Used for picking up bullets and dragging bullets from chamber back into inverntory
	{
		_bulletModel.AddBulletToInventory(bullet);
	}
	
	public void AddBulletToChamber(int inventoryIndex, int chamberIndex) // Used for moving bullets from inventory to chamber
	{
		_bulletModel.AddBulletToChamber(inventoryIndex, chamberIndex);
	}
	
	public void SwapBullets(int chamberIndex1, int chamberIndex2) // Used for swapping bullets in the chamber
	{
		_bulletModel.SwapBullets(chamberIndex1, chamberIndex2);
	}
	
	public void RemoveBulletFromChamber(int chamberIndex) // Used for taking out a bullet and putting it back in your inventory
	{
		_bulletModel.RemoveBulletFromChamber(chamberIndex);
	}
	
	private void OnChamberUpdate()
	{
		Debug.Log("Chamber Updated");
		_bulletView.UpdateChamberView(_bulletModel.ChamberBullets);
	}
	
	private void OnInventoryUpdate()
	{
		Debug.Log("Inventory Updated");
		_bulletView.UpdateInventoryView(_bulletModel.InventoryBullets);
	}
}