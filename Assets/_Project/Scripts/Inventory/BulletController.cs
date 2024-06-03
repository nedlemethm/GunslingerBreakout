using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

// This controller will link internal bullet selection
public class BulletController : MonoBehaviour
{
	[SerializeField] private Transform _bulletPoint;
    [SerializeField] private Transform _revolver;
    [SerializeField] private Camera _revolverCam;
	[SerializeField] private LineRenderer _laser;
	[SerializeField] private string _activationLayer;

    private BulletModel _bulletModel;
	private BulletView _bulletView;
	private PlayerControls _playerInput;
	private bool _toolbarEnabled;
	private int _activationLayerNum;
	private bool _controlsEnabled;

	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Fire.started += FireBullet;
		_playerInput.Player.Toolbar.started += ToggleToolbar;
		_playerInput.Player.Activation.started += Activation;
		_playerInput.UI.Pause.started += TogglePause;

		_bulletModel = new();
		_activationLayerNum = LayerMask.NameToLayer(_activationLayer);

		GameSignals.TOOLBAR_ENABLED.AddListener(DisableControls);
		GameSignals.TOOLBAR_DISABLED.AddListener(EnableControls);
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

	private void OnDestroy()
	{
		GameSignals.TOOLBAR_ENABLED.RemoveListener(DisableControls);
		GameSignals.TOOLBAR_DISABLED.RemoveListener(EnableControls);
	}

	private void EnableControls(ISignalParameters parameters)
	{
		_playerInput.Player.Fire.Enable();
		_controlsEnabled = true;
	}

	private void DisableControls(ISignalParameters parameters)
	{
		_playerInput.Player.Fire.Disable();
		_controlsEnabled = false;
	}

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		_bulletView = FindObjectOfType<BulletView>();
		_bulletView.Initialize(this);
	}

    private void Update()
    {
        if (_bulletModel.BulletToShoot != null)
		{
            if (_bulletModel.BulletToShoot.showLaser)
            {
				//Debug.Log("i love life");
				//Ray ray = _revolverCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
				Ray ray = new Ray(_bulletPoint.position, CalcDirection());
                RaycastHit hit;

                //check if ray hits something
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                    List<Vector3> points = new List<Vector3> { _bulletPoint.position + .04f * _bulletPoint.transform.up };
                    points.AddRange(Reflective.GetPoints(_bulletPoint.transform.position, targetPoint - _bulletPoint.transform.position).ToArray());
                    _laser.positionCount = points.Count;
                    _laser.SetPositions(points.ToArray());
                }
                else
                {
                    targetPoint = ray.GetPoint(50f); //player is pointing in the air
                    List<Vector3> points = new List<Vector3> { _bulletPoint.position, targetPoint };
                    _laser.positionCount = points.Count;
                    _laser.SetPositions(points.ToArray());
                }
            }
            else
            {
                _laser.positionCount = 0;
            }
        }
        else
        {
            _laser.positionCount = 0;
        }
    }

    private void FireBullet(InputAction.CallbackContext context) // When the player Fires a Bullet
	{
		Debug.Log(_bulletModel.BulletToShoot);
		if(_bulletModel.BulletToShoot != null && _controlsEnabled)
		{
			BulletObject bulletToShoot = _bulletModel.BulletToShoot;
			//Quaternion bulletSpawnPoint = Quaternion.Euler(_bulletPoint.rotation.x + 90, _bulletPoint.rotation.y, _bulletPoint.rotation.z);
			BulletBase bullet = Instantiate(bulletToShoot.model, _bulletPoint.transform.position, _bulletPoint.transform.rotation).GetComponent<BulletBase>();
			bullet.OnShoot(CalcDirection(), bulletToShoot.bulletSpeed);
			Debug.Log($"Firing {bulletToShoot.name}!");

			_bulletModel.AfterFireHandle();
		}
	}

	private void Activation(InputAction.CallbackContext context) // When the player actiavtes an activatable bullet
    {
		Ray ray = _revolverCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;
		Debug.Log("right click");
		if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == _activationLayerNum)
		{
			Debug.Log(hit.collider.gameObject);
			Activation activate = hit.collider.gameObject.GetComponent<Activation>();
			activate.ToggleActivation();
		}
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
            Cursor.lockState = CursorLockMode.Confined;
        }
		else
		{
			GameSignals.TOOLBAR_DISABLED.Dispatch();
            Cursor.lockState = CursorLockMode.Locked;
        }
	}

	private void TogglePause(InputAction.CallbackContext context)
	{
		GameSignals.PAUSE_TOGGLED.Dispatch();
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
        _bulletView.UpdateWaifu(_bulletModel.CurrentShotIndex);

        if (!_toolbarEnabled)
            _bulletView.RotateChamber(_bulletModel.CurrentShotIndex);
    }
	
	private void OnInventoryUpdate()
	{
		Debug.Log("Inventory Updated");
		_bulletView.UpdateInventoryView(_bulletModel.InventoryBullets);
	}
}