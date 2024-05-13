using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BulletView : MonoBehaviour
{
	[SerializeField] private List<ChamberSlotView> _chamberSlots = new();
	[SerializeField] private List<InventorySlotView> _inventorySlots = new();
	
	private BulletObject[] _chamberBullets;
	private BulletObject[] _inventoryBullets;
	private BulletController _controller;

	[Header("Animation Variables")]

	[Header("Inventory Animation")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private float inventoryEnableTime;
	[SerializeField] private Vector3 inventoryStartLocation;
	[SerializeField] private Vector3 inventoryEndLocation;
	[SerializeField] private Vector3 inventoryStartRotation;
	[SerializeField] private Vector3 inventoryEndRotation;
	private Coroutine inventoryRoutine;

	[Header("Chamber Animation")]
	[SerializeField] private GameObject chamber;
	[SerializeField] private float chamberEnableTime;
	[SerializeField] private Vector3 chamberStartRotation;
	[SerializeField] private Vector3 chamberEndRotation;
	private Coroutine chamberRoutine;
	
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
		if (inventoryRoutine != null)
			StopCoroutine(inventoryRoutine);

		if (chamberRoutine != null)
			StopCoroutine(chamberRoutine);

		inventoryRoutine = StartCoroutine(InventoryEnableAnimation());
		chamberRoutine = StartCoroutine(ChamberEnableAnimation());
	}
	
	private void OnUiDisable(ISignalParameters parameters)
	{
        if (inventoryRoutine != null)
            StopCoroutine(inventoryRoutine);

        if (chamberRoutine != null)
            StopCoroutine(chamberRoutine);

        inventoryRoutine = StartCoroutine(InventoryDisableAnimation());
        chamberRoutine = StartCoroutine(ChamberDisableAnimation());
    }
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}

	//Animation Stuff
	private IEnumerator InventoryEnableAnimation()
	{
		inventory.transform.localPosition = inventoryStartLocation;
		inventory.transform.localEulerAngles = inventoryStartRotation;

		//These should ideally be handled via a wrapper coroutine but no time to do that
        LoopThroughChildElements(true);

        float currentTime = 0f;

		while (currentTime < inventoryEnableTime)
		{
			yield return null;
			currentTime += Time.deltaTime;

			inventory.transform.localPosition = Vector3.Lerp(inventoryStartLocation, inventoryEndLocation, currentTime / inventoryEnableTime);
			inventory.transform.localEulerAngles = Vector3.Lerp(inventoryStartRotation, inventoryEndRotation, currentTime / inventoryEnableTime);
		}
	}

	private IEnumerator InventoryDisableAnimation()
	{
        float currentTime = 0f;

        while (currentTime < inventoryEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            inventory.transform.localPosition = Vector3.Lerp(inventoryEndLocation, inventoryStartLocation, currentTime / inventoryEnableTime);
            inventory.transform.localEulerAngles = Vector3.Lerp(inventoryEndRotation, inventoryStartRotation, currentTime / inventoryEnableTime);
        }

        //These should ideally be handled via a wrapper coroutine but no time to do that
        LoopThroughChildElements(false);
    }

	private IEnumerator ChamberEnableAnimation()
	{
		float currentTime = 0f;

		while (currentTime < chamberEnableTime)
		{
			yield return null;
            currentTime += Time.deltaTime;

			chamber.transform.localEulerAngles = Vector3.Lerp(chamberStartRotation, chamberEndRotation, currentTime / inventoryEnableTime);
        }
	}

    private IEnumerator ChamberDisableAnimation()
    {
        float currentTime = 0f;

        while (currentTime < chamberEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            chamber.transform.localEulerAngles = Vector3.Lerp(chamberEndRotation, chamberStartRotation, currentTime / inventoryEnableTime);
        }
    }
}
