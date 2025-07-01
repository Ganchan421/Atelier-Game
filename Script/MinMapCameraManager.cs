using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMapCameraManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Mark;
    float Camera_Hight = 150;
    float Mark_Hight = 100;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 PlayerPosition = Player.transform.position;
        this.transform.position = new Vector3(PlayerPosition.x, Camera_Hight, PlayerPosition.z);
        Mark.gameObject.transform.position = new Vector3(PlayerPosition.x, Mark_Hight, PlayerPosition.z);
    }
}
