using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
namespace cemtas81
{
    public class SettlementManager : MonoBehaviour
    {
        private MySolidSpawner spawner;
        public GameObject dust,fire;
        public bool isHere;
        private BarbCont2 barbar;
        private Transform player;
        public float dist;
        private bool couldBurn;
        public int spawns;
        public List<SettlementSpawner> spawnPoints;
        private void Start()
        {
            NavMesh.avoidancePredictionTime = 0.1f;
            spawner = SharedVariables.Instance.spawner;
            SharedVariables.Instance.settlementManager = this;
            barbar = SharedVariables.Instance.cont;
            player = barbar.GetComponent<Transform>();
            StartCoroutine(SpawnCoroutine());
        }
        IEnumerator SpawnCoroutine()
        {
            while (gameObject.activeInHierarchy)
            {
                foreach (SettlementSpawner settlementSpawner in spawnPoints)
                {
                    if (settlementSpawner.soldiers < settlementSpawner.maxSoldier && isHere)
                    {
                        yield return new WaitForSeconds(settlementSpawner.spawnInterval);
                        Instantiate(settlementSpawner.prefab, settlementSpawner.transform.position, Quaternion.identity);
                        settlementSpawner.soldiers++;
                        spawns++;
                    }
                    else if(settlementSpawner.soldiers==settlementSpawner.maxSoldier)
                    {
                        settlementSpawner.fire.SetActive(true);
                    }
                }

                yield return null;
            }
   
        }
        public IEnumerator Destruction()
        {
            yield return new WaitForSeconds(1);
            transform.DOMoveY(transform.position.y - 8f, 3f);
            Instantiate(dust, transform.position, Quaternion.identity);
            //dust.SetActive(true);
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
            spawner.BossHere = false;
        }
        private void Update()
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= dist)
            {
                isHere = true;
            }
            else
            {
                isHere = false;
            }
        
        }
        public void OnSoldierDie()
        {
            if (spawns>1)
            {
                spawns--;
            }
            else
            {
                couldBurn = true;
                Burn();
            }
        }
      
        private void Burn()
        {
            StartCoroutine(Destruction());
            //fire.SetActive(true);
        }
    }
}
