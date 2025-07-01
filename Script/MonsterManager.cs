using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    float timer = 0.0f;
    float time = 0.0f;
    float speed = 2.0f;
    float Mark_Hight = 100.0f;
    Animator animator;
    public GameObject Mark;
    Vector3 Position;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        time = UnityEngine.Random.Range(5f, 10f);
        Position = this.transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > time + 3.0f)
        {
            animator.SetBool("Moving", true);
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
        if (timer > time + 6.0f)
        {
            timer = 0.0f;
            animator.SetBool("Moving", false);
            Vector3 angles = this.transform.eulerAngles;
            angles.y += 90.0f;
            this.transform.eulerAngles = angles;
            time = UnityEngine.Random.Range(5f, 10f);
        }
        Mark.gameObject.transform.position = new Vector3(this.transform.position.x, Mark_Hight, this.transform.position.z);
        if (this.transform.position.y < -10f)
        {
            this.transform.position = Position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "canon")
        {
            animator.SetBool("Dying", true);
            Destroy(this.gameObject,1);
        }
    }
}
