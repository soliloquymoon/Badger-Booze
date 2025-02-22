using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private Scene currScene;
    


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        } else {
            //Destroy(gameObject);
        }
    }

    private void Start() {
        currScene = SceneManager.GetActiveScene();

        if(currScene.name == "TitleScene") {
            PlayMusic("TitleTheme");
        }

        if(currScene.name == "GameScene") {
            PlayMusic("InGameTheme");
        }
        
    }


    public void PlayMusic(string name) {
        Sound s = Array.Find(musicSounds, x=> x.name == name);

        if (s == null) {
            Debug.Log("Sound Not Found");
        } else {
            musicSource.clip = s.clip;
            musicSource.Play();
        }

    } 


    public void PlaySFX(string name) {
        Sound s = Array.Find(sfxSounds, x=> x.name == name);

        if (s == null) {
            Debug.Log("Sound Not Found");
            Debug.Log(name);
        } else {
            sfxSource.PlayOneShot(s.clip);
        }

    }

    public void ToggleMusic() {
        musicSource.mute = !musicSource.mute;
    } 

    public void ToggleSFX() {
        sfxSource.mute = !sfxSource.mute;
    }


    public void MusicVolume(float volume) {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume) {
        sfxSource.volume = volume;
    }
}
