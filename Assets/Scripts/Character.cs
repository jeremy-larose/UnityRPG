using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance { get; private set; }
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
        Instance = this;
        healthSystem = new HealthSystem( Dice.Roll( 4, 8 ));
        currentHP = healthSystem.GetHealth();
        TimeSystem.OnTick_5 += RegenHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void RegenHealth(object sender, TimeSystem.OnTickEventArgs e)
    {
        Debug.Log( "Character: RegenHealth: Player->CurrentHP +5." );
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
        CombatText.Create(GetPosition(), damageTotal, isCriticalHit, new Color32( 255, 128, 0, 255 ) );

        if (healthSystem.GetHealth() <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int healing)
    {
        currentHP += healing;
        OnHealthChanged?.Invoke( this, EventArgs.Empty );
        CombatText.Create(GetPosition() + new Vector3( 0, .5f, .5f ), 5, false, Color.green );

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
