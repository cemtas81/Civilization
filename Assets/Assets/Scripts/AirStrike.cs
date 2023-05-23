using System.Collections;
using UnityEngine;

public class AirStrike : MonoBehaviour,IUpgrade
{
    public GameObject bombPrefab;
    public GameObject targetObject;
    public float bombDropInterval = 0.1f;
    public float repeatInterval = 10.0f;
    public int bombDropCount = 10;
    public Vector3 offsetFromTarget = new Vector3(0, 10, 0);
    public int Level;
    public void Upgrade(int level)
    {
        Level += level;
    }

    private void OnEnable()
    {
        StartCoroutine(DropBombs());
    }
    private IEnumerator DropBombs()
    {
        while (true)
        {
            for (int i = 0; i < bombDropCount; i++)
            {
                Vector3 targetPos = targetObject.transform.position + offsetFromTarget;
                Vector3 dropPos = new Vector3(targetPos.x, transform.position.y+offsetFromTarget.y, targetPos.z);
                GameObject bomb = Instantiate(bombPrefab, dropPos, Quaternion.identity);
                bomb.GetComponent<Rigidbody>().AddForce((targetPos - dropPos).normalized * 1750.0f);
                yield return new WaitForSeconds(bombDropInterval);
            }
            yield return new WaitForSeconds(repeatInterval);
        }
    }
}






