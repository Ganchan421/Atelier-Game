using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class BombManager : MonoBehaviour
{
    //GameObjectê›íË
    public GameObject Wood;
    public GameObject Stone;
    public GameObject Shell;
    public GameObject Slime;
    public GameObject Meat;
    //Componentê›íË
    Animator animator;
    //SE
    AudioSource ads;
    void Start()
    {
        ads = this.gameObject.GetComponent<AudioSource>();
        ads.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 1.0f);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tree")
        {
            Vector3 WoodPosition = new Vector3(other.transform.position.x, other.transform.position.y + 0.2f, other.transform.position.z);
            Destroy(other.gameObject);
            Instantiate(Wood, WoodPosition, Quaternion.Euler(0, 0, 90));
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Slime")
        {
            Instantiate(Slime, this.transform.position, this.transform.rotation);
            Instantiate(Meat, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Turtle")
        {
            Instantiate(Shell, this.transform.position, this.transform.rotation);
            Instantiate(Meat, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
