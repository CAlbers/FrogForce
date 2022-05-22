using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrogForceManager : MonoBehaviour
{
    public AudioSource mainAudio;
    public AudioClip mainTheme, gameplayTheme, losingTheme;

    public static FrogForceManager instance = null;
    public int friendInNeed;
    public int score = 0;
    public int errors = 0;

    [SerializeField]
    private Animator anim;
    private string nextScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(base.gameObject);
        }
    }

    /// <summary>
    /// We subscribe to the SceneLoaded event so we can switch music and such
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Be nice and always clean up your listeners ;)
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        anim.SetTrigger("TransitionScreen");

        if (scene.name == "MainMenu")
        {
            StartCoroutine(TransitionMusic(mainTheme));
        }
        else if (scene.name == "GameScene")
        {
            StartCoroutine(TransitionMusic(gameplayTheme));
            //mainAudio.clip = gameplayTheme;
            //mainAudio.Play();
        }
        else if (scene.name == "ThrowHands")
        {
            StartCoroutine(MuteMusic());
        }
        else if (scene.name == "WinScreen")
        {
            StartCoroutine(TransitionMusic(mainTheme));
        }
        else if (scene.name == "DefeatScreen")
        {
            StartCoroutine(TransitionMusic(losingTheme));
        }

    }

    IEnumerator TransitionMusic(AudioClip newAudio)
    {

            while (mainAudio.volume > 0)
            {
                mainAudio.volume -= 0.1f;
                yield return new WaitForSeconds(0.02f);
            }
            mainAudio.clip = newAudio;
            mainAudio.Play();
            while (mainAudio.volume != 1)
            {
                mainAudio.volume += 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
    }

    IEnumerator MuteMusic()
    {
        while (mainAudio.volume > 0)
        {
            mainAudio.volume -= 0.1f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void TransitionScene(string sceneName)
    {
        nextScene = sceneName;
        if (score == 6)
        {
            nextScene = "WinScreen";
        }
        else if (errors == 3)
        {
            nextScene = "DefeatScreen";
        }
        anim.SetTrigger("TransitionBlack");
        //Start Animation, when black, queue transition. then on load start animation
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
        nextScene = null;
    }

    public void Reset()
    {
        errors = 0;
        score = 0;
    }
}
