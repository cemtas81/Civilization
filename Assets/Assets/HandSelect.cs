
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandSelect : MonoBehaviour
{
    public GameObject handSag,handSol,handSelectSag, handSelectSol;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Hand")==0)
        {
            handSag.SetActive(true);
            handSelectSag.SetActive(false);
            handSelectSol.SetActive(true);
            handSol.SetActive(false);    
        }
        else
        {
            handSag.SetActive(false);
            handSol.SetActive(true);
            handSelectSol.SetActive(false);
            handSelectSag.SetActive(true) ;
        }
        
    }
    public void Sag()
    {
        PlayerPrefs.SetInt("Hand", 0);
        handSag.SetActive(true);
        handSelectSag.SetActive(false);
        handSelectSol.SetActive(true);
        handSol.SetActive(false);
        StartCoroutine(StartL());
    }
    public void Sol()
    {
        PlayerPrefs.SetInt("Hand", 1);
        handSag.SetActive(false);
        handSol.SetActive(true);
        handSelectSol.SetActive(false);
        handSelectSag.SetActive(true);
        StartCoroutine(StartL());
    }
    IEnumerator StartL()
    {
        PlayerPrefs.Save();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(1);
    }
}
