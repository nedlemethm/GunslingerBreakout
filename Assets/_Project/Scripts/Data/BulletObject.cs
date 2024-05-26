using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu (fileName = "New Bullet", menuName = "Bullet")]
public class BulletObject : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite artwork;
    public GameObject model;
    public Color color;
    public VisualEffect visualEffect;
    public AudioClip gunshotSound;

    public bool remoteSecondaryFire;
    public float bulletSpeed;
    public bool secondaryFireActivated;
    public bool showLaser;

}
