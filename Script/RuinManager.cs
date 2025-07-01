using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RuinManager : MonoBehaviour
{
    //SPEED設定
    float speed;
    float walk_speed = 3.0f;
    float run_speed = 7.5f;
    //空腹ゲージ設定
    float maxHunger = 10.0f;
    float currentHunger;
    float hunger_speed = 0.25f;
    bool Hunger = true;
    //時間変数設定
    float text_timer = 0;
    float camera_timer = 0;
    //アイテム数変数
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
    //Component設定
    Animator animator;
    //Text設定
    public Text CommentText;
    public Text GoalText;
    public Text WoodNum;
    public Text StoneNum;
    public Text SlimeNum;
    public Text ShellNum;
    public Text MeatNum;
    //Image設定
    public Image HungerImage;
    //Tips
    string[] Goal = new string[] {"森で木片を3個集めよう","森で木片を5個集めよう","森で木片を10個集めよう","湿地で木を3個、スライムを3個集めよう", "湿地で木を5個、スライムを5個集めよう", "湿地で木を10個、スライムを10個集めよう","高山で石を3個、甲羅を3個集めよう", "高山で石を5個、甲羅を5個集めよう", "高山で石を10個、甲羅を10個集めよう", "すべての遺跡を解放した" };
    int num;
    //遺跡再生成用リスト
    List<GameObject> ruins = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();
    //Camera
    public Camera MainCamera;
    public Camera RRCamera;
    //CameraBool
    bool isMainActive;
    //スマホ対応
    bool WalkMove;
    bool RunMove;
    public Text DashText;
    int dash_count = 0;
    public Text CursorText;
    public Button Dash;
    //メニューボタン
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
        WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
        StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
        SlimeNum.text = string.Format("スライム　×　{0}", slime_count).ToString();
        ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
        MeatNum.text = string.Format("肉　×　{0}", meat_count).ToString();
        HungerImage.fillAmount = currentHunger / maxHunger;
        GoalText.text = string.Format("{0}", Goal[num]).ToString();
        List<GameObject> ruins = new List<GameObject> { Ruin1, Ruin2, Ruin3, Ruin4, Ruin5, Ruin6, Ruin7, Ruin8, Ruin9 };
        List<Vector3> positions = new List<Vector3> { new Vector3(31.01089f, 0.0402923f, 9.21743f), new Vector3(10.55266f, -1.191145f, 12.15006f), new Vector3(22.14985f, -0.01638826f, 0.1132722f), new Vector3(7.34791f, -0.9132723f, -4.9371f), new Vector3(36.4117f, 3.369771f, -18.91694f), new Vector3(24.85003f, 5.566135f, -21.89919f), new Vector3(11.67266f, 5.566135f, -21.32228f), new Vector3(10.09277f, -1f, 20.62524f), new Vector3(0f, -1f, 0f) };
        //解放した遺跡を再生成
        for (int i = 0; i < num; i++)
        {
            Instantiate(ruins[i], positions[i], Quaternion.Euler(-90, 0, 0));
        }
        //CameraBool
        isMainActive = MainCamera.enabled;
        RRCamera.enabled = !isMainActive;
        //スマホ対応
        DashText.text = "走る".ToString();
        int cursor_num = PlayerPrefs.GetInt("c", 0);
        if (cursor_num == 0)
        {
            Dash.gameObject.SetActive(false);//カーソル非表示へ
            CursorText.text = "カーソル表示".ToString();
        }
        else
        {
            Dash.gameObject.SetActive(true);//カーソル表示へ
            CursorText.text = "カーソル非表示".ToString();
        }
        //UI整理
        MenuBack.gameObject.SetActive(false);
        MenuText.text = "メニューを開く".ToString();
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
        //空腹時のダッシュ不可
        if (currentHunger <= 0)
        {
            Hunger = false;
            RunMove = false;
            walkSource.pitch = 1.0f;
        }
        //Player上移動
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
            angles.y = 0.0f; // 任意の角度に設定
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
        //Player下移動
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
            angles.y = 180.0f; // 任意の角度に設定
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
        //Player右移動
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
            angles.y = 90.0f; // 任意の角度に設定
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
        //Player左移動
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
            angles.y = -90.0f; // 任意の角度に設定
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
        //Playerがステージ外に落下した場合の処理
        if (this.transform.position.y < -10.0f)
        {
            this.transform.position = new Vector3(0, 0, 0);
        }
        //スマホ対応
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

    public void RestoreButton()//空腹ゲージ回復
    {
        if (meat_count == 0)
        {
            ads.clip = UIsound;
            ads.Play();
            comment("肉を所持していません");
        }
        else if (currentHunger >= 10)
        {
            ads.clip = UIsound;
            ads.Play();
            comment("空腹ゲージは満タンです");
        }
        else
        {
            ads.clip = Eatsound;
            ads.Play();
            currentHunger += 2.5f;
            meat_count -= 1;
            MeatNum.text = string.Format("肉　×　{0}", meat_count).ToString();
            PlayerPrefs.SetInt("meat", meat_count);
            comment("空腹ゲージが回復しました");
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
    //遺跡解放
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
        //遺跡1の解放(木3)
        if (num == 0)
        {
            if (wood_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("木片が足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 1;
                comment("遺跡1を解放しました！");
                GoalText.text = string.Format("{0}", Goal[1]);
                wood_count -= 3;
                PlayerPrefs.SetInt("wood", wood_count);
                WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
                GameObject R1 = Instantiate(Ruin1, new Vector3(31.01089f, 0.0402923f, 9.21743f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(31.01089f, 0.0402923f, 9.21743f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(31.01089f, 0.0402923f + 10f, 9.21743f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡2の解放(木5)
        else if (num == 1)
        {
            if (wood_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("木片が足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 2;
                comment("遺跡2を解放しました！");
                GoalText.text = string.Format("{0}", Goal[2]);
                wood_count -= 5;
                PlayerPrefs.SetInt("wood", wood_count);
                WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
                GameObject R2 = Instantiate(Ruin2, new Vector3(10.55266f, -1.191145f, 12.15006f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(10.55266f, -1.191145f, 12.15006f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(10.55266f, -1.191145f + 10f, 12.15006f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡3の解放
        else if (num == 2)
        {
            if (wood_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("木片が足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 3;
                comment("遺跡3を解放しました！");
                comment("新ステージ「湿地」が解放された！");
                comment("Unity砲-βが使えるようになった！");
                GoalText.text = string.Format("{0}", Goal[3]);
                wood_count -= 10;
                PlayerPrefs.SetInt("wood", wood_count);
                WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
                GameObject R3 = Instantiate(Ruin3, new Vector3(22.14985f, -0.01638826f, 0.1132722f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(22.14985f, -0.01638826f, 0.1132722f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(22.14985f, -0.01638826f + 10f, 0.1132722f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡4の解放
        else if (num == 3)
        {
            if (wood_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("木片が足りません");
            }
            else if (slime_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("スライムが足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 4;
                comment("遺跡4を解放しました！");
                GoalText.text = string.Format("{0}", Goal[4]);
                wood_count -= 3;
                slime_count -= 3;
                PlayerPrefs.SetInt("wood", wood_count);
                PlayerPrefs.SetInt("slime", slime_count);
                WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
                SlimeNum.text=string.Format("スライム　×　{0}", slime_count).ToString();
                GameObject R4 = Instantiate(Ruin4, new Vector3(7.34791f, -0.9132723f, -4.9371f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(7.34791f, -0.9132723f, -4.9371f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(7.34791f, -0.9132723f + 10f, -4.9371f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡5の解放
        else if (num == 4)
        {
            if (wood_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("木片が足りません");
            }
            else if (slime_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("スライムが足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 5;
                comment("遺跡5を解放しました！");
                GoalText.text = string.Format("{0}", Goal[5]);
                wood_count -= 5;
                slime_count -= 5;
                PlayerPrefs.SetInt("wood", wood_count);
                PlayerPrefs.SetInt("slime", slime_count);
                WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
                SlimeNum.text = string.Format("スライム　×　{0}", slime_count).ToString();
                GameObject R5 = Instantiate(Ruin5, new Vector3(36.4117f, 3.369771f, -18.91694f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(36.4117f, 3.369771f, -18.91694f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(36.4117f, 3.369771f + 10f, -18.91694f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡6の解放
        else if (num == 5)
        {
            if (wood_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("木片が足りません");
            }
            else if (slime_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("スライムが足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 6;
                comment("遺跡6を解放しました！");
                comment("新ステージ「高山」が解放された！");
                comment("Unity砲-γが使えるようになった！");
                GoalText.text = string.Format("{0}", Goal[6]);
                wood_count -= 10;
                slime_count -= 10;
                PlayerPrefs.SetInt("wood", wood_count);
                PlayerPrefs.SetInt("slime", slime_count);
                WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
                SlimeNum.text = string.Format("スライム　×　{0}", slime_count).ToString();
                GameObject R6 = Instantiate(Ruin6, new Vector3(24.85003f, 5.566135f, -21.89919f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(24.85003f, 5.566135f, -21.89919f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(24.85003f, 5.566135f + 10f, -21.89919f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡7の解放
        else if (num == 6)
        {
            if (stone_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("石が足りません");
            }
            else if (shell_count < 3)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("甲羅が足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 7;
                comment("遺跡7を解放しました！");
                GoalText.text = string.Format("{0}", Goal[7]);
                stone_count -= 3;
                shell_count -= 3;
                PlayerPrefs.SetInt("stone", stone_count);
                PlayerPrefs.SetInt("shell", shell_count);
                StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
                ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
                GameObject R7 = Instantiate(Ruin7, new Vector3(11.67266f, 5.566135f, -21.32228f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(11.67266f, 5.566135f, -21.32228f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(11.67266f, 5.566135f + 10f, -21.32228f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡8の解放
        else if (num == 7)
        {
            if (stone_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("石が足りません");
            }
            else if (shell_count < 5)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("甲羅が足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                num = 8;
                comment("遺跡8を解放しました！");
                GoalText.text = string.Format("{0}", Goal[8]);
                stone_count -= 5;
                shell_count -= 5;
                PlayerPrefs.SetInt("stone", stone_count);
                PlayerPrefs.SetInt("shell", shell_count);
                StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
                ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
                GameObject R8 = Instantiate(Ruin8, new Vector3(10.09277f, -1f, 20.62524f), Quaternion.Euler(-90, 0, 0));
                GameObject AE = Instantiate(AppearEffect, new Vector3(10.09277f, -1f, 20.62524f), Quaternion.Euler(-90, 0, 0));
                camera_timer = 0;
                MainCamera.enabled = !isMainActive;
                RRCamera.enabled = isMainActive;
                RRCamera.transform.position = new Vector3(10.09277f, -1f + 10f, 20.62524f);
                RRCamera.transform.rotation = Quaternion.Euler(90, 0, 0);

            }
        }
        //遺跡9の解放
        else if (num == 8)
        {
            if (stone_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("石が足りません");
            }
            else if (shell_count < 10)
            {
                ads.clip = UIsound;
                ads.Play();
                comment("甲羅が足りません");
            }
            else
            {
                ads.clip = Releasesound;
                ads.Play();
                RuinReleaseButtonText.text = "エンディング".ToString();
                num = 9;
                comment("遺跡9を解放しました！");
                GoalText.text = string.Format("{0}", Goal[9]);
                stone_count -= 10;
                shell_count -= 10;
                PlayerPrefs.SetInt("stone", stone_count);
                PlayerPrefs.SetInt("shell", shell_count);
                StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
                ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
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
    //スマホ対応ボタン関数
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
                comment("空腹時は走れません");
            }
            else
            {
                RunMove = true;
                DashText.text = "歩く".ToString();
            }
        }
        else
        {
            RunMove = false;
            DashText.text = "走る".ToString();
        }
    }
    //上移動
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
        angles.y = 0.0f; // 任意の角度に設定
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
    //下移動
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
        angles.y = 180.0f; // 任意の角度に設定
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
    //右移動
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
        angles.y = 90.0f; // 任意の角度に設定
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
    //左移動
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
        angles.y = -90.0f; // 任意の角度に設定
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
    //スマホ対応ボタンのON/OFF
    public void CursorButton()
    {
        ads.clip = UIsound;
        ads.Play();
        if (Dash.gameObject.activeSelf)
        {
            Dash.gameObject.SetActive(false);
            CursorText.text = "カーソル表示".ToString();
            PlayerPrefs.SetInt("c", 0);
        }
        else
        {
            Dash.gameObject.SetActive(true);
            CursorText.text = "カーソル非表示".ToString();
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
            MenuText.text = "メニューを開く".ToString();
        }
        else
        {
            MenuBack.gameObject.SetActive(true);
            MenuText.text = "メニューを閉じる".ToString();
        }
    }

    //コメント調整関数
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