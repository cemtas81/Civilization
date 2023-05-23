
using UnityEngine;

using UnityEngine.UI;
using TMPro;


public class UpgradePanel : MonoBehaviour
{
    MyController controller;
    public WeaponUp[] weaponUp;
    public Image sprite;
    public TextMeshProUGUI text;
    public WeaponUp recentWeapon;
    public PlayerController playerController;
    
    private void Awake()
    {
        controller=new MyController();
        controller.MyGameplay.Submit.started += sbm => ClosePanel();
       
    }
    private void OnEnable()
    {
        int randomIndex = Random.Range(0, weaponUp.Length);
        recentWeapon = weaponUp[randomIndex];
        controller.Enable();
        Time.timeScale = 0;
        text.text = recentWeapon.skill+(" Lvl")+recentWeapon.skillLevel;
        sprite.sprite = recentWeapon.sprite;
        playerController.Spell(recentWeapon.skillNumber);
    }
    private void OnDisable()
    {
        controller.Disable();
    }
    public void ClosePanel()
    {
        
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
