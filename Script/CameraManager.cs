using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 PlayerAngles = Player.transform.eulerAngles;
        this.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y + 3, PlayerPosition.z - 6);
        /*
        if (PlayerAngles.y == 0.0f)
        {
            this.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y + 3, PlayerPosition.z - 6);
            this.transform.eulerAngles=new Vector3(PlayerAngles.x+12,PlayerAngles.y,PlayerAngles.z);
        }
        if (PlayerAngles.y == 180.0f)
        {
            this.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y + 3, PlayerPosition.z + 6);
            this.transform.eulerAngles = new Vector3(PlayerAngles.x + 12, PlayerAngles.y, PlayerAngles.z);
        }
        if (PlayerAngles.y == 90.0f)
        {
            this.transform.position = new Vector3(PlayerPosition.x - 6, PlayerPosition.y + 3, PlayerPosition.z);
            this.transform.eulerAngles = new Vector3(PlayerAngles.x+12, PlayerAngles.y, PlayerAngles.z);
        }
        if (PlayerAngles.y == 270.0f)
        {
            this.transform.position = new Vector3(PlayerPosition.x + 6, PlayerPosition.y + 3, PlayerPosition.z);
            this.transform.eulerAngles = new Vector3(PlayerAngles.x+12, PlayerAngles.y, PlayerAngles.z);
        }*/
    }
}

