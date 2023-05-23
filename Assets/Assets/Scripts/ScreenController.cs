using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScreenController : MonoBehaviour {

	[SerializeField] private Slider healthSlider,coinSlider,aggroSlider;
	[SerializeField] private GameObject gameOverPanel,pausePanel,sagH,solH;	
	[SerializeField] private Text deadZombiesText,bossText,maxScoreText,scoreText,comboText;
	public GameObject upgradePanel;
	public int weaponLevel;
	private float maxScore,lastKillTime;
	private int deadZombiesCount;
	MyController control;
    private PlayerController playerController;
    [SerializeField] private int comboCount;
    [SerializeField] private float comboTimeLimit =.5f;
  
    private void Awake()
	{
		if (PlayerPrefs.GetInt("Hand")==0)
		{
			sagH.SetActive(true);
			solH.SetActive(false);	
		}
		else
		{
			sagH.SetActive(false);
			solH.SetActive(true) ;
		}
        control = new MyController();
        control.MyGameplay.Back.started += bck => Pause(); 
		control.MyGameplay.Submit.started += bck => Restart();
		
    }
	void Start () {
		playerController = FindObjectOfType<PlayerController>();
		
		maxScore = PlayerPrefs.GetFloat("MaxScore");
		coinSlider.value = 0;
		healthSlider.maxValue = playerController.playerStatus.health;
		UpdateHealthSlider();
		Time.timeScale = 1;
		
    }
	private void OnEnable()
	{
        control.MyGameplay.Enable();
    }
	private void OnDisable()
	{
		control.MyGameplay.Disable();
	}
	public void UpdateHealthSlider () {
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
	
    public void GameOver () {
		gameOverPanel.SetActive(true);
		Time.timeScale = 0;

		float time = Time.timeSinceLevelLoad;

		int minutes = (int)(time / 60);
		int seconds = (int)(time % 60);
		scoreText.text = "You survived for " + minutes + " minutes and " + seconds + " seconds.";
		UpdateMaxScore(minutes, seconds, time);
	}
	public void Pause()
	{
		if (pausePanel.activeInHierarchy==true&&upgradePanel.activeInHierarchy==false)
		{
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
		if (upgradePanel.activeInHierarchy==true)
		{
          upgradePanel.SetActive(false);
        }
		if (pausePanel.activeInHierarchy == false)
		{
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
		
    }

	public void Restart () 
	{
		//SceneManager.LoadScene("Game");
		if (gameOverPanel.activeInHierarchy==true)
		{
            SceneManager.LoadScene(1);
        }
       
    }

	public void UpdateDeadZombiesCount () {
		deadZombiesCount++;
		deadZombiesText.text = string.Format("x {0}", deadZombiesCount);
        if (Time.time - lastKillTime <= comboTimeLimit)
        {
            comboText.gameObject.SetActive(true);
            comboCount++;
            comboText.text = string.Format("{1}x combo", deadZombiesCount, comboCount);
            StartCoroutine(TextDisappear(2, comboText));
			if (aggroSlider.value==aggroSlider.maxValue)
			{
				Debug.Log("MAX_COMBOO!!!");
			}
        }
        else
        {
            comboCount = 1;
            comboText.text = string.Format("", deadZombiesCount);
        }

        lastKillTime = Time.time;
		aggroSlider.value = comboCount;
    }

	private void UpdateMaxScore (int minutes, int seconds, float time) {
		if (time > maxScore) {
			maxScore = time;
			maxScoreText.text = string.Format("Your best time is {0} minutes and {1} seconds.", minutes, seconds);
			PlayerPrefs.SetFloat("MaxScore", maxScore);
		} else {
			time = PlayerPrefs.GetFloat("MaxScore");
			minutes = (int)(time / 60);
			seconds = (int)(time % 60);
			maxScoreText.text = string.Format("Your best time is {0} minutes and {1} seconds.", minutes, seconds);
		}
	}

	public void ShowBossText() {
		StartCoroutine(TextDisappear(2, bossText));
	}

	private IEnumerator TextDisappear(float time,Text text) {
		text.gameObject.SetActive(true);
		Color textColor = text.color;
		textColor.a = 1;
		text.color = textColor;
		yield return new WaitForSeconds(1);
		float count = 0;
		while (text.color.a > 0) {
			count += Time.deltaTime / time;
			textColor.a = Mathf.Lerp(1, 0, count);
			text.color = textColor;
			if (text.color.a <= 0)
				text.gameObject.SetActive(false);
			yield return null;
		}
	}
}
