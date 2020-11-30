using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject characterScreen;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadCharacterScreen();
        }
    }
    
    private void LoadCharacterScreen()
    {
        if (characterScreen.gameObject.activeInHierarchy)
        {
            characterScreen.SetActive(false);
        }
        else
        {
            characterScreen.SetActive(true);
        }
    }
    
    
}
