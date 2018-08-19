using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager2 : MonoBehaviour
{
    AudioSource controlsMusic;
    AudioSource AS;

    public void NewGameBtn()
    {
        StartCoroutine("StartGame");
    }

    public void quitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void restart()
    {
        SceneManager.LoadScene("Main Level");
    }
}