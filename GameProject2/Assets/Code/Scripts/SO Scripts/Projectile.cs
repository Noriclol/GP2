using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileType", menuName = "ScriptableObjects/Projectiles", order = 1)]
public class Projectile : ScriptableObject
{
    [Header("Resources")]
    public GameObject Prefab;
    public ParticleSystem OnDeath;
    public ParticleSystem OnSpawn;
    public ParticleSystem Muzzleflash;
    [Space]
    [Header("Modifiers")]
    public float Damage;
    public float ImpactForce;
    public float MuzzleSpeed;
    public float DragMultiplier = 1f;
    public float GravityMultiplier = 1f;
}
