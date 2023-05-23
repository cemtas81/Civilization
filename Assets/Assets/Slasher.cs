
using UnityEngine;

public class Slasher : MonoBehaviour
{
    GameObject player;
    Vector3 locationInFrontOfPlayer;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        locationInFrontOfPlayer =player.transform.position + player.transform.forward * 10f;
        Destroy(gameObject,0.3f);
    }
    private void Update()
    {
        transform.Rotate( 0, 0, Time.deltaTime * 2000);
        transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 30);
    }

}
