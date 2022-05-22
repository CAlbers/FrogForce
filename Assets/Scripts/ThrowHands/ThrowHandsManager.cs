using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using UnityEngine.SceneManagement;

/// <summary>
/// This class will track the main half beat so we can build/animate the other parts of the minigame based on this script.
/// 
/// </summary>
public class ThrowHandsManager : MonoBehaviour
{
    private enum HandStatus { Highfive, Fistbump, Handshake }
    private HandStatus playerHand;
    private HandStatus friendHand;
    private BeatObserver beatObserver;
    private int currentBeats;
    private bool lockInput = false;
    private bool running = false;

    public bool greenLookingLeft;

    public Transform friendHandHolder;
    public Sprite[] friendSpritesLeft;
    public Sprite[] friendSpritesRight;
    public SpriteRenderer friendSprite;
    public Animator friendAnimator;

    public Transform playerHandHolder;
    public Sprite[] playerSpritesLeft;
    public Sprite[] playerSpritesRight;
    public SpriteRenderer playerSprite;
    public Animator playerAnimator;

    public bool started = false;
    public int beatsPerChange = 2;

    public ResultAudioManager rAudio;




    void Start()
    {
        beatObserver = GetComponent<BeatObserver>();

        //Very annoying way to get random from enum and make sure it's not hardcoded
        var v = typeof(HandStatus);
        var values = v.GetEnumValues();
        int random = Random.Range(0, values.Length);
        friendHand = (HandStatus)values.GetValue(random);

        //Read greenToLeft from an overall manager that doesn't get destroyed. For now it's set manually
        if (FrogForceManager.instance.friendInNeed == 0)
        {
            greenLookingLeft = true;
        }
        else
            greenLookingLeft = false;

        if (greenLookingLeft)
            friendSprite.sprite = friendSpritesLeft[(int)friendHand];
        else
        {
            friendHandHolder.localScale = new Vector3(-1, 1, 1);
            playerHandHolder.localScale = new Vector3(-1, 1, 1);
            playerSprite.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            friendSprite.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            friendSprite.sprite = friendSpritesRight[(int)friendHand];
            playerSprite.sprite = playerSpritesRight[0];
        }

        playerHand = HandStatus.Highfive;
    }

    void Update()
    {
        if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {
            //Activate animations to switch the hands and save the next hand for when space is pressed we can check that.
            //Build in coroutine to delay the return so no double beats get picked up.
            if (!running)
                StartCoroutine(RegisterBeat());
        }
        //Check for the spacebar

        if (Input.GetKeyDown(KeyCode.Space) && !lockInput)
        {
            StartCoroutine(HandleInput());
        }
    }

    IEnumerator HandleInput()
    {
        lockInput = true;
        if (playerHand == friendHand)
        {
            rAudio.PlaySound(true);
            FrogForceManager.instance.score += 1;
        }
        else
        {
            rAudio.PlaySound(false);
            FrogForceManager.instance.errors += 1;
        }
        yield return new WaitForSeconds(0.5f);
        FrogForceManager.instance.TransitionScene("GameScene");
    }

    IEnumerator RegisterBeat()
    {
        running = true;
        if (started)
        {
            currentBeats += 1;
            if (currentBeats > beatsPerChange)
            {
                playerAnimator.SetTrigger("Change");
                currentBeats = 0;
            }
            else
                playerAnimator.SetTrigger("Bop");
            friendAnimator.SetTrigger("Bop");
        }
        else
        {
            started = true;
            playerAnimator.SetTrigger("Start");
        }

        Debug.Log("Hit");

        yield return new WaitForSeconds(.1f);
        running = false;
    }

    public void ChangePlayerHand()
    {
        Debug.Log("ChangeHands");

        switch (playerHand)
        {
            case HandStatus.Highfive:
                //Image to next one
                playerHand = HandStatus.Fistbump;
                break;
            case HandStatus.Fistbump:
                playerHand = HandStatus.Handshake;
                break;
            case HandStatus.Handshake:
                playerHand = HandStatus.Highfive;
                break;
        }

        if (greenLookingLeft)
            playerSprite.sprite = playerSpritesLeft[(int)playerHand];
        else
        {
            playerSprite.sprite = playerSpritesRight[(int)playerHand];
        }
    }
}
