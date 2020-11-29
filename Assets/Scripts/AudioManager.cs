using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffect;
    public event EventHandler<OnChangeBackgroundMusicArgs> OnChangeBackgroundMusic;

    public class OnChangeBackgroundMusicArgs : EventArgs
    {
        public AudioClip bgMusic;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnChangeBackgroundMusic?.Invoke( this, new OnChangeBackgroundMusicArgs { bgMusic = GameAssets.i.townMusic } );
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            OnChangeBackgroundMusic?.Invoke( this, new OnChangeBackgroundMusicArgs{ bgMusic = GameAssets.i.overWorldMusic } );
        }
    }

    void Awake()
    {
        if( instance != null )
            Destroy( gameObject );
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic( AudioClip audioClip )
    {
        StopBGM();
        backgroundMusic.clip = audioClip;
        backgroundMusic.Play();
    }

    public void PlaySound(AudioClip sound )
    {
        soundEffect.clip = sound;
        soundEffect.Play();
    }

    private void StopBGM()
    {
        backgroundMusic.Stop();
    }
    
    
}
