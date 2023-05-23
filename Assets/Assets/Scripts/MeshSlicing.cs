using EzySlice;
using UnityEngine;


public class MeshSlicing : MonoBehaviour
{
    public Material materialSlicedSide;
    public float explosionForce;
    public float explosionRadius;
    public bool gravity, kinematic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sliceable"))
        {
            SlicedHull sliceobj = Slice(other.gameObject, materialSlicedSide);
            GameObject slicedObjTop = sliceobj.CreateUpperHull(other.gameObject, materialSlicedSide);
            GameObject slicedObjDown = sliceobj.CreateLowerHull(other.gameObject, materialSlicedSide);
            Destroy(other.gameObject);
            AddComponent(slicedObjTop);
            AddComponent(slicedObjDown);

        }
    }
    private SlicedHull Slice(GameObject obj, Material mat)
    {
        return obj.Slice(transform.position, transform.up, mat);
    }
    void AddComponent(GameObject obj)
    {
        obj.AddComponent<BoxCollider>();
        var rigidbody = obj.AddComponent<Rigidbody>();
        rigidbody.isKinematic = kinematic;
        rigidbody.useGravity = gravity;
        rigidbody.AddExplosionForce(explosionForce, obj.transform.position, explosionRadius);
        obj.tag = "Sliceable";
    }
}
