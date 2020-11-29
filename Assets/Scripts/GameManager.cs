using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Material[] skins;
    private int _skinIndex = 0;

    public List<GameObject> NPCList;

    
    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy( gameObject );
        }

        instance = this;
        DontDestroyOnLoad( gameObject );
    }
    #endregion
    
    public GameObject player;

    void Start()
    {
        player = GameObject.Find( "Player" );
        TimeSystem.OnTick_5 += delegate
        {
            player.GetComponent<PlayerBase>().healthSystem.Heal(5);
        };
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var enemy = Instantiate( enemyPrefab, transform.position, Quaternion.identity );
            enemyPrefab.GetComponentInChildren<SpriteRenderer>().material = skins[_skinIndex];
            if (_skinIndex < skins.Length-1)
                _skinIndex++; 
            else
                _skinIndex = 0;
            
            NPCList.Add( enemy );
        }
        
        if (Input.GetKeyDown(KeyCode.T ))
        {
            Debug.Log( "Taking Damage.");
            player.GetComponent<Character>().TakeDamage( Random.Range( 1, 10 ) );
        }
    }
    
    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }
}
