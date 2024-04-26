using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private BulletObject bullet;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag(player.tag) &&
        //     (player.GetComponent<PlayerInventory>().getAmountBulletsLoaded() <= player.GetComponent<PlayerInventory>().getMaxBullets()))
        // {
        //     player.GetComponent<PlayerInventory>().ReplaceCurrentBullet(bullet);
        //     Debug.Log(bullet.name + "Picked Up");
        // }
    }
}
