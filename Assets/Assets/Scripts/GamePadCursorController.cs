
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamePadCursorController : MonoBehaviour
{
    public float cursorSpeed = 5f; // Adjust the cursor movement speed as needed
    private RectTransform cursorObject; // Assign a UI image or object representing the cursor
    private Vector2 cursorPosition;
    public LayerMask mask;
    Ray ray;
    Camera cam;
    public Transform aim;
    //public StarterAssetsInputs joy;
    private MyController myController;
    private InputAction myAction;   
    private Image joyCursor;
    private AimLock myLock;
    private bool hitted;
    private Vector3 newPosition;

    private void Awake()
    {
        myController = new MyController();
        cam = Camera.main;   
        cursorObject = GetComponent<RectTransform>();   
        joyCursor=GetComponent<Image>();
        cursorPosition = ClampToScreen(Input.mousePosition);
        //cursorPosition=Input.mousePosition;
        AimLock Lock=(AimLock)FindObjectOfType(typeof(AimLock));
        if (Lock)
        {
            myLock = Lock;
        }
    }
    private void OnEnable()
    {
        joyCursor.enabled = true;
        myController.Enable();
        myAction = myController.MyGameplay.Rotate;
    }
    void Update()
    {
    
        if (myLock!=null&& myLock.screenPos != Vector3.zero&&hitted==false&&myAction.ReadValue<Vector2>().magnitude<.5f)
        {
            cursorObject.position = myLock.screenPos;
            
        }
       
        else
        {
            // Get the cursor movement input from the "MoveCursor" action
            Vector2 movementInput = myAction.ReadValue<Vector2>();
            // Update the cursor position based on the input
            cursorPosition += cursorSpeed * Time.deltaTime * movementInput;

            // Apply clamping to ensure the cursor stays within the screen bounds
            cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0f, Screen.width);
            cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0f, Screen.height);

            // Set the position of the cursor object
            cursorObject.position = cursorPosition;
            ray = cam.ScreenPointToRay(cursorPosition);

            //Vector3 newPosition;
        }
 
        // Fire a ray through the scene at the mouse position and place the target where it hits
        if (Physics.Raycast(ray, out RaycastHit hit, 100, mask))
        {

            newPosition = hit.point;
            aim.position = newPosition;
            if (hit.collider.gameObject.CompareTag("Enemy")|| hit.collider.gameObject.CompareTag("Boss"))
            {
                joyCursor.color = Color.red;
                hitted = true;
            }
            else
                joyCursor.color = Color.white;
                hitted = false;
           
        }

    }
    private Vector2 ClampToScreen(Vector2 position)
    {
        // Get the screen edges in pixels
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Clamp the position to the screen edges
        position.x = Mathf.Clamp(position.x, 0f, screenWidth);
        position.y = Mathf.Clamp(position.y, 0f, screenHeight);

        return position;
    }
    private void OnDisable()
    {
       
        joyCursor.enabled=false;
        myController.Disable();
    }
}

