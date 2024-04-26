using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private string pickupTag;

    private BulletObject currentBullet;
    [SerializeField] private int bulletListMaxLength = 6;
    [SerializeField] private bool presetBullets;
    [SerializeField] private List<BulletObject> bullets;

    // Start is called before the first frame update
    void Start()
    {
        //if there isn't a preset of bullets for the level, set the max amount of bullets the player can have at once
        if (!presetBullets) { bullets.Capacity = bulletListMaxLength; }
    }

    // Update is called once per frame
    void Update()
    {
        //first bullet in the list is always the loaded bullet
        if (bullets.Count > 0) { currentBullet = bullets[0]; }
        else { currentBullet = null; }
    }

    public void addBullet(BulletObject bullet)
    {
        bullets.Add(bullet);
    }

    public BulletObject getCurrentBullet()
    {
        if (bullets.Count > 0) { return bullets[0]; }
        else { return null; }
    }

    public void ReplaceCurrentBullet(BulletObject newBullet)
    {
        bullets[0] = newBullet;
    }

    public int getMaxBullets()
    {
        return bulletListMaxLength;
    }

    public int getAmountBulletsLoaded()
    {
        return bullets.Count;
    }

    public void bulletShot()
    {
        bullets.RemoveAt(0);
    }
}
