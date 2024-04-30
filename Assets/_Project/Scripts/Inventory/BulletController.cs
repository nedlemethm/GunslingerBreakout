using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This controller will link internal bullet selection
public class BulletController : MonoBehaviour
{
	[SerializeField] private int _inventorySize = 5;
	
	private BulletModel _bulletModel;
	private BulletView _bulletView;
	private PlayerControls _playerInput;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Fire.started += FireAction;
		_playerInput.Enable();
		
		_bulletModel = new();
		_bulletView = FindObjectOfType<BulletView>();
		_bulletView.Initialize();
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
	}
	
	private void FireAction(InputAction.CallbackContext context) // When the player Fires a Bullet
	{
		
	}
	
	public void CollectBullet(BulletObject bullet) // Used for picking up bullets and dragging bullets from chamber back into inverntory
	{
		_bulletModel.AddBulletToInventory(bullet);
	}
	
	public void LoadBullet() // Used for moving bullets into chamber
	{
		
	}
	
	public void SwapBulletOrder() // Used for swapping bullets in the chamber
	{
		
	}
}
