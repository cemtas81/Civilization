
using UnityEngine;
using UnityEngine.AI;

namespace cemtas81
{
    public class SettlementSoldier : MonoBehaviour
    {
        private SettlementManager m_manager;
        private NavMeshAgent m_agent;
        public float maxSpeed;
        public float minSpeed;
        private void Start()
        {
            m_manager=SharedVariables.Instance.settlementManager;
            m_manager.spawns++;
            m_agent=GetComponent<NavMeshAgent>();
            if (m_agent!=null)
            {
                m_agent.speed = Random.Range(minSpeed, maxSpeed);
            }
            
        }
        private void OnDestroy()
        {
            m_manager.OnSoldierDie();
        }


    }
}
