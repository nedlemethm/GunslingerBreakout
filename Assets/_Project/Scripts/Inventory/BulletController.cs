using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This controller will link internal bullet selection
public class BulletController : MonoBehaviour
{
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
		_bulletView.Initialize();
	}
	
	private void FireBullet(InputAction.CallbackContext context) // When the player Fires a Bullet
	{
		_bulletModel.FireBullet();
	}
	
	private void ToggleToolbar(InputAction.CallbackContext context)
	{
		_toolbarEnabled = !_toolbarEnabled;
		if(_toolbarEnabled)
		{
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
		_bulletView.UpdateChamber();
	}
	
	private void OnInventoryUpdate()
	{
		_bulletView.UpdateInventory();
	}
}
