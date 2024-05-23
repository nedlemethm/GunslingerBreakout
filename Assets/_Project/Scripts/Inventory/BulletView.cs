using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BulletView : MonoBehaviour
{
	[SerializeField] private List<ChamberSlotView> _chamberSlots = new();
	[SerializeField] private List<InventorySlotView> _inventorySlots = new();
	
	private BulletObject[] _chamberBullets;
	private BulletObject[] _inventoryBullets;
	private BulletController _controller;

	[SerializeField] private BulletViewAnimation bulletViewAnimation;
	[SerializeField] private Image waifuImage;

	
	private void Awake()
	{
		GameSignals.TOOLBAR_ENABLED.AddListener(OnUiEnable);
		GameSignals.TOOLBAR_DISABLED.AddListener(OnUiDisable);
	}
	
	private void OnDestroy()
	{
		GameSignals.TOOLBAR_ENABLED.RemoveListener(OnUiEnable);
		GameSignals.TOOLBAR_DISABLED.RemoveListener(OnUiDisable);
	}
	
	private void Start()
	{
		LoopThroughChildElements(false);
	}
	
	public void Initialize(BulletController controller)
	{
		_controller = controller;
		
		// Inject the controller into each slot view
		foreach (ChamberSlotView item in _chamberSlots)
		{
			item.Controller = _controller;
		}
		
		foreach (InventorySlotView item in _inventorySlots)
		{
			item.Controller = _controller;
		}
	}
	
	public void AddBulletToChamber(int inventoryIndex, int chamberIndex) // Called from player interaction with UI For Matthew
	{
		_controller.AddBulletToChamber(inventoryIndex, chamberIndex);
	}
	
	public void SwapBullets(int chamberIndex1, int chamberIndex2) // Called from player interaction with UI for Matthew
	{
		_controller.SwapBullets(chamberIndex1, chamberIndex2);
	}
	
	public void RemoveBulletFromChamber(int chamberIndex) // Called from player interaction with UI for Matthew
	{
		_controller.RemoveBulletFromChamber(chamberIndex);
	}
	
	public void UpdateChamberView(BulletObject[] _chamberBulletsToView)
	{
		_chamberBullets = _chamberBulletsToView;
		
		// View stuff
		for (int i = 0; i < _chamberBullets.Length; i++)
		{
			ChamberSlotView view = _chamberSlots.ElementAt(i);
			view.UpdateView(_chamberBullets[i]);
		}
	}
	
	public void UpdateInventoryView(BulletObject[] _inventoryBulletsToView)
	{
		_inventoryBullets = _inventoryBulletsToView;
		
		// View stuff
		for (int i = 0; i < _inventoryBullets.Length; i++)
		{
			InventorySlotView view = _inventorySlots.ElementAt(i);
			Debug.Log(_inventoryBullets[i]);
			view.UpdateView(_inventoryBullets[i]);
		}
	}
	
	private void OnUiEnable(ISignalParameters parameters)
	{
		bulletViewAnimation.OnUiEnable();
    }
	
	private void OnUiDisable(ISignalParameters parameters)
	{
        bulletViewAnimation.OnUiDisable();
    }
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}

	public void RotateChamber(int chamberIndex)
	{
		bulletViewAnimation.RotateChamber(chamberIndex);
    }

	public void UpdateWaifu(int chamberIndex)
	{
		if (_chamberBullets[chamberIndex] != null)
		{
			waifuImage.sprite = _chamberBullets[chamberIndex].artwork;
			waifuImage.color = Color.white;
		}
		else
		{
			waifuImage.sprite = null;
            waifuImage.color = Color.clear;
        }
	}

    //Animations
    
}
