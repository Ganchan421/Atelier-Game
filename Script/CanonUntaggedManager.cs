using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonUntaggedManager : MonoBehaviour
{
    float timer;
    float speed = 3f;
    public GameObject BombPrefab;
    public GameObject OrangePrefab;
    public GameObject BlackPrefab;
    public GameObject YellowPrefab;
    public GameObject Stone;
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            GameObject Bomb = Instantiate(BombPrefab, this.transform.position, this.transform.rotation);
            GameObject Orange = Instantiate(OrangePrefab, this.transform.position, this.transform.rotation);
            GameObject Black = Instantiate(BlackPrefab, this.transform.position, this.transform.rotation);
            GameObject Yellow = Instantiate(YellowPrefab, this.transform.position, this.transform.rotation);
            Instantiate(Stone, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
            Destroy(Orange.gameObject, 1.0f);
            Destroy(Black.gameObject, 1.0f);
            Destroy(Yellow.gameObject, 1.0f);
            timer = 0;
        }
    }
}
