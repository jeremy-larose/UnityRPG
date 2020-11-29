using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    
    public static GameAssets i {
        get {
            if( _i == null ) _i = ( Instantiate( Resources.Load( "GameAssets" ) ) as GameObject ).GetComponent<GameAssets>();
            return _i;
        }
    }

    public AudioClip townMusic;
    public AudioClip overWorldMusic;

    public Transform pfCombatText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
