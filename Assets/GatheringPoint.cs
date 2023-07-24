
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

    }
}
