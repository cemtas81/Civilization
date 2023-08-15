using UnityEngine;

public class ItemMover : MonoBehaviour
{ 
    private Vector3 targetPosition;
    public Transform target;
    void Update()
    {


        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.y = 0f;
        Move();


    }
    void Move()
    {
        target.localPosition = targetPosition;
    }

}
