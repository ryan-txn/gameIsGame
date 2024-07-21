using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;

    private void Awake()
    {
        //make sound sliders and settings from audiomanager reflect to actual audiosource
        InitializeMusic();
        InitializeSFX();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; //add to sceneLoaded unityevent
        }
        else
        {
            Destroy(gameObject); //destroy extra audiomanager if present
            return;
        }
    }

    private void InitializeMusic()
    {
        foreach (Sound s in musicSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = true;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void InitializeSFX()
    {
        foreach (Sound s in sfxSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void PlayMusic(string musicName)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == musicName);

        if (sound == null)
        {
            Debug.Log("'" + musicName + "' music not found");
        }
        else
        {
            sound.source.Play();
        }
    }

    public void PlaySFX(string sfxName)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == sfxName);

        if (sound == null)
        {
            Debug.Log("'" + sfxName + "' sound not found");
        }
        else
        {
            sound.source.PlayOneShot(sound.clip);
        }
    }

    public void StopMusic()
    {
        foreach(Sound s in musicSounds)
        {
            if(s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    public void PauseMusic(string musicName)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == musicName);

        if (sound == null)
        {
            Debug.Log("'" + musicName + "' music not found");
        }
        else
        {
            sound.source.Pause();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // unsubscribe from unity event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                StopMusic();
                PlaySFX("Title");
                break;

            case "Lobby":
                PlayMusic("Lobby music");
                break;

            case "Level 1-1":
                StopMusic();
                PlayMusic("Level 1 music");
                break;

            case "Boss 1":
                StopMusic();
                PlayMusic("Boss 1 music");
                break;
  
            default:
                //PlayMusic("DefaultTheme"); 
                break;
        }
    }
}

