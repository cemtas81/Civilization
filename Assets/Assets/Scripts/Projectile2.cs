using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rigidbodyProjectile;
    private PlayerController playerController;
    void Start()
    {
        rigidbodyProjectile = GetComponent<Rigidbody>();
        playerController =FindObjectOfType<PlayerController>();
        Destroy(gameObject, 6);
    }

    void FixedUpdate()
    {
        // moves the projectile forward using physics (rigidbody)
        rigidbodyProjectile.MovePosition(
        rigidbodyProjectile.position + (speed * Time.deltaTime * transform.forward));
    }

    // Destroy the projectile and the enemy
    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Player":
               
                playerController.LoseHealth(10);
                Destroy(gameObject);
                break;
            case "Enemy":
                EnemyController enemy2 = other.GetComponent<EnemyController>();
                enemy2.LoseHealth(3);
                enemy2.BloodParticle(transform.position, rotation);
                Destroy(gameObject);
                break;
        }
        
    }
}
