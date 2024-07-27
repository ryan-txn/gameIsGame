using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSoundsList1, musicSoundsList2, sfxSounds;
    private enum MusicList { List1, List2 }// Enum to keep track of the currently active music list
    private MusicList currentMusicList = MusicList.List1;

    private Sound[] activeMusicSounds; // Array based on the selected music list

    private void Awake()
    {
        //make sound sliders and settings from audiomanager reflect to actual audiosource
        InitializeMusic();
        InitializeSFX();

        //make the active list to be list 1 initially
        activeMusicSounds = musicSoundsList1;

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
    
    //Initialise both music lists
    private void InitializeMusic()
    {
        foreach (Sound s in musicSoundsList1)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = true;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        foreach (Sound s in musicSoundsList2)
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
        Sound sound = Array.Find(activeMusicSounds, x => x.name == musicName);

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

    //Stops all music
    public void StopMusic()
    {
        foreach(Sound s in activeMusicSounds)
        {
            if(s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    //Pauses that specific music track
    public void PauseMusic(string musicName)
    {
        Sound sound = Array.Find(activeMusicSounds, x => x.name == musicName);

        if (sound == null)
        {
            Debug.Log("'" + musicName + "' music not found");
        }
        else
        {
            sound.source.Pause();
        }
    }

    // Fades out music track then stops it
    public void FadeOutMusic(string musicName, float duration)
    {
        Sound sound = Array.Find(activeMusicSounds, x => x.name == musicName);

        if (sound == null)
        {
            Debug.Log("'" + musicName + "' music not found");
        }
        else
        {
            StartCoroutine(FadeOutCoroutine(sound.source, duration));
        }
    }

    // Coroutine to fade out the volume over time
    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset the volume for future use
    }

    // Method to switch to music list 1
    public void SelectMusicList1()
    {
        if (currentMusicList != MusicList.List1)
        {
            StopMusic(); // Stop any currently playing music
            currentMusicList = MusicList.List1;
            activeMusicSounds = musicSoundsList1; // Switch the active list
            Debug.Log("Switched to Music List 1");
        }
    }

    // Method to switch to music list 2
    public void SelectMusicList2()
    {
        if (currentMusicList != MusicList.List2)
        {
            StopMusic(); // Stop any currently playing music
            currentMusicList = MusicList.List2;
            activeMusicSounds = musicSoundsList2; // Switch the active list
            Debug.Log("Switched to Music List 2");
        }
    }

    public string GetCurrentList()
    {
        if (currentMusicList == MusicList.List2)
        {
            return "PLAYLIST 2";
        }
        return "PLAYLIST 1";
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
                FadeOutMusic("Level 1 music", 3);
                //Play music method is used in ActivateBoss method in bossmovement script, assigned to DetectPlayerEntry Event
                //FadeOutMusic method is used in FadeBossMusic method in bossmovement script, assigned to DetectPlayerEntry Event
                break;

            case "Level 2-1":
                PlayMusic("Level 2 music");
                break;
            
            case "Boss 2":
                FadeOutMusic("Level 2 music", 3);
                //Play music method is used in ActivateBoss method in jamrangedmovement script, assigned to DetectPlayerEntry Event
                //FadeOutMusic method is used in FadeBossMusic method in boss2roomcontroller script, assigned to DetectPlayerEntry Event
                break;
  
            default:
                //PlayMusic("DefaultTheme"); 
                break;
        }
    }
}

