
using UnityEngine;

namespace cemtas81
{
    public class GatheringPoint : MonoBehaviour
    {
        //public bool gather;
        private Transform target;
        private void Start()
        {
            target = transform;
            SharedVariables.Instance.gatherPoint = target;
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        gather=true;
        //        SharedVariables.Instance.gathering=gather;
                
        //    }
        //}
        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        gather = false;
        //        SharedVariables.Instance.gathering = gather;

        //    }
        //}
    }
}
