using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreStartManager : MonoBehaviour
{
    public Text CreditText;
    public Text ButtonText;
    float duration = 2.0f;
    float timer = 0f;

    void Start()
    {
        CreditText.color = Color.black;
        ButtonText.color = Color.black;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < duration)
        {
            float t = Mathf.Clamp01(timer / duration);//0`1 ‚É§ŒÀ
            CreditText.color = Color.Lerp(Color.black, Color.white, t);
        }
        if (timer > duration && timer < duration + 2.0f)
        {
            float u = Mathf.Clamp01((timer - 2.0f) / duration);//0`1 ‚É§ŒÀ
            ButtonText.color = Color.Lerp(Color.black, Color.white, u);
        }
    }

    public void PreStartButton()
    {
        SceneManager.LoadScene("Start");
    }
}
