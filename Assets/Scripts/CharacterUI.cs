using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    private Character _character;
    
    // Start is called before the first frame update
    private void Start()
    {
        _character = GameManager.instance.player;

        nameText.text = _character.charName;
        healthText.text = $"HP: {_character.GetHealth()} / {_character.GetHealthMax()}";
        Character.OnHealthChanged += CharacterOnOnHealthChanged;
    }

    private void CharacterOnOnHealthChanged()
    {
        healthText.text = $"HP: {_character.GetHealth()} / {_character.GetHealthMax()}";
    }

    private void OnDestroy()
    {
        Character.OnHealthChanged -= CharacterOnOnHealthChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
