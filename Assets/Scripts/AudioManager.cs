using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Music
    public AudioSource levelMusic, gameOverMusic, winMusic, mainMenuMusic;
    public float Volume;

    // SFX
    public AudioSource[] sfx;

    private void Awake()
    {
        instance = this;
        if (PlayerPrefs.HasKey("volume"))
        {
            Volume = PlayerPrefs.GetFloat("volume", 1f);
        } else
        {
            Volume = 0.35f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelMusic.volume = Volume;
        gameOverMusic.volume = Volume;
        winMusic.volume = Volume;

        foreach(AudioSource sound in sfx)
        {
            if(Volume + 0.2f > 1f)
            {
                sound.volume = 1f;
            } else
            {
                sound.volume = Volume + 0.2f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();

        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();

        winMusic.Play();
    }

    public void PlayMainMenu()
    {
        mainMenuMusic.Stop();

        mainMenuMusic.Play();
    }

    public void PlayLevelMusic()
    {
        levelMusic.Stop();

        levelMusic.Play();
    }

    public void PlaySFX(int choice)
    {
        sfx[choice].Stop();

        sfx[choice].Play();
    }
}
