using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/ZombieData", fileName = "Zombie Data")]
public class ZombieData : ScriptableObject
{
    public GameObject zombieObj;
    public float health = 100f;
    public float damage = 5f;
    public float speed = 2f;

    public float probability;
}
