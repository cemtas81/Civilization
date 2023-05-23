using UnityEngine;

public class SatelliteController : MonoBehaviour
{
    public Transform player;
    public float circleSpeed = 10.0f;
    public float circleRadius = 5.0f;

    private Vector3 circleCentre;
    private float angle;

    private void Start()
    {
        circleCentre = player.position;
    }

    private void Update()
    {
        circleCentre = player.position;
        angle += Time.deltaTime * circleSpeed;
        Vector3 newPosition = new Vector3(
            circleCentre.x + Mathf.Cos(angle) * circleRadius,
            transform.position.y,
            circleCentre.z + Mathf.Sin(angle) * circleRadius
        );
        transform.position = newPosition;
        transform.LookAt(player);
    }
    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Enemy":
                EnemyController enemy = other.GetComponent<EnemyController>();
                enemy.LoseHealth(1);
                enemy.BloodParticle(transform.position, rotation);
                break;
            case "Boss":
                BossCont2 boss = other.GetComponent<BossCont2>();
                boss.LoseHealth(1);
                boss.BloodParticle(transform.position, rotation);
                break;
        }


    }
}

