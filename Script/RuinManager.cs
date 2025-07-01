using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RuinManager : MonoBehaviour
{
    //SPEED�ݒ�
    float speed;
    float walk_speed = 3.0f;
    float run_speed = 7.5f;
    //�󕠃Q�[�W�ݒ�
    float maxHunger = 10.0f;
    float currentHunger;
    float hunger_speed = 0.25f;
    bool Hunger = true;
    //���ԕϐ��ݒ�
    float text_timer = 0;
    float camera_timer = 0;
    //�A�C�e�����ϐ�
    int wood_count;
    int stone_count;
    int slime_count;
    int shell_count;
    int meat_count;
    //RuinObject
    public GameObject Ruin1;
    public GameObject Ruin2;
    public GameObject Ruin3;
    public GameObject Ruin4;
    public GameObject Ruin5;
    public GameObject Ruin6;
    public GameObject Ruin7;
    public GameObject Ruin8;
    public GameObject Ruin9;
    public GameObject AppearEffect;
    public Button RuinRelease;
    //Component�ݒ�
    Animator animator;
    //Text�ݒ�
    public Text CommentText;
    public Text GoalText;
    public Text WoodNum;
    public Text StoneNum;
    public Text SlimeNum;
    public Text ShellNum;
    public Text MeatNum;
    //Image�ݒ�
    public Image HungerImage;
    //Tips
    string[] Goal = new string[] {"�X�ŖؕЂ�3�W�߂悤","�X�ŖؕЂ�5�W�߂悤","�X�ŖؕЂ�10�W�߂悤","���n�Ŗ؂�3�A�X���C����3�W�߂悤", "���n�Ŗ؂�5�A�X���C����5�W�߂悤", "���n�Ŗ؂�10�A�X���C����10�W�߂悤","���R�Ő΂�3�A�b����3�W�߂悤", "���R�Ő΂�5�A�b����5�W�߂悤", "���R�Ő΂�10�A�b����10�W�߂悤", "���ׂĂ̈�Ղ��������" };
    int num;
    //��ՍĐ����p���X�g
    List<GameObject> ruins = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();
    //Camera
    public Camera MainCamera;
    public Camera RRCamera;
    //CameraBool
    bool isMainActive;
    //�X�}�z�Ή�
    bool WalkMove;
    bool RunMove;
    public Text DashText;
    int dash_count = 0;
    public Text CursorText;
    public Button Dash;
    //���j���[�{�^��
    public Image MenuBack;
    public Text MenuText;
    //SE
    AudioSource ads;
    public AudioClip UIsound;
    public AudioClip Eatsound;
    public AudioSource walkSource;
    float pitch_speed = 1.2f;
    public AudioClip Releasesound;
    //END
    public Text RuinReleaseButtonText;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        CommentText.text = "".ToString();
        num = PlayerPrefs.GetInt("n", 0);
        wood_count = PlayerPrefs.GetInt("wood", 0);
        stone_count = PlayerPrefs.GetInt("stone", 0);
        slime_count = PlayerPrefs.GetInt("slime", 0);
        shell_count = PlayerPrefs.GetInt("shell", 0);
        meat_count = PlayerPrefs.GetInt("meat", 0);
        currentHunger = PlayerPrefs.GetFloat("hunger", maxHunger);
        WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
        StoneNum.text = string.Format("�΁@�~�@{0}", stone_count).ToString();
        SlimeNum.text = string.Format("�X���C���@�~�@{0}", slime_count).ToString();
        ShellNum.text = string.Format("�b���@�~�@{0}", shell_count).ToString();
        MeatNum.text = string.Format("���@�~�@{0}", meat_count).ToString();
        HungerImage.fillAmount = currentHunger / maxHunger;
        GoalText.text = string.Format("{0}", Goal[num]).ToString();
        List<GameObject> ruins = new List<GameObject> { Ruin1, Ruin2, Ruin3, Ruin4, Ruin5, Ruin6, Ruin7, Ruin8, Ruin9 };
        List<Vector3> positions = new List<Vector3> { new Vector3(31.01089f, 0.0402923f, 9.21743f), new Vector3(10.55266f, -1.191145f, 12.15006f), new Vector3(22.14985f, -0.01638826f, 0.1132722f), new Vector3(7.34791f, -0.9132723f, -4.9371f), new Vector3(36.4117f, 3.369771f, -18.91694f), new Vector3(24.85003f, 5.566135f, -21.89919f), new Vector3(11.67266f, 5.566135f, -21.32228f), new Vector3(10.09277f, -1f, 20.62524f), new Vector3(0f, -1f, 0f) };
        //���������Ղ��Đ���
        for (int i = 0; i < num; i++)
        {
            Instantiate(ruins[i], positions[i], Quaternion.Euler(-90, 0, 0));
        }
        //CameraBool
        isMainActive = MainCamera.enabled;
        RRCamera.enabled = !isMainActive;
        //�X�}�z�Ή�
        DashText.text = "����".ToString();
        int cursor_num = PlayerPrefs.GetInt("c", 0);
        if (cursor_num == 0)
        {
            Dash.gameObject.SetActive(false);//�J�[�\����\����
            CursorText.text = "�J�[�\���\��".ToString();
        }
        else
        {
            Dash.gameObject.SetActive(true);//�J�[�\���\����
            CursorText.text = "�J�[�\����\��".ToString();
        }
        //UI����
        MenuBack.gameObject.SetActive(false);
        MenuText.text = "���j���[���J��".ToString();
        RuinRelease.gameObject.SetActive(false);
        //SE
        ads = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        camera_timer += Time.deltaTime;
        if (camera_timer > 5 && MainCamera.enabled == !isMainActive)
        {
            camera_timer = 0;
            MainCamera.enabled = isMainActive;
            RRCamera.enabled = !isMainActive;
        }
        if (CommentText.text != "".ToString())
        {
            text_timer += Time.deltaTime;
            if (text_timer > 5)
            {
                CommentText.text = "".ToString();
                text_timer = 0;
            }
        }
        //�󕠎��̃_�b�V���s��
        if (currentHunger <= 0)
        {
            Hunger = false;
            RunMove = false;
            walkSource.pitch = 1.0f;
        }
        //Player��ړ�
        if (Input.GetKeyDown("w"))
        {
            walkSource.Play();
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        if (Input.GetKey("w") == true)
        {
            animator.SetBool("Walking", true);
            speed = walk_speed;
            Vector3 angles = this.transform.eulerAngles;
            angles.y = 0.0f; // �C�ӂ̊p�x�ɐݒ�
            this.transform.eulerAngles = angles;
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Hunger == true)
            {
                speed = run_speed;
                animator.SetBool("Running", true);
                currentHunger -= hunger_speed * Time.deltaTime;
                PlayerPrefs.SetFloat("hunger", currentHunger);
                HungerImage.fillAmount = currentHunger / maxHunger;
                walkSource.pitch = pitch_speed;
            }
            else
            {
                animator.SetBool("Running", false);
                walkSource.pitch = 1.0f;
            }
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp("w"))
        {
            walkSource.pitch = 1.0f;
            walkSource.Stop();
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        //Player���ړ�
        if (Input.GetKeyDown("s"))
        {
            walkSource.Play();
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        if (Input.GetKey("s") == true)
        {
            animator.SetBool("Walking", true);
            speed = walk_speed;
            Vector3 angles = this.transform.eulerAngles;
            angles.y = 180.0f; // �C�ӂ̊p�x�ɐݒ�
            this.transform.eulerAngles = angles;
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Hunger == true)
            {
                speed = run_speed;
                animator.SetBool("Running", true);
                currentHunger -= hunger_speed * Time.deltaTime;
                PlayerPrefs.SetFloat("hunger", currentHunger);
                HungerImage.fillAmount = currentHunger / maxHunger;
                walkSource.pitch = pitch_speed;
            }
            else
            {
                animator.SetBool("Running", false);
                walkSource.pitch = 1.0f;
            }
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp("s"))
        {
            walkSource.pitch = 1.0f;
            walkSource.Stop();
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        //Player�E�ړ�
        if (Input.GetKeyDown("d"))
        {
            walkSource.Play();
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        if (Input.GetKey("d") == true)
        {
            animator.SetBool("Walking", true);
            speed = walk_speed;
            Vector3 angles = this.transform.eulerAngles;
            angles.y = 90.0f; // �C�ӂ̊p�x�ɐݒ�
            this.transform.eulerAngles = angles;
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Hunger == true)
            {
                speed = run_speed;
                animator.SetBool("Running", true);
                currentHunger -= hunger_speed * Time.deltaTime;
                PlayerPrefs.SetFloat("hunger", currentHunger);
                HungerImage.fillAmount = currentHunger / maxHunger;
                walkSource.pitch = pitch_speed;
            }
            else
            {
                animator.SetBool("Running", false);
                walkSource.pitch = 1.0f;
            }
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp("d"))
        {
            walkSource.pitch = 1.0f;
            walkSource.Stop();
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        //Player���ړ�
        if (Input.GetKeyDown("a"))
        {
            walkSource.Play();
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        if (Input.GetKey("a") == true)
        {
            animator.SetBool("Walking", true);
            speed = walk_speed;
            Vector3 angles = this.transform.eulerAngles;
            angles.y = -90.0f; // �C�ӂ̊p�x�ɐݒ�
            this.transform.eulerAngles = angles;
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Hunger == true)
            {
                speed = run_speed;
                animator.SetBool("Running", true);
                currentHunger -= hunger_speed * Time.deltaTime;
                PlayerPrefs.SetFloat("hunger", currentHunger);
                HungerImage.fillAmount = currentHunger / maxHunger;
                walkSource.pitch = pitch_speed;
            }
            else
            {
                animator.SetBool("Running", false);
                walkSource.pitch = 1.0f;
            }
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp("a"))
        {
            walkSource.pitch = 1.0f;
            walkSource.Stop();
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        //Player���X�e�[�W�O�ɗ��������ꍇ�̏���
        if (this.transform.position.y < -10.0f)
        {
            this.transform.position = new Vector3(0, 0, 0);
        }
        //�X�}�z�Ή�
        if (WalkMove == true)
        {
            animator.SetBool("Walking", true);
            if (RunMove == true)
            {
                speed = run_speed;
                animator.SetBool("Running", true);
                currentHunger -= hunger_speed * Time.deltaTime;
                PlayerPrefs.SetFloat("hunger", currentHunger);
                HungerImage.fillAmount = currentHunger / maxHunger;
            }
            else
            {
                speed = walk_speed;
                animator.SetBool("Running", false);
                animator.SetBool("Walking", true);
            }
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
    }

    public void StageChangeButton()
    {
        SceneManager.LoadScene("NowLoading");
    }

    public void RestoreButton()//�󕠃Q�[�W��
    {
        if (meat_count == 0)
        {
            ads.clip = UIsound;
            ads.Play();
            comment("�����������Ă��܂���");
        }
        else if (currentHunger >= 10)
        {
            ads.clip = UIsound;
            ads.Play();
            comment("�󕠃Q�[�W�͖��^���ł�");
        }
        else
        {
            ads.clip = Eatsound;
            ads.Play();
            currentHunger += 2.5f;
            meat_count -= 1;
            MeatNum.text = string.Format("���@�~�@{0}", meat_count).ToString();
            PlayerPrefs.SetInt("meat", meat_count);
            comment("�󕠃Q�[�W���񕜂��܂���");
            if (currentHunger > 10)
            {
                currentHunger = 10.0f;
            }
            if (Hunger == false)
            {
                Hunger = true;
            }
            HungerImage.fillAmount = currentHunger / maxHunger;
            PlayerPrefs.SetFloat("hunger", currentHunger);
        }
    }
    //��Չ��
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ruin")
        {
            RuinRelease.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ruin")
        {
            RuinRelease.gameObject.SetActive(false);
        }
    }

    public void RuinReleaseButton()
    {
        //���1�̉��(��3)
        if (num == 0)
        {
            if (wood_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�ؕЂ�����܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 1;
                comment("���1��������܂����I");
                GoalText.text = string.Format("{0}", Goal[1]);
                wood_count -= 3;
                PlayerPrefs.SetInt("wood", wood_count);
                WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
                GameObject R1 = Instantiate(Ruin1, new Vector3(31.01089f, 0.0402923f, 9.21743f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(31.01089f, 0.0402923f, 9.21743f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(31.01089f, 0.0402923f + 10f, 9.21743f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���2�̉��(��5)
        else if (num == 1)
        {
            if (wood_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�ؕЂ�����܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 2;
                comment("���2��������܂����I");
                GoalText.text = string.Format("{0}", Goal[2]);
                wood_count -= 5;
                PlayerPrefs.SetInt("wood", wood_count);
                WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
                GameObject R2 = Instantiate(Ruin2, new Vector3(10.55266f, -1.191145f, 12.15006f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(10.55266f, -1.191145f, 12.15006f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(10.55266f, -1.191145f + 10f, 12.15006f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���3�̉��
        else if (num == 2)
        {
            if (wood_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�ؕЂ�����܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 3;
                comment("���3��������܂����I");
                comment("�V�X�e�[�W�u���n�v��������ꂽ�I");
                comment("Unity�C-�����g����悤�ɂȂ����I");
                GoalText.text = string.Format("{0}", Goal[3]);
                wood_count -= 10;
                PlayerPrefs.SetInt("wood", wood_count);
                WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
                GameObject R3 = Instantiate(Ruin3, new Vector3(22.14985f, -0.01638826f, 0.1132722f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(22.14985f, -0.01638826f, 0.1132722f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(22.14985f, -0.01638826f + 10f, 0.1132722f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���4�̉��
        else if (num == 3)
        {
            if (wood_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�ؕЂ�����܂���");
            }
            else if (slime_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�X���C��������܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 4;
                comment("���4��������܂����I");
                GoalText.text = string.Format("{0}", Goal[4]);
                wood_count -= 3;
                slime_count -= 3;
                PlayerPrefs.SetInt("wood", wood_count);
                PlayerPrefs.SetInt("slime", slime_count);
                WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
                SlimeNum.text=string.Format("�X���C���@�~�@{0}", slime_count).ToString();
                GameObject R4 = Instantiate(Ruin4, new Vector3(7.34791f, -0.9132723f, -4.9371f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(7.34791f, -0.9132723f, -4.9371f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(7.34791f, -0.9132723f + 10f, -4.9371f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���5�̉��
        else if (num == 4)
        {
            if (wood_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�ؕЂ�����܂���");
            }
            else if (slime_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�X���C��������܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 5;
                comment("���5��������܂����I");
                GoalText.text = string.Format("{0}", Goal[5]);
                wood_count -= 5;
                slime_count -= 5;
                PlayerPrefs.SetInt("wood", wood_count);
                PlayerPrefs.SetInt("slime", slime_count);
                WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
                SlimeNum.text = string.Format("�X���C���@�~�@{0}", slime_count).ToString();
                GameObject R5 = Instantiate(Ruin5, new Vector3(36.4117f, 3.369771f, -18.91694f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(36.4117f, 3.369771f, -18.91694f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(36.4117f, 3.369771f + 10f, -18.91694f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���6�̉��
        else if (num == 5)
        {
            if (wood_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�ؕЂ�����܂���");
            }
            else if (slime_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�X���C��������܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 6;
                comment("���6��������܂����I");
                comment("�V�X�e�[�W�u���R�v��������ꂽ�I");
                comment("Unity�C-�����g����悤�ɂȂ����I");
                GoalText.text = string.Format("{0}", Goal[6]);
                wood_count -= 10;
                slime_count -= 10;
                PlayerPrefs.SetInt("wood", wood_count);
                PlayerPrefs.SetInt("slime", slime_count);
                WoodNum.text = string.Format("�ؕЁ@�~�@{0}", wood_count).ToString();
                SlimeNum.text = string.Format("�X���C���@�~�@{0}", slime_count).ToString();
                GameObject R6 = Instantiate(Ruin6, new Vector3(24.85003f, 5.566135f, -21.89919f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(24.85003f, 5.566135f, -21.89919f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(24.85003f, 5.566135f + 10f, -21.89919f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���7�̉��
        else if (num == 6)
        {
            if (stone_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�΂�����܂���");
            }
            else if (shell_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�b��������܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 7;
                comment("���7��������܂����I");
                GoalText.text = string.Format("{0}", Goal[7]);
                stone_count -= 3;
                shell_count -= 3;
                PlayerPrefs.SetInt("stone", stone_count);
                PlayerPrefs.SetInt("shell", shell_count);
                StoneNum.text = string.Format("�΁@�~�@{0}", stone_count).ToString();
                ShellNum.text = string.Format("�b���@�~�@{0}", shell_count).ToString();
                GameObject R7 = Instantiate(Ruin7, new Vector3(11.67266f, 5.566135f, -21.32228f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(11.67266f, 5.566135f, -21.32228f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(11.67266f, 5.566135f + 10f, -21.32228f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���8�̉��
        else if (num == 7)
        {
            if (stone_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�΂�����܂���");
            }
            else if (shell_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�b��������܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 8;
                comment("���8��������܂����I");
                GoalText.text = string.Format("{0}", Goal[8]);
                stone_count -= 5;
                shell_count -= 5;
                PlayerPrefs.SetInt("stone", stone_count);
                PlayerPrefs.SetInt("shell", shell_count);
                StoneNum.text = string.Format("�΁@�~�@{0}", stone_count).ToString();
                ShellNum.text = string.Format("�b���@�~�@{0}", shell_count).ToString();
                GameObject R8 = Instantiate(Ruin8, new Vector3(10.09277f, -1f, 20.62524f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(10.09277f, -1f, 20.62524f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(10.09277f, -1f + 10f, 20.62524f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //���9�̉��
        else if (num == 8)
        {
            if (stone_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�΂�����܂���");
            }
            else if (shell_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("�b��������܂���");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                RuinReleaseButtonText.text = "�G���f�B���O".ToString();
                num = 9;
                comment("���9��������܂����I");
                GoalText.text = string.Format("{0}", Goal[9]);
                stone_count -= 10;
                shell_count -= 10;
                PlayerPrefs.SetInt("stone", stone_count);
                PlayerPrefs.SetInt("shell", shell_count);
                StoneNum.text = string.Format("�΁@�~�@{0}", stone_count).ToString();
                ShellNum.text = string.Format("�b���@�~�@{0}", shell_count).ToString();
                GameObject R9 = Instantiate(Ruin9, new Vector3(0f, -1f, 0f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(0f, -1f, 0f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(0f, -1f + 15f, 0f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        else
        {
            SceneManager.LoadScene("Ending");
        }

        PlayerPrefs.SetInt("n", num);
    }
    //�X�}�z�Ή��{�^���֐�
    public void DashButton()
    {
        ads.clip = UIsound;
        ads.Play();
        dash_count++;
        if (dash_count % 2 == 1)
        {
            if (Hunger == false)
            {
                RunMove = false;
                dash_count--;
                comment("�󕠎��͑���܂���");
            }
            else
            {
                RunMove = true;
                DashText.text = "����".ToString();
            }
        }
        else
        {
            RunMove = false;
            DashText.text = "����".ToString();
        }
    }
    //��ړ�
    public void UpButtonUp()
    {
        walkSource.pitch = 1.0f;
        walkSource.Stop();
        WalkMove = false;
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
    }
    public void UpButtonDown()
    {
        WalkMove = true;
        Vector3 angles = this.transform.eulerAngles;
        angles.y = 0.0f; // �C�ӂ̊p�x�ɐݒ�
        this.transform.eulerAngles = angles;
        if (RunMove == true)
        {
            walkSource.pitch = 2.0f;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        walkSource.Play();
    }
    //���ړ�
    public void DownButtonUp()
    {
        walkSource.pitch = 1.0f;
        walkSource.Stop();
        WalkMove = false;
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
    }

    public void DownButtonDown()
    {
        WalkMove = true;
        Vector3 angles = this.transform.eulerAngles;
        angles.y = 180.0f; // �C�ӂ̊p�x�ɐݒ�
        this.transform.eulerAngles = angles;
        if (RunMove == true)
        {
            walkSource.pitch = 2.0f;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        walkSource.Play();
    }
    //�E�ړ�
    public void RightButtonUp()
    {
        walkSource.pitch = 1.0f;
        walkSource.Stop();
        WalkMove = false;
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
    }
    public void RightButtonDown()
    {
        WalkMove = true;
        Vector3 angles = this.transform.eulerAngles;
        angles.y = 90.0f; // �C�ӂ̊p�x�ɐݒ�
        this.transform.eulerAngles = angles;
        if (RunMove == true)
        {
            walkSource.pitch = 2.0f;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        walkSource.Play();
    }
    //���ړ�
    public void LeftButtonUp()
    {
        walkSource.pitch = 1.0f;
        walkSource.Stop();
        WalkMove = false;
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
    }
    public void LeftButtonDown()
    {
        WalkMove = true;
        Vector3 angles = this.transform.eulerAngles;
        angles.y = -90.0f; // �C�ӂ̊p�x�ɐݒ�
        this.transform.eulerAngles = angles;
        if (RunMove == true)
        {
            walkSource.pitch = 2.0f;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        walkSource.Play();
    }
    //�X�}�z�Ή��{�^����ON/OFF
    public void CursorButton()
    {
        ads.clip = UIsound;
        ads.Play();
        if (Dash.gameObject.activeSelf)
        {
            Dash.gameObject.SetActive(false);
            CursorText.text = "�J�[�\���\��".ToString();
            PlayerPrefs.SetInt("c", 0);
        }
        else
        {
            Dash.gameObject.SetActive(true);
            CursorText.text = "�J�[�\����\��".ToString();
            PlayerPrefs.SetInt("c", 1);
        }
    }

    public void MenuButton()
    {
        ads.clip = UIsound;
        ads.Play();
        if (MenuBack.gameObject.activeSelf)
        {
            MenuBack.gameObject.SetActive(false);
            MenuText.text = "���j���[���J��".ToString();
        }
        else
        {
            MenuBack.gameObject.SetActive(true);
            MenuText.text = "���j���[�����".ToString();
        }
    }

    //�R�����g�����֐�
    void comment(string a)
    {
        StartCoroutine(CommentCoroutine(a));
    }

    IEnumerator CommentCoroutine(string a)
    {
        if (string.IsNullOrEmpty(CommentText.text))
        {
            CommentText.text = a;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            text_timer = 0;
            CommentText.text = a;
        }
    }
}