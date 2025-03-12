using System.Collections;
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


    public void Start(){
        managementData.SetAudioMixerData();
    }
    public void ChangeSceneSelector(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.HomeScene:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.OptionsScene:
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;
            case TypeScene.EscenaInicio:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.Exit:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.NextLevel:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
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
        yield return new WaitForSecondsRealtime(2);

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
        yield return new WaitForSecondsRealtime(2);
        OpenCloseScene.SetBool("Out", false);
        StartCoroutine(FadeIn());

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
