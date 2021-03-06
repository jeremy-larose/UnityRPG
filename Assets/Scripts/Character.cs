﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance { get; private set; }
    public HealthSystem healthSystem;
    public int currentHP { get; private set; }
    public string charName;
    public int maxHP = 20;
    public float attackSpeed;
    public Dictionary<CharacterFlags, bool> Traits = new Dictionary<CharacterFlags, bool>();


    public delegate void HealthChanged();
    public static event HealthChanged OnHealthChanged;

    public enum CharacterFlags
    {
        Pacifist,
        Energetic,
        Sluggish
    }
    
    private enum State
    {
        Normal,
        Busy
    }

    private void Awake()
    {
        Instance = this;
        healthSystem = new HealthSystem( Dice.Roll( 4, 8 ), 50 );
        currentHP = healthSystem.GetHealth();
        TimeSystem.OnTick_5 += RegenHealth;
        charName = "Jerekai";
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Traits.Add(CharacterFlags.Pacifist, true);
        }
    }

    private void RegenHealth(object sender, TimeSystem.OnTickEventArgs e)
    {
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
        OnHealthChanged?.Invoke();
        Debug.Log( $"{charName} take {damageTotal} points of damage." );

        CombatText.Create(GetPosition() + new Vector3( 0, .5f, .5f ), damageTotal, isCriticalHit, 
            new Color32( 255, 128, 0, 255 ) );

        if (healthSystem.GetHealth() <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            Debug.Log( $"Character: Taking damage from {other}!");
        }
    }

    private void AddHealth(int healing)
    {
        healthSystem.Heal( healing );
        OnHealthChanged?.Invoke();
        Debug.Log( $"{charName} heals for {healing}." );
        CombatText.Create(GetPosition() + new Vector3( 0, .5f, .5f ), 5, false, Color.green );

        if (currentHP >= maxHP)
            currentHP = maxHP;
    }

    public int GetHealth()
    {
        return healthSystem.GetHealth();
    }

    public int GetHealthMax()
    {
        return healthSystem.GetHealthMax();
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
