using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NowLoadingManager : MonoBehaviour
{
    public Text Comment;
    int num;
    float timer;
    public AudioSource ads;
    void Start()
    {
        num = PlayerPrefs.GetInt("n", 0);
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5 && Comment.text != "".ToString())
        {
            Comment.text = "".ToString();
            timer = 0;
        }
    }

    public void ForestButton()
    {
        SceneManager.LoadScene("Forest");
    }

    public void RuinButton()
    {
        SceneManager.LoadScene("Ruin");
    }

    public void WetLandButton()
    {
        if (num < 3)
        {
            ads.Play();
            Comment.text = "���3�����ɖ`���ł��܂�".ToString();
            timer = 0;
        }
        else
        {
            SceneManager.LoadScene("Wetland");
        }
    }

    public void MountainButton()
    {
        if (num < 6)
        {
            ads.Play();
            Comment.text = "���6�����ɖ`���ł��܂�".ToString();
            timer = 0;
        }
        else
        {
            SceneManager.LoadScene("Mountain");
        }
    }
}
