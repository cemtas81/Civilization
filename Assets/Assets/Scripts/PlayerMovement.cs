

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement {
    
    //public StarterAssetsInputs joy;
    public GameObject pad;
    [SerializeField] private float range;
    public Vector3 target;
    public float speed = 2.0f,turnSpeed, nextUpdate;
    private Quaternion qTo;
    private Camera m_camera;
    public Transform aim;
    public Texture2D texture,texture2;
    private void Start()
    {
        qTo = transform.rotation;
        m_camera = Camera.main; 

    }
    private void Update()
    {
#if UNITY_ANDROID || UNITY_IPHONE
       
        if (Time.time >= nextUpdate)
        {
           
            nextUpdate = Mathf.FloorToInt(Time.time) + 0.5f;
           
            UpdateTarget();
        }

#endif
    }
    void UpdateTarget()
    {
        Collider[] colliders = new Collider[150];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, colliders);
        float closestDistance = Mathf.Infinity;
        Vector3 closestPosition = Vector3.zero;

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliders[i];
            if (collider.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestPosition = enemy.transform.position;
                }
            }
            else if (collider.TryGetComponent<BossCont2>(out BossCont2 enemy2))
            {
                float distanceToEnemy2 = Vector3.Distance(transform.position, enemy2.transform.position);
                if (distanceToEnemy2 < closestDistance)
                {
                    closestDistance = distanceToEnemy2;
                    closestPosition = enemy2.transform.position;
                }
            }
        }
        target = closestPosition;
    }

    public void PlayerRotation(LayerMask groundMask)
    {
#if UNITY_STANDALONE
        //makes the player rotation follows the mouse position
        // it uses a LayerMask that computes only the Raycasts that collide with the ground
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null)
        {

            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundMask))
            {
                //Vector3 positionPoint = Vector3.Slerp(transform.position, hit.point - transform.position, turnSpeed * Time.deltaTime);
                Vector3 positionPoint = hit.point - transform.position;
                //positionPoint.y = transform.position.y;
                positionPoint.y = 0;
                //joy.cursorInputForLook = false;
           
                //pad.SetActive(false);
                Rotation(positionPoint);
                target = positionPoint;
                //if (hit.collider.gameObject.CompareTag("Enemy")|| hit.collider.gameObject.CompareTag("Boss"))
                //{
                //    Cursor.SetCursor(texture, new Vector2(Screen.width * .02f, Screen.height * .020f), CursorMode.Auto);
                //}
                //else
                //{
                //    Cursor.SetCursor(texture2, new Vector2(Screen.width*.02f,Screen.height*.02f), CursorMode.Auto);
                //}
                    
            }
        }

        else if (gamepad.rightStick.IsActuated(0F))
        {
            //joy.cursorInputForLook = true;
            //Vector3 direction = new Vector3(joy.look.x, 0, joy.look.y * -1);
            //qTo = Quaternion.LookRotation(direction);
            //Quaternion smoothdir = Quaternion.Slerp(transform.rotation, qTo, speed * Time.deltaTime);
            ////transform.rotation = Quaternion.Slerp(transform.rotation, qTo, speed * Time.deltaTime);
            ////for character movement                    
            //Rotation(smoothdir);
            //joy.cursorInputForLook = true;
            Vector3 positionPoint = aim.position - transform.position;
            //positionPoint.y = transform.position.y;
            positionPoint.y = 0;
            Rotation(positionPoint);
            target = positionPoint;
        }
       
#endif

#if UNITY_ANDROID || UNITY_IPHONE
       
        //transform.LookAt(transform.position + new Vector3(joy.look.x, 0, joy.look.y)); 
        //transform.LookAt(target);
        Vector3 lookPos = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookPos);

        float smoothTime = 10f; // adjust as needed
        float t = smoothTime * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);

#endif
    }
  
}
