using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingGO : MonoBehaviour, IDamageable
{
    public float startHealth = 100f;
    public float health {  get; private set; } 
    public float speed { get; set; }
    public bool dead { get; private set; }
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    public void UpdateHealth(float newHp, bool newDead)
    {
        health = newHp;
        dead = newDead;
    }


    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if(health <= 0 && !dead)
        {
            OnDie();
        }
    }

    public virtual void OnDie()
    {
        if(onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
}
