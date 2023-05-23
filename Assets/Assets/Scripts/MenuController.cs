using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	[SerializeField] private GameObject quitButton;
	MyController actions;
	
	public GameObject optionsPanel,creditsPanel,quitPanel, storyPanel, controlPanel;
	public GameObject StartFirstB,optionsFirstB;
    // Use this for initialization
    private void Awake()
	{
		EventSystem.current.SetSelectedGameObject(StartFirstB);
		actions=new MyController();
		actions.MyGameplay.Submit.started+=sbm=>SubmitAct();
       
        actions.MyGameplay.Geri.started+=sbm=>Back();
	}
	private void OnEnable()
	{
		actions.Enable();
	}
	private void OnDisable()
	{
		actions.Disable();
	}
	void Start()
	{
//#if UNITY_STANDALONE || UNITY_EDITOR
//		quitButton.SetActive(true);
//#endif
Time.timeScale = 1.0f;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Back();
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (quitPanel.activeInHierarchy == false&&controlPanel.activeInHierarchy==false)
			{
                SubmitAct();
            }

        }
	}
	public void Back()
	{
		if (!storyPanel.activeInHierarchy)
		{
            optionsPanel.SetActive(false);
            controlPanel.SetActive(false);
            creditsPanel.SetActive(false);
            quitPanel.SetActive(false);
        }
		EventSystem.current.SetSelectedGameObject(StartFirstB);
	}
	public void SubmitAct()
	{
	
		if (storyPanel.activeInHierarchy)
		{
            controlPanel.SetActive(true);
  
        }
		
		if (quitPanel.activeInHierarchy)
		{
			QuitGame();
		}
		if (controlPanel.activeInHierarchy )
		{
			PlayGame();
		}
		
	}
	
	public void PlayGame() 
	{

		//StartCoroutine(ChangeScene("Procedural game"));
		StartCoroutine(ChangeScene(1));
    }

	IEnumerator ChangeScene(int scene) {
		yield return new WaitForSeconds(.5f);
		SceneManager.LoadScene(scene);
	}

	public void QuitGame() 
	{
		StartCoroutine(Quit());
	}
	
	IEnumerator Quit () {
		yield return new WaitForSeconds(.2f);
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
