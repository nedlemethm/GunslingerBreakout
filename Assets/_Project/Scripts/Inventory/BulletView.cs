using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletView : MonoBehaviour
{
	// Have a list of 6 bullet types to display
	
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
	
	public void Initialize()
	{
		
	}
	
	public void UpdateChamber()
	{
		
	}
	
	public void UpdateInventory()
	{
		
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
