using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControlScript : MonoBehaviour
{
    public static GlobalControlScript Instance;
    public Sounds[] sounds;

    Vector2[] slideProfile = {new Vector2(0,-960),new Vector2(0,-960)};

    public float l1, l2, l3, l4, l5, l6;

    public int sProfile = 0;
    private void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        foreach (Sounds s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isMusic;
        }
    }

    //[SETTINGS]
    public float musicVol, sfxVol;
    public bool mute, pause;

    private void Start() {
        musicVol = 0.2f;
        sfxVol = 0.75f;
        mute = false;
        UpdateAudio();
        //PlaySound("MAIN_MENU");
    }
    public void UpdateAudio() {
        foreach (Sounds s in sounds) {
            if (s.isMusic) s.source.volume = musicVol;
            else s.source.volume = sfxVol;
            s.source.mute = mute;
        }
    }
    public void PlaySound(string name) {
        print(name);
        Sounds s = Array.Find(sounds, s => s.name == name);
        if(s==null){
            print("Unable to find sound : "+name);
            return;
        }else if (!s.source.isPlaying) { 
            if (s.isMusic) StopMusic();
            s.source.Stop();
            s.source.Play();
        }if(s.source.isPlaying&&!s.isMusic){
            s.source.Stop();
            s.source.Play();
        }
    }

    void StopMusic() {
        foreach(Sounds s in sounds) {
            if (s.isMusic && s.source.isPlaying) s.source.Stop();
        }
        
    }

    public Vector2 GetSProfile(){
        return slideProfile[sProfile];
    }

    public void Pause(){
        pause=!pause;
        Time.timeScale = pause? 0 : 1;
        PlaySound(pause?"PAUSE":"UNPAUSE");
        GameObject.Find("Canvas").GetComponent<UIBehaviour>().SetPauseTransform(pause);
    }
}
