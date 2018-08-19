using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject controlsPage;
    AudioSource controlsMusic;
    AudioSource AS;
    public void NewGameBtn()
    {

        StartCoroutine("StartGame");
    }

    void Start()
    {
        controlsMusic = gameObject.GetComponent<AudioSource>();
        AS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    IEnumerator StartGame()
    {
        
        float volume = AS.volume;
        float duration = 1.5f;
        float start = Time.time;
        while(Time.time-start < duration)
        {
            AS.volume*=.95f;
            yield return null;
        }
        SceneManager.LoadScene("Main Level");

    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    IEnumerator FadeInMusic()
    {

        float duration = 2.0f;
        float start = Time.time;
        while(Time.time-start < duration)
        {
            if(controlsMusic.volume<.98f)
            {
                controlsMusic.volume+=1/120f;
                AS.volume *=.91f;
            }
            yield return null;
        }
        controlsPage.SetActive (true);
    }

    IEnumerator FadeOutMusic()
    {
        float duration = 2.0f;
        float start = Time.time;
        while(Time.time-start < duration)
        {
            AS.volume/=.91f;
            controlsMusic.volume*= .93f;
            yield return null;
        }
        controlsPage.SetActive (false);

    }

	public void controls()
	{  
		StartCoroutine("FadeInMusic");
	}

	public void controlsExit()
	{
		StartCoroutine("FadeOutMusic");
	}

    public void quitToMenu()
    {
        Application.Quit();
    }

    public void restart()
    {
        SceneManager.LoadScene("Main Level");
    }
}