//using PlayerNameSpace;

using UnityEngine;


public class GetCoin : MonoBehaviour
{
    private ScreenController slider;
    private void Start()
    {
        slider =FindObjectOfType<ScreenController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            
            slider.UpdateCoinSlider(10);
        }
    }
    void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
