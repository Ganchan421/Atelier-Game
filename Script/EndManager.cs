using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    float maxHunger = 10f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void RestartButton()
    {
        PlayerPrefs.SetInt("wood", 5);
        PlayerPrefs.SetInt("stone", 0);
        PlayerPrefs.SetInt("slime", 0);
        PlayerPrefs.SetInt("shell", 0);
        PlayerPrefs.SetInt("meat", 0);
        PlayerPrefs.SetFloat("hunger", maxHunger);
        SceneManager.LoadScene("Start");
    }
}
