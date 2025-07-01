using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerManager_wetland : MonoBehaviour
{
    //SPEED設定
    float speed;
    float walk_speed=3.0f;
    float run_speed = 7.5f;
    //空腹ゲージ設定
    float maxHunger=10.0f;
    float currentHunger;
    float hunger_speed = 0.25f;
    bool Hunger = true;
    //時間変数設定
    float text_timer=0;
    float canon_timer=4f;//βは3秒
    //アイテム数変数
    int wood_count;
    int stone_count;
    int slime_count;
    int shell_count;
    int meat_count;
    //GameObject設定
    public GameObject Canon;
    //Component設定
    Animator animator;
    //Text設定
    public Text CommentText;
    public Text WoodNum;
    public Text StoneNum;
    public Text SlimeNum;
    public Text ShellNum;
    public Text MeatNum;
    public Text GoalText;
    //Image設定
    public Image HungerImage;
    //Tips
    string[] Goal = new string[] { "森で木片を3個集めよう", "森で木片を5個集めよう", "森で木片を10個集めよう", "湿地で木を3個、スライムを3個集めよう", "湿地で木を5個、スライムを5個集めよう", "湿地で木を10個、スライムを10個集めよう", "高山で石を3個、甲羅を3個集めよう", "高山で石を5個、甲羅を5個集めよう", "高山で石を10個、甲羅を10個集めよう", "すべての遺跡を解放した" };
    int num;
    //スマホ対応
    bool WalkMove;
    bool RunMove;
    public Text DashText;
    int dash_count = 0;
    public Text CursorText;
    public Button Dash;
    public Button Gun;
    public Text GunText;
    //メニューボタン
    public Image MenuBack;
    public Text MenuText;
    //SE
    AudioSource ads;
    public AudioClip UIsound;
    public AudioClip Gunsound;
    public AudioClip Reloadsound;
    public AudioClip Eatsound;
    public AudioSource walkSource;
    float pitch_speed = 1.2f;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        CommentText.text = "".ToString();
        wood_count = PlayerPrefs.GetInt("wood", 0);
        stone_count = PlayerPrefs.GetInt("stone", 0);
        slime_count = PlayerPrefs.GetInt("slime", 0);
        shell_count = PlayerPrefs.GetInt("shell", 0);
        meat_count = PlayerPrefs.GetInt("meat", 0);
        currentHunger = PlayerPrefs.GetFloat("hunger", maxHunger);
        HungerImage.fillAmount = currentHunger / maxHunger;
        WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
        StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
        SlimeNum.text = string.Format("スライム　×　{0}", slime_count).ToString();
        ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
        MeatNum.text = string.Format("肉　×　{0}", meat_count).ToString();
        num = PlayerPrefs.GetInt("n", 0);
        GoalText.text = string.Format("{0}", Goal[num]).ToString();
        //スマホ対応
        DashText.text = "走る".ToString();
        GunText.text = "Unity砲-β".ToString();
        int cursor_num = PlayerPrefs.GetInt("c", 0);
        if (cursor_num == 0)
        {
            Dash.gameObject.SetActive(false);//カーソル非表示へ
            Gun.gameObject.SetActive(false);
            CursorText.text = "カーソル表示".ToString();
        }
        else
        {
            Dash.gameObject.SetActive(true);//カーソル非表示へ
            Gun.gameObject.SetActive(true);
            CursorText.text = "カーソル非表示".ToString();
        }
        //UI整理
        MenuBack.gameObject.SetActive(false);
        MenuText.text = "メニューを開く".ToString();
        //SE
        ads = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        canon_timer += Time.deltaTime;
        if (CommentText.text != "".ToString())
        {//5秒でコメントテキストを消す
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
        //Unity砲生成
        if (Input.GetMouseButtonDown(1) && canon_timer > 0.5f)
        {
            if (canon_timer < 3f)
            {
                ads.clip = Reloadsound;
                ads.Play();
                comment("Unity砲-βは3秒に1回打てます");
            }
            else
            {
                ads.clip = Gunsound;
                ads.Play();
                Vector3 PlayerPosition = this.transform.position;
                PlayerPosition.y += 0.6f;
                Instantiate(Canon, PlayerPosition, this.transform.rotation);
                Instantiate(Canon, PlayerPosition, Quaternion.Euler(0, this.transform.eulerAngles.y + 15f, 0));
                Instantiate(Canon, PlayerPosition, Quaternion.Euler(0, this.transform.eulerAngles.y - 15f, 0));
                canon_timer = 0;
                GunText.text = "リロード中".ToString();
            }
        }
        //Playerがステージ外に落下した際、ステージ中心に戻す処理
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
        if (canon_timer > 3)
        {
            GunText.text = "Unity砲-β".ToString();
        }
    }

    void OnTriggerEnter(Collider other)//アイテム入手処理とデータ保存
    {
        if (other.gameObject.tag == "wood")
        {
            Destroy(other.gameObject,0.5f);
            comment("木片を入手した");
            wood_count += 1;
            PlayerPrefs.SetInt("wood", wood_count);
            WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
        }
        if (other.gameObject.tag == "slime")
        {
            Destroy(other.gameObject, 0.5f);
            comment("スライムを入手した");
            slime_count += 1;
            PlayerPrefs.SetInt("slime", slime_count);
            SlimeNum.text = string.Format("スライム　×　{0}", slime_count).ToString();
        }
        if (other.gameObject.tag == "meat")
        {
            Destroy(other.gameObject, 0.5f);
            comment("肉を入手した");
            meat_count += 1;
            PlayerPrefs.SetInt("meat", meat_count);
            MeatNum.text = string.Format("肉　×　{0}", meat_count).ToString();
        }
        /*ステージ"湿地"では不要
        if (other.gameObject.tag == "stone")
        {
            Destroy(other.gameObject, 0.5f);
            comment("石を入手した");
            stone_count += 1;
            PlayerPrefs.SetInt("stone", stone_count);
            StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
        }
        if (other.gameObject.tag == "shell")
        {
            Destroy(other.gameObject, 0.5f);
            comment("甲羅を入手した");
            shell_count += 1;
            PlayerPrefs.SetInt("shell", shell_count);
            ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
        }*/
    }

    public void StageChangeButton()//ステージ移動
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
    //Unity砲
    public void GunButton()
    {
        if (canon_timer < 3f)
        {
            ads.clip = Reloadsound;
            ads.Play();
            comment("Unity砲-βは3秒間に1回打てます");
        }
        else
        {
            ads.clip = Gunsound;
            ads.Play();
            Vector3 PlayerPosition = this.transform.position;
            PlayerPosition.y += 0.6f;
            Instantiate(Canon, PlayerPosition, this.transform.rotation);
            Instantiate(Canon, PlayerPosition, Quaternion.Euler(0, this.transform.eulerAngles.y + 15f, 0));
            Instantiate(Canon, PlayerPosition, Quaternion.Euler(0, this.transform.eulerAngles.y - 15f, 0));
            canon_timer = 0;
            GunText.text = "リロード中".ToString();
        }
    }
    //スマホ対応ボタンのON/OFF
    public void CursorButton()
    {
        ads.clip = UIsound;
        ads.Play();
        if (Dash.gameObject.activeSelf)
        {
            Dash.gameObject.SetActive(false);
            Gun.gameObject.SetActive(false);
            CursorText.text = "カーソル表示".ToString();
            PlayerPrefs.SetInt("c", 0);
        }
        else
        {
            Dash.gameObject.SetActive(true);
            Gun.gameObject.SetActive(true);
            CursorText.text = "カーソル非表示".ToString();
            PlayerPrefs.SetInt("c", 1);
        }
    }

    //UI整理
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

    public void NumResetButton()//調整用（リリース時は不要）
    {
        wood_count = 5;
        stone_count = 0;
        slime_count = 0;
        shell_count = 0;
        meat_count = 0;
        currentHunger = maxHunger;
        PlayerPrefs.SetInt("wood", 5);
        PlayerPrefs.SetInt("stone", 0);
        PlayerPrefs.SetInt("slime", 0);
        PlayerPrefs.SetInt("shell", 0);
        PlayerPrefs.SetInt("meat", 0);
        PlayerPrefs.SetFloat("hunger", maxHunger);
        HungerImage.fillAmount=currentHunger / maxHunger;
        WoodNum.text = string.Format("木片　×　{0}", wood_count).ToString();
        StoneNum.text = string.Format("石　×　{0}", stone_count).ToString();
        SlimeNum.text = string.Format("スライム　×　{0}", slime_count).ToString();
        ShellNum.text = string.Format("甲羅　×　{0}", shell_count).ToString();
        MeatNum.text = string.Format("肉　×　{0}", meat_count).ToString();
        Hunger = true;
        PlayerPrefs.SetInt("n", 0);
    }
}