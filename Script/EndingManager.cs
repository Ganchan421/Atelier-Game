using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EngingManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "https://ganchan421.github.io/unity-ending-video/ending.mp4";
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (vp) => vp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 70)
        {
            SceneManager.LoadScene("End");
        }
    }
}
