using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletView : MonoBehaviour
{
	private BulletObject[] _chamberBullets;
	private BulletObject[] _inventoryBullets;
	private BulletController _controller;
	
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
		
		// get UI elements here and inject indexes into them
	}
	
	public void AddBulletToChamber() // Called from player interaction with UI
	{
		_controller.AddBulletToChamber(1, 1);
	}
	
	public void SwapBullets() // Called from player interaction with UI
	{
		_controller.SwapBullets(1, 1);
	}
	
	public void RemoveBulletFromChamber() // Called from player interaction with UI
	{
		_controller.RemoveBulletFromChamber(1);
	}
	
	public void UpdateChamberView(BulletObject[] _chamberBulletsToView)
	{
		_chamberBullets = _chamberBulletsToView;
		
		// View stuff
	}
	
	public void UpdateInventoryView(BulletObject[] _inventoryBulletsToView)
	{
		_inventoryBullets = _inventoryBulletsToView;
		
		// View stuff
	}
	
	private void OnUiEnable(ISignalParameters parameters)
	{
		LoopThroughChildElements(true);
	}
	
	private void OnUiDisable(ISignalParameters parameters)
	{
		LoopThroughChildElements(false);
	}
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}
}
