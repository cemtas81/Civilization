using System.Collections;
using UnityEngine;

public class Icecle : MonoBehaviour, IUpgrade
{
    public float activeDuration = 3.0f, inactiveDuration = 5.0f;
    public Transform Player;
    public GameObject targetObject;
    public int Level;
    public void Upgrade(int level)
    {
        Level += level;
    }
    private void OnEnable()
    {
        StartCoroutine(ActivateObject());       
    }

    private IEnumerator ActivateObject()
    {
        while (true)
        {
            targetObject.transform.position = Player.position;

            targetObject.SetActive(true);
            yield return new WaitForSeconds(activeDuration);
            targetObject.SetActive(false);
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

}
