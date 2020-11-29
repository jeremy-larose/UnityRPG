using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    //public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnCharacterDied;
    
    private int healthMax;
    private int health;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public void SetHealthAmount(int health)
    {
        this.health = health;
        OnHealthChanged?.Invoke( this, EventArgs.Empty ); 
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetHealthMax()
    {
        return healthMax;
    }

    public float GetHealthNormalized()
    {
        return (float) health / healthMax;
    }

    public void Damage(int amount)
    {

        health -= amount;
        if (health < 0)
        {
            health = 0;
        }


        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log( "Health Changed: " +amount + " current: " + health );
        //OnDamaged?.Invoke(this, EventArgs.Empty);
        if (health <= 0)
        {
            Debug.Log( " has died.");
            OnCharacterDied?.Invoke( this, EventArgs.Empty );
        }
    }

    public void Heal( int amount )
    {
        health += amount;
        if (health > healthMax)
        {
            health = healthMax;
        }
        OnHealthChanged?.Invoke( this, EventArgs.Empty);
        //OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealComplete()
    {
        health = healthMax;
        OnHealthChanged?.Invoke( this, EventArgs.Empty);
    }

    public void SetHealthMax(int healthMax, bool fullHealth)
    {
        this.healthMax = healthMax;
        if (fullHealth) health = healthMax;
        //OnHealthMaxChanged?.Invoke( this, EventArgs.Empty);
        OnHealthChanged?.Invoke( this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        if (health <= 0)
        {
            return true;
        }
        return false;
    }
}
