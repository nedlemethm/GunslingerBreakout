using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChecker : MonoBehaviour
{
    private int iceCount = 0;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private PhysicMaterial icyMat;
    [SerializeField] private PhysicMaterial defaultMat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("IceFloor"))
        {
            iceCount++;
            if (iceCount >= 1)
            {
                playerController.SetInIce(true);
                Debug.Log("setting mat");
                playerCollider.material = icyMat;
                Debug.Log("mat set on " + playerCollider.name + " to " + icyMat.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("IceFloor"))
        {
            iceCount--;
            if (iceCount <= 0)
            {
                playerController.SetInIce(false);
                Debug.Log("setting mat");
                playerCollider.material = defaultMat;
                Debug.Log("mat set on " + playerCollider.name + " to " + defaultMat.name);
            }
        }
    }
}
