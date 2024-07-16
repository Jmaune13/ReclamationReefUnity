using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    
    public void PlayGame()
    {
        SceneManager.LoadScene(3);
    }

    public void HowToPlay()
    {

    }

    public void clickQuit()
    {
        Application.Quit();
    }




}
