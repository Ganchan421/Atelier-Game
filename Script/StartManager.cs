using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public AudioSource BGM;
    void Start()
    {
        BGM.Play();
    }

    void Update()
    {
        
    }

    public void SceneJump()
    {
        SceneManager.LoadScene("Ruin");
    }
}
