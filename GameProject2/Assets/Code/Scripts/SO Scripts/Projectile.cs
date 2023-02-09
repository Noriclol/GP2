using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileType", menuName = "ScriptableObjects/Projectiles", order = 1)]
public class Projectile : ScriptableObject
{
    [Space]
    [Header("Resources")]
    public GameObject Prefab;
    public ParticleSystem OnDeath;
    public ParticleSystem OnSpawn;
    public ParticleSystem Muzzleflash;
    [Space]
    [Header("Stats")]
    public float Damage;
    public float ImpactForce;
    public float MuzzleSpeed;
    [Header("Modifiers")]
    [Space]
    public float DragMultiplier = 1f;
    public float GravityMultiplier = 1f;
}
