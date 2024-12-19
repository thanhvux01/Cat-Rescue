using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private bool isInvincible;
    public event Action OnTakeDamage;
    public event Action OnDeath;
    private int health;

    public bool IsDead => health == 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int amount)
    {
        if (health == 0) { return; }

        if(isInvincible) {return;}
 
        health = Mathf.Max(health - amount, 0);

        if (health == 0) { OnDeath?.Invoke(); return; }
        
        Debug.Log(health);

        OnTakeDamage?.Invoke();

    }

    public void SetInvincible(bool value)
    {
        this.isInvincible = value;
    }
}
