using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Credits : MonoBehaviour
{
    public GameObject textBox;
    public Text theText;
    public AudioSource music;

    public TextAsset textFile;
    public string[] textLines;


    public int currentLine;
    public int endAtLine;

    public bool started;

    public float fadeSpeed = 1.5f;
    public Image FadeImg;

    // Use this for initialization
    void Start()
    {
        //music = gameObject.GetComponent<AudioSource>();
        started = false;

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }


        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        FadeImg = GameObject.Find("Fade").GetComponent<Image>();
        InvokeRepeating("FadeToClear", 0.0f, 0.02f);

    }

    void Update()
    {

        if (!started)
        {
            started = true;
            StartCoroutine("waitThreeSeconds");
        }
    }

    public void FadeToClear()
    {
        FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
        if (FadeImg.color.a < 0.05f)
        {
            CancelInvoke("FadeToClear");
            FadeImg.color = Color.clear;
        }
    }

    void FadeToBlack()
    {
        FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
        if (FadeImg.color.a == 1.0f)
        {
            CancelInvoke("FadeToBlack");
        }
    }

    void updateLine()
    {
        StartCoroutine("waitThreeSeconds");
    }

    IEnumerator waitThreeSeconds()
    {
        StartCoroutine(FadeInMusic());
        yield return new WaitForSeconds(3.0f);
        theText.text = "Programmers:\n\nMasa Maeda\n\nDaniel Gutierrez\n\nUlises Perez";
        yield return new WaitForSeconds(6.0f);
        theText.text = "Artists:\n\nTina Feng\n\nNaz Hartoonian";
        yield return new WaitForSeconds(6.0f);
        theText.text = " ";
        FadeImg = GameObject.Find("Fade").GetComponent<Image>();
        InvokeRepeating("FadeToBlack", 0.0f, 0.02f);
        yield return new WaitForSeconds(3.0f);
        yield return new WaitForSeconds(3.0f);
        StartCoroutine("FadeOutMusic");
    }

    IEnumerator FadeInMusic()
    {
    float duration = 2.0f;
    float start = Time.time;
    while(Time.time-start < duration)
    {
        if(music.volume<.98f)
        {
            music.volume+=1/120f;
        }
        yield return null;
    }
    }

    IEnumerator FadeOutMusic()
    {
        float duration = 2.0f;
        float start = Time.time;
        while(Time.time-start < duration)
        {
            music.volume*=.93f;
            yield return null;
        }
        SceneManager.LoadScene("Main Menu");

    }
    
}