using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This scene will eventually proc the minigames and handle the health. Might be a connector between the beats of the main music manager and the bopping of the characters
/// This scene will also draw the lightbulbs to easily access them.
/// </summary>
public class MainSceneManager : MonoBehaviour
{
    public Sprite lightOn;
    public GameObject lightbulb;
    public Vector3 lightbulbStartPos;
    public float minCooldownMinigame, maxCooldownMinigame;
    public SpriteRenderer cloudYellow, cloudBlue, cracked1,cracked2;
    public float timeToMinigame;
    public Animator alarm;
    public AudioSource alarmSound;

    private List<GameObject> lightbulbs = new List<GameObject>();
    private float minigameTimer, minigameActivationTimer;
    private bool minigameReady;
    private int friendInNeed;


    // Start is called before the first frame update
    void Start()
    {
        CreateLightbulb(0.25f);

        minigameTimer = Random.Range(minCooldownMinigame, maxCooldownMinigame);
    }

    void Awake()
    {
        ErrorState(FrogForceManager.instance.errors);

    }

    // Update is called once per frame
    void Update()
    {
        if (!minigameReady)
        {
            minigameTimer -= Time.deltaTime;
            if (minigameTimer < 0)
            {
                //Spawn cloud and enable scanning for input 
                PrepMinigame();
            }
        }

        if (minigameReady)
        {
            minigameActivationTimer -= Time.deltaTime;
            if (minigameActivationTimer < 0)
            {
                //Error failed minigame, up state of damage of ship
                FrogForceManager.instance.errors += 1;
                ErrorState(FrogForceManager.instance.errors);

                minigameReady = false;
                minigameTimer = Random.Range(minCooldownMinigame, maxCooldownMinigame);

                cloudBlue.enabled = false;
                cloudYellow.enabled = false;
            }
            else
            {
                if (friendInNeed == 0 && Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    //Activate minigame for yellow
                    FrogForceManager.instance.friendInNeed = 0;
                    //Manual go to next scene. Move to manager with transition etc.
                    FrogForceManager.instance.TransitionScene("ThrowHands");
                }
                else if (friendInNeed == 1 && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //Activate minigame for blue
                    FrogForceManager.instance.friendInNeed = 1;
                    FrogForceManager.instance.TransitionScene("ThrowHands");
                }
            }
        }

    }

    private void ErrorState(int errors)
    {
        if (errors == 1)
        {
            alarm.SetTrigger("Alarm");
            alarmSound.volume = 0.3f;
            cracked1.enabled = true;
        }
        else if (errors == 2)
        {
            alarm.SetTrigger("AlarmIntense");
            alarmSound.volume = 0.7f;
            cracked1.enabled = false;
            cracked2.enabled = true;
        }
        else if (errors >= 3)
        {
            FrogForceManager.instance.TransitionScene("DefeatScreen");
        }
    }

    /// <summary>
    /// This will pick who, what and enables the input to start the minigame
    /// </summary>
    private void PrepMinigame()
    {
        //Pick who will supose the minigame, eventually what minigame
        friendInNeed = Random.Range(0, 2);
        if (friendInNeed == 0)
        {
            cloudYellow.enabled = true;
        }
        else
        {
            cloudBlue.enabled = true;
        }

        minigameReady = true;
        minigameActivationTimer = timeToMinigame;
        //Enable the popup and read input
    }

    private void CreateLightbulb(float distance)
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject light = Instantiate(lightbulb, new Vector3(lightbulbStartPos.x + (distance * i), lightbulbStartPos.y, 0), Quaternion.identity, GameObject.Find("Ship").transform);
            if (FrogForceManager.instance.score >= i + 1)
                light.GetComponent<SpriteRenderer>().sprite = lightOn;
            lightbulbs.Add(light);
        }
    }

}
