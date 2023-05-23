
using UnityEngine;
using UnityEngine.EventSystems;
public class PanelOn : MonoBehaviour
{
    public GameObject firstButton;
  
    private void OnEnable()
    {      
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(firstButton);
       
    }

}
