
using UnityEngine;

namespace cemtas81
{
    public class SettlementSoldier : MonoBehaviour
    {
        private SettlementManager m_manager;    
        private void Start()
        {
            m_manager=SharedVariables.Instance.settlementManager;
        }
        private void OnDestroy()
        {
            m_manager.OnSoldierDie();
        }


    }
}
