using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenNavigationEnd : MonoBehaviour
{
    public string sceneName;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            FrogForceManager.instance.Reset();
            FrogForceManager.instance.TransitionScene("MainMenu");
            //SceneManager.LoadScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FrogForceManager.instance.Reset();
            FrogForceManager.instance.TransitionScene("GameScene");
            //SceneManager.LoadScene(sceneName);
        }
    }
}
