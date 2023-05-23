using UnityEngine.EventSystems;
using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    public GameObject firstSlider;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSlider);
    }
}
