using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject controlsPage;

    public void NewGameBtn()
    {
        SceneManager.LoadScene("Main Level");
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

	public void controls()
	{  
		controlsPage.SetActive (true);
	}

	public void controlsExit()
	{
		controlsPage.SetActive (false);
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