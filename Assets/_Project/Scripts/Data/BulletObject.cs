using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;

[CreateAssetMenu (fileName = "New Bullet", menuName = "Bullet")]
public class BulletObject : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite artwork;
    public Sprite icon;
    public GameObject model;
    public Color color;
    public VisualEffect visualEffect;
    public AudioClip gunshotSound;
 

    public float bulletSpeed;
    public bool showLaser;

   
}
