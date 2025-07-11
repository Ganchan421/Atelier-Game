using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonManager : MonoBehaviour
{
    //数値設定
    float speed = 7.5f;
    float timer;
    //GameObject設定
    public GameObject Wood;
    public GameObject Stone;
    public GameObject Shell;
    public GameObject Slime;
    public GameObject Meat;
    //Component設定
    Animator animator;
    //森林ではモンスターから肉のみが落ちるよう対応
    int num;

    void Start()
    {
        timer = 0;
        num = PlayerPrefs.GetInt("n", 0);
    }

    void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 1)//Unity砲を消す処理
        {
            Destroy(this.gameObject);
            timer = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tree")
        {
            Vector3 WoodPosition = new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, other.transform.position.z);
            Destroy(other.gameObject);
            Instantiate(Wood, WoodPosition, Quaternion.Euler(0, 0, 90));
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Slime")
        {
            Instantiate(Meat, this.transform.position, this.transform.rotation);
            if (num >= 3)
            {
                Instantiate(Slime, this.transform.position, this.transform.rotation);
            }
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Turtle")
        {
            Instantiate(Meat, this.transform.position, this.transform.rotation);
            if (num >= 3)
            {
                Instantiate(Shell, this.transform.position, this.transform.rotation);
            }
            Destroy(this.gameObject);
        }
    }
}
