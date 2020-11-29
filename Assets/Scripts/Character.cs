using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public HealthSystem healthSystem;
    public int currentHP { get; private set; }

    public int maxHP = 20;

    public float attackSpeed;

    public event EventHandler OnHealthChanged;

    private enum State
    {
        Normal,
        Busy
    }

    private void Awake()
    {
        healthSystem = new HealthSystem( Dice.Roll( 4, 8 ));
        currentHP = healthSystem.GetHealth();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log( "Taking Damage.");
            healthSystem.Damage( 20 );
        }
    }

    private void TimeTickSystem_OnTick_5(object sender, TimeSystem.OnTickEventArgs e)
    {
        Debug.Log( "Character: Tick. Player->CurrentHP +5." );
        AddHealth(5);
    }

    public void TakeDamage(int damage)
    {
        damage = Mathf.Clamp(damage, 0, int.MaxValue); // Damage should never be negative.
        
        // Calculate damage modifiers
        bool isCriticalHit = UnityEngine.Random.Range(0, 100) < 30;
        int damageTotal = UnityEngine.Random.Range(1, damage);
        
        if (isCriticalHit)
            damageTotal *= 2;
        
        healthSystem.Damage( damageTotal );
        CombatText.Create(GetPosition(), damageTotal, isCriticalHit, "FFFFFF");

        if (healthSystem.GetHealth() <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int healing)
    {
        currentHP += healing;
        OnHealthChanged?.Invoke( this, EventArgs.Empty );

        if (currentHP >= maxHP)
            currentHP = maxHP;
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overriden.
        Debug.Log( $"[Character] {transform.name} has died.");
    }
    

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
