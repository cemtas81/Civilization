
using UnityEngine;

public class HeadCoin : MonoBehaviour
{
  
    private BarbScreenCont slider;
  
    private void Awake()
    {
        slider = FindObjectOfType<BarbScreenCont>();
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);

            slider.UpdateHead(1);
        }
    }
    void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
