

using UnityEngine;

public class AimLock : MonoBehaviour
{
    [SerializeField] private float range;
    public Vector3 target;
    public Vector3 screenPos; 
    private void Update()
    {

        Collider[] colliders = new Collider[26];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, colliders);
        float closestDistance = Mathf.Infinity;
        Vector3 closestPosition = Vector3.zero;

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliders[i];
            if (collider.TryGetComponent<BarbEnemyCont>(out BarbEnemyCont enemy))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestPosition = enemy.transform.position;
                }
            }
            else if (collider.TryGetComponent<BossCont2>(out BossCont2 enemy2))
            {
                float distanceToEnemy2 = Vector3.Distance(transform.position, enemy2.transform.position);
                if (distanceToEnemy2 < closestDistance)
                {
                    closestDistance = distanceToEnemy2;
                    closestPosition = enemy2.transform.position;
                }
            }
        }
        target = closestPosition;

        if (target != Vector3.zero)
        {
            transform.position = target;
            screenPos = Camera.main.WorldToScreenPoint(target);
        }
        else
        {
            //transform.position = transform.parent.position;
            screenPos = Vector3.zero;
        }
    }


}

