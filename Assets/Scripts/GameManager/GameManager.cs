﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager GameInstance { get; private set; }

    public ManagementData managementData;
    public Animator OpenCloseScene;


    public bool isPaused;

    //public event Action OnWin;
    //public event Action OnLose;



    private void Awake()
    {
        if (GameInstance == null)
        {
            GameInstance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        managementData.SetAudioMixerData();

        if (OpenCloseScene == null)
        {
            Debug.LogError("El Animator OpenCloseScene no está asignado en GameManager.");
        }
        else
        {
            OpenCloseScene.gameObject.SetActive(true); // 🔥 Asegurar que esté activo
            OpenCloseScene.Update(0); // 🔥 Forzar actualización en WebGL
        }
    }

    public void ChangeSceneSelector(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.HomeScene:
            case TypeScene.EscenaInicio:
            case TypeScene.Exit:
                StartCoroutine(TransitionScene(typeScene));
                break;

            case TypeScene.OptionsScene:
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;

            case TypeScene.NextLevel:
                StartCoroutine(NextLevel());
                break;
        }
    }



    public IEnumerator FadeIn()
    {
        float decibelsMaster = Mathf.Lerp(-80f, 0f, ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100f);
        float currentVolumen = -80f;

        if (ManagementData.audioMixer.GetFloat(ManagementOptions.TypeSound.Master.ToString(), out float volume))
        {
            currentVolumen = volume;
        }

        while (currentVolumen < decibelsMaster)
        {
            if (ManagementData.saveData.configurationsInfo.soundConfiguration.isMute) break;
            currentVolumen += 1;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public IEnumerator ChangeScene(TypeScene typeScene)
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(1);

        if (typeScene != TypeScene.Exit)
        {
            SceneManager.LoadScene(typeScene.ToString());
        }
        else
        {
            Application.Quit();
        }

        yield return new WaitForSecondsRealtime(0.5f); // Espera medio segundo antes de restaurar volumen
        StartCoroutine(FadeIn()); // 🔥 Ahora se ejecuta justo después del cambio de escena
    }

    public IEnumerator NextLevel()
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        OpenCloseScene.SetBool("Out", false);
        StartCoroutine(FadeIn());

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator TransitionScene(TypeScene typeScene)
    {
        if (OpenCloseScene == null)
        {
            Debug.LogError("El Animator OpenCloseScene no está asignado.");
            yield break;
        }

        OpenCloseScene.SetBool("Out", true); // 🔥 Inicia Fade Out
        yield return new WaitForSecondsRealtime(1f); // Esperar la animación de salida

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(typeScene.ToString());
        yield return new WaitUntil(() => asyncLoad.isDone); // Esperar carga completa

        OpenCloseScene.gameObject.SetActive(true); // 🔥 Asegurar que esté activo
        OpenCloseScene.Update(0); // 🔥 Forzar actualización del Animator en WebGL

        yield return new WaitForSecondsRealtime(0.1f); // Esperar un frame antes de Fade In

        OpenCloseScene.SetBool("Out", false); // 🔥 Activar Fade In
    }

    public IEnumerator FadeOut()
{
    float decibelsMaster = Mathf.Lerp(-80f, 0f, ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100f);
    
    while (decibelsMaster > -80)
    {
        if (ManagementData.saveData.configurationsInfo.soundConfiguration.isMute) break;
        decibelsMaster -= 1;
        yield return new WaitForSecondsRealtime(0.05f);
    }
}
    public void PlayASound(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("Intentando reproducir un sonido que no está cargado.");
            return;
        }

        StartCoroutine(WaitForSoundLoad(audioClip));
    }

    private IEnumerator WaitForSoundLoad(AudioClip audioClip)
    {
        while (audioClip.loadState == AudioDataLoadState.Loading)
        {
            yield return null;  // Espera hasta que el sonido esté completamente cargado
        }

        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }

    public void PlayASound(AudioClip audioClip, float initialRandomPitch)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.pitch = Random.Range(initialRandomPitch - 0.1f, initialRandomPitch + 0.1f);
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }

    internal void SetAudioMixerData()
    {
        managementData.SetAudioMixerData();
    }

    public enum TypeScene
    {
        HomeScene = 0,
        OptionsScene = 1,
        EscenaInicio = 2,
        Exit = 3,
        NextLevel = 4
    }

    public void GameOverWin()
    {
        Debug.Log("You win!");
        AudioManager.Instance.PlaySfx("GameWin");
        ChangeSceneSelector(TypeScene.NextLevel);
    }

    public void GameOverLose()
    {
        Debug.Log("You lose!");
        AudioManager.Instance.PlaySfx("GameOver");
        ChangeSceneSelector(TypeScene.HomeScene);
    }
}
