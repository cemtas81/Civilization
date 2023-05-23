
using UnityEngine;

public class Slicer2 : MonoBehaviour,IUpgrade
{
    public Transform player;
    public float speed;
    public int Level=1;
    public void Upgrade(int level)
    {
        Level += level;
    }
    void Update()
    {
        transform.position = player.position;
        transform.Rotate(Level*speed * Time.deltaTime * Vector3.up);
              
    }
  
}
