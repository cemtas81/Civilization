
using System.Collections.Generic;
using UnityEngine;

namespace cemtas81
{
    public class FlockingManager : MonoBehaviour
    {
        public List<GameObject> enemies; // Reference to your spawned enemies

        public float alignmentWeight = 1f;
        public float cohesionWeight = 1f;
        public float separationWeight = 1f;
        public float flockingRadius = 5f;
        private MySolidSpawner m_spawner;
        private void Start()
        {
            m_spawner = SharedVariables.Instance.spawner; 
        }
        void Update()
        {
            enemies=m_spawner.spawnedPrefabs;
            foreach (GameObject enemy in enemies)
            {
                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody == null)
                {
                    Debug.LogWarning("Enemy doesn't have a Rigidbody component!");
                    continue;
                }

                Vector3 alignment = CalculateAlignment(enemy);
                Vector3 cohesion = CalculateCohesion(enemy);
                Vector3 separation = CalculateSeparation(enemy);

                Vector3 flockingForce = alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight;

                enemyRigidbody.velocity += flockingForce * Time.deltaTime; // Update velocity based on flocking force
            }
        }

        Vector3 CalculateAlignment(GameObject enemy)
        {
            Vector3 averageDirection = Vector3.zero;
            int neighborCount = 0;

            foreach (GameObject otherEnemy in enemies)
            {
                if (otherEnemy != enemy)
                {
                    float distance = Vector3.Distance(enemy.transform.position, otherEnemy.transform.position);

                    if (distance <= flockingRadius)
                    {
                        Rigidbody otherRigidbody = otherEnemy.GetComponent<Rigidbody>();
                        if (otherRigidbody != null)
                        {
                            averageDirection += otherRigidbody.velocity.normalized;
                            neighborCount++;
                        }
                    }
                }
            }

            if (neighborCount > 0)
            {
                averageDirection /= neighborCount;
                return (averageDirection - enemy.GetComponent<Rigidbody>().velocity.normalized).normalized;
            }

            return Vector3.zero;
        }

        Vector3 CalculateCohesion(GameObject enemy)
        {
            Vector3 averagePosition = Vector3.zero;
            int neighborCount = 0;

            foreach (GameObject otherEnemy in enemies)
            {
                if (otherEnemy != enemy)
                {
                    float distance = Vector3.Distance(enemy.transform.position, otherEnemy.transform.position);

                    if (distance <= flockingRadius)
                    {
                        averagePosition += otherEnemy.transform.position;
                        neighborCount++;
                    }
                }
            }

            if (neighborCount > 0)
            {
                averagePosition /= neighborCount;
                return (averagePosition - enemy.transform.position).normalized;
            }

            return Vector3.zero;
        }

        Vector3 CalculateSeparation(GameObject enemy)
        {
            Vector3 separationDirection = Vector3.zero;

            foreach (GameObject otherEnemy in enemies)
            {
                if (otherEnemy != enemy)
                {
                    float distance = Vector3.Distance(enemy.transform.position, otherEnemy.transform.position);

                    if (distance <= flockingRadius)
                    {
                        separationDirection += (enemy.transform.position - otherEnemy.transform.position).normalized / distance;
                    }
                }
            }

            return separationDirection.normalized;
        }
    }
}
