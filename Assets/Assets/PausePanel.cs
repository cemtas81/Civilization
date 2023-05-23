
using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject UpgradeP;
    MyController controller;
   
    private void Awake()
    {
        controller=new MyController();
        controller.MyGameplay.Geri.started += gri => Geriye();
        controller.MyGameplay.Menu.started+=gri=> UpgradeScreen();
        controller.MyGameplay.MainMenu.started += bck => MainMenu();
    }
    private void OnEnable()
    {
        controller.Enable();
      
    }
    void MainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    void Geriye()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
        controller.Disable();
        UpgradeP.SetActive(false);
    }
    public void UpgradeScreen()
    {
        UpgradeP.SetActive(!UpgradeP.activeSelf);
    }
}
