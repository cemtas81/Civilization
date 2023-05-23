using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarbScreenCont : MonoBehaviour
{
  
    [SerializeField] private Slider healthSlider,coinSlider;
    [SerializeField] private GameObject gameOverPanel, pausePanel;
    [SerializeField] private Text scoreText, maxScoreText, deadZombiesText, bossText, woodText,comboText;   
    public GameObject upgradePanel;
    public int weaponLevel;
    private float maxScore,lastKillTime;
    private int deadZombiesCount,woodCount,headCount;
    MyController control;
    private BarbCont2 playerController;
    [SerializeField] private int comboCount;
    [SerializeField] private float comboTimeLimit = 2f;

    private void Start()
    {
      
        
        playerController =FindObjectOfType<BarbCont2>();
        maxScore = PlayerPrefs.GetFloat("MaxScore");
        coinSlider.value = 0;
        healthSlider.maxValue = playerController.playerStatus.health;
        UpdateHealthSlider();
        Time.timeScale = 1;
    }
    private void Awake()
    {
        control = new MyController();
        control.MyGameplay.Back.started += bck => Pause();
        control.MyGameplay.Submit.started += bck => Restart();
    }
    private void OnEnable()
    {
        control.MyGameplay.Enable();
    }
    private void OnDisable()
    {
        control.MyGameplay.Disable();
    }
    public void UpdateHealthSlider()
    {
        healthSlider.value = playerController.playerStatus.health;
    }
    public void UpdateCoinSlider(int coin)
    {
        coinSlider.value += coin / (1 + weaponLevel);

        if (coinSlider.value >= 100)
        {
            weaponLevel++;
            //playerController.Upgrade(uP.recentWeapon.skillLevel);
            upgradePanel.SetActive(true);
            coinSlider.value = 0;
        }

    }
    public void UpdateHead(int head)
    {
        headCount++;
        deadZombiesText.text=headCount.ToString();
    }  
    public void UpdateWood(int wood)
    {
        woodCount++;
        woodText.text=woodCount.ToString();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

        float time = Time.timeSinceLevelLoad;

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 100);
        scoreText.text = "You survived for " + minutes + " centuries and " + seconds + " years.";
        UpdateMaxScore(minutes, seconds, time);
    }
    public void Pause()
    {
        if (pausePanel.activeInHierarchy == true && upgradePanel.activeInHierarchy == false)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        if (upgradePanel.activeInHierarchy == true)
        {
            upgradePanel.SetActive(false);
        }
        if (pausePanel.activeInHierarchy == false)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    public void Restart()
    {
        //SceneManager.LoadScene("Game");
        if (gameOverPanel.activeInHierarchy == true)
        {
            SceneManager.LoadScene("Barbi2");
        }

    }

    public void UpdateDeadZombiesCount()
    {
        if (Time.time - lastKillTime <= comboTimeLimit)
        {
            comboText.gameObject.SetActive(true);
            comboCount++;
            comboText.text = string.Format("{1}x combo", headCount, comboCount);
            StartCoroutine(TextDisappear(2,comboText));
        }
        else
        {
            comboCount = 1;
            comboText.text = string.Format("", headCount);
        }

        lastKillTime = Time.time;

    }

    private void UpdateMaxScore(int minutes, int seconds, float time)
    {
        if (time > maxScore)
        {
            maxScore = time;
            maxScoreText.text = string.Format("Your best time is {0} centuries and {1} years.", minutes, seconds);
            PlayerPrefs.SetFloat("MaxScore", maxScore);          
        }
        else
        {
            time = PlayerPrefs.GetFloat("MaxScore");
            minutes = (int)(time / 100);
            seconds = (int)(time % 100);
            maxScoreText.text = string.Format("Your best time is {0} centuries and {1} years.", minutes, seconds);
        }
    }

    public void ShowBossText()
    {
        StartCoroutine(TextDisappear(2, bossText));
    }

    private IEnumerator TextDisappear(float time, Text text)
    {
        text.gameObject.SetActive(true);
        Color textColor = text.color;
        textColor.a = 1;
        text.color = textColor;
        yield return new WaitForSeconds(1);
        float count = 0;
        while (text.color.a > 0)
        {
            count += Time.deltaTime / time;
            textColor.a = Mathf.Lerp(1, 0, count);
            text.color = textColor;
            if (text.color.a <= 0)
                text.gameObject.SetActive(false);
            yield return null;
        }
    }
}
