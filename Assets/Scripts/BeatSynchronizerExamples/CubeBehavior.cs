using UnityEngine;
using System.Collections;
using SynchronizerData;


public class CubeBehavior : MonoBehaviour {

	private Animator anim;
	private BeatObserver beatObserver;


	void Start ()
	{
		anim = GetComponent<Animator>();
		beatObserver = GetComponent<BeatObserver>();
	}

	void Update ()
	{
		if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
			anim.SetTrigger("DownBeatTrigger");
			Debug.Log("Beat!!");
		}
		if ((beatObserver.beatMask & BeatType.UpBeat) == BeatType.UpBeat) {
			Debug.Log("Upbeat");
		}
	}
}
