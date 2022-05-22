using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultAudioManager : MonoBehaviour
{
    public AudioClip[] success;
    public AudioClip[] mistake;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(bool won)
    {
        if(won)
        {
            audioSource.clip = success[Random.Range(0, success.Length - 1)];
        }
        else
            audioSource.clip = mistake[Random.Range(0, mistake.Length - 1)];

        audioSource.Play();
    }
}
