
using UnityEngine;

namespace cemtas81
{
    public class GatheringPoint : MonoBehaviour
    {
        public bool gather;
        public Transform target;
        private void Start()
        {
            SharedVariables.Instance.gatherPoint = target;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gather=!gather;
                SharedVariables.Instance.gathering=gather;
                
            }
        }
    }
}
