using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private KeyCode primaryFire = KeyCode.Mouse0;
    [SerializeField] private KeyCode secondaryFire = KeyCode.Mouse1;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform revolverBarrel;
    [SerializeField] private Camera cam;
    [SerializeField] private bool shotDelay;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private LineRenderer laser;

    private Bullet loadedBullet = null;
    private BulletBase currentBullet;
    private bool canShoot;
    private List<Bullet> secondaryFireQueue;

    void Awake()
    {
       canShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (loadedBullet.showLaser)
        //{
        //    laser.SetPositions();
        //}

        if (player.GetComponent<PlayerInventory>().getMaxBullets() > 0)
        {
            loadedBullet = player.GetComponent<PlayerInventory>().getCurrentBullet();
            canShoot = true;
        }

        if (Input.GetKeyDown(primaryFire) && canShoot)
        {
            //if (loadedBullet.remoteSecondaryFire) { secondaryFireQueue.Add(loadedBullet); }
            PrimaryFire();
        }

    }

    private void PrimaryFire()
    {
        //finds center of the screen -> might change later to match with crosshair
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(float.MaxValue); //player is pointing in the air
        }

        //calc direction from revolverBarrel to targetPoint
        Vector3 direction = targetPoint - revolverBarrel.position;
        SpawnBullet();
        //currentBullet.transform.forward = direction;

        //add forces to bullet
        currentBullet.OnShoot(direction.normalized, loadedBullet.bulletSpeed);

        if (shotDelay)
        {
            Invoke("ShotDelay", timeBetweenShots);
            canShoot = false;
        }
    }

    private void ShotDelay()
    {
        shotDelay = true;
        canShoot = true;
    }

    private BulletBase SpawnBullet()
    {
        currentBullet = Instantiate(loadedBullet.model, revolverBarrel.position, revolverBarrel.rotation, null).GetComponent<BulletBase>();
        return currentBullet;
    }
}
