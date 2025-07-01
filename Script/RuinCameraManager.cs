using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RuinCameraManager : MonoBehaviour
{
    public GameObject Player;
    int num;
    List<Vector3> positions = new List<Vector3>();
    void Start()
    {
        List<Vector3> positions = new List<Vector3> { new Vector3(31.01089f, 0.6402923f + 5, 9.21743f) };
    }

    void Update()
    {
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 PlayerAngles = Player.transform.eulerAngles;
        this.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y + 2, PlayerPosition.z - 4);
        /*if (Input.GetKeyDown("up"))
        {
            this.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y + 2, PlayerPosition.z - 4);
            this.transform.eulerAngles=new Vector3(PlayerAngles.x+12, 0 ,PlayerAngles.z);
        }
        if (Input.GetKeyDown("down"))
        {
            this.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y + 2, PlayerPosition.z + 4);
            this.transform.eulerAngles = new Vector3(PlayerAngles.x + 12, 180, PlayerAngles.z);
        }
        if (Input.GetKeyDown("right"))
        {
            this.transform.position = new Vector3(PlayerPosition.x - 4, PlayerPosition.y + 2, PlayerPosition.z);
            this.transform.eulerAngles = new Vector3(PlayerAngles.x+12, 90, PlayerAngles.z);
        }
        if (Input.GetKeyDown("left"))
        {
            this.transform.position = new Vector3(PlayerPosition.x + 4, PlayerPosition.y + 2, PlayerPosition.z);
            this.transform.eulerAngles = new Vector3(PlayerAngles.x+12, 270, PlayerAngles.z);
        }*/
    }
}
