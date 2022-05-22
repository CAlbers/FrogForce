using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandChangeNotifier : MonoBehaviour
{
    public ThrowHandsManager throwHandsManager;

    public void ChangeHands()
    {
        throwHandsManager.ChangePlayerHand();
    }
}
