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
	[SerializeField] private AnimationCurve inventoryLocationCurve;
	[SerializeField] private AnimationCurve inventoryRotationCurve;
	private Coroutine inventoryRoutine;

	[Header("Chamber Animation")]
	[SerializeField] private GameObject chamber;
	[SerializeField] private float chamberEnableTime;
	[SerializeField] private AnimationCurve chamberRotationCurve;
	private Coroutine chamberRoutine;

	private Coroutine toggleRoutine;

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
        if (toggleRoutine != null)
            StopCoroutine(toggleRoutine);

        toggleRoutine = StartCoroutine(ToggleUIAnimation(true));
    }
	
	private void OnUiDisable(ISignalParameters parameters)
	{
		if (toggleRoutine != null)
			StopCoroutine(toggleRoutine);

		toggleRoutine = StartCoroutine(ToggleUIAnimation(false));
    }
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}

	private IEnumerator ToggleUIAnimation(bool flag)
	{
        if (inventoryRoutine != null)
            StopCoroutine(inventoryRoutine);

        if (chamberRoutine != null)
            StopCoroutine(chamberRoutine);

		if (flag)
		{
            LoopThroughChildElements(true);
            inventoryRoutine = StartCoroutine(InventoryEnableAnimation());
            chamberRoutine = StartCoroutine(ChamberEnableAnimation());

			yield return inventoryRoutine;
        }
		else
		{
            inventoryRoutine = StartCoroutine(InventoryDisableAnimation());
            chamberRoutine = StartCoroutine(ChamberDisableAnimation());
			yield return inventoryRoutine;

            LoopThroughChildElements(false);
        }
    }

	//Animation Stuff
	private IEnumerator InventoryEnableAnimation()
	{
		inventory.transform.localPosition = inventoryStartLocation;
		inventory.transform.localEulerAngles = new Vector3(inventory.transform.localEulerAngles.x, inventory.transform.localEulerAngles.y, inventoryRotationCurve.Evaluate(1f));

        float currentTime = 0f;

		while (currentTime < inventoryEnableTime)
		{
			yield return null;
			currentTime += Time.deltaTime;

			inventory.transform.localPosition = Vector3.Lerp(inventoryStartLocation, inventoryEndLocation, inventoryLocationCurve.Evaluate(currentTime / inventoryEnableTime));
            inventory.transform.localEulerAngles = new Vector3(inventory.transform.localEulerAngles.x, inventory.transform.localEulerAngles.y, inventoryRotationCurve.Evaluate(currentTime / inventoryEnableTime));
        }
	}

	private IEnumerator InventoryDisableAnimation()
	{
        float currentTime = 0f;

        while (currentTime < inventoryEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            inventory.transform.localPosition = Vector3.Lerp(inventoryEndLocation, inventoryStartLocation, inventoryLocationCurve.Evaluate(1 - currentTime / inventoryEnableTime));
            inventory.transform.localEulerAngles = new Vector3(inventory.transform.localEulerAngles.x, inventory.transform.localEulerAngles.y, inventoryRotationCurve.Evaluate(1 - currentTime / inventoryEnableTime));
        }
    }

	private IEnumerator ChamberEnableAnimation()
	{
		float currentTime = 0f;

		while (currentTime < chamberEnableTime)
		{
			yield return null;
            currentTime += Time.deltaTime;

            chamber.transform.localEulerAngles = new Vector3(chamber.transform.localEulerAngles.x, chamber.transform.localEulerAngles.y, chamberRotationCurve.Evaluate(currentTime / chamberEnableTime));
        }
	}

    private IEnumerator ChamberDisableAnimation()
    {
        float currentTime = 0f;

        while (currentTime < chamberEnableTime)
        {
            yield return null;
            currentTime += Time.deltaTime;

            chamber.transform.localEulerAngles = new Vector3(chamber.transform.localEulerAngles.x, chamber.transform.localEulerAngles.y, chamberRotationCurve.Evaluate(1 - currentTime / chamberEnableTime));
        }
    }
}
