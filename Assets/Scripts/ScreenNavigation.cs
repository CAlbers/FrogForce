using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenNavigation : MonoBehaviour
{
    public string sceneName;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FrogForceManager.instance.TransitionScene(sceneName);
            //SceneManager.LoadScene(sceneName);
        }
    }
}
