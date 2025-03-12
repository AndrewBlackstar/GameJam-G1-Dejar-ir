using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public Sounds[] musicSound, SfxSound;
    public AudioSource  musicSource, sfxSource;
    public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        PlayMusic("BGMusic");
    }

    public void PlayMusic(String name)
    {
        Sounds s = Array.Find(musicSound, x => x.name == name);
        if (s == null)
        {
            Debug.Log("musica no encontrado");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySfx (string name)
    {
        Sounds s = Array.Find(SfxSound, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sfx no encontrado");
        }

        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }

    // Métodos para ajustar volumen
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }



}
