using UnityEngine;
using UnityEngine.InputSystem;

public class sc_movementController : MonoBehaviour
{
    [HeaderAttribute("Movement Stats")]
    public float maxSpeed;
    public float accelerationScalar;
    public float deccelerationScalar;

    [HeaderAttribute("Rotation Stats")]
    [SerializeField] private float maxAngularSpeed;
    [SerializeField] private float angularAccelerationScalar;

    [HeaderAttribute("Linked Objects")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private sc_fieldOfView fovScript;

    private Rigidbody2D rb;

    
    InputAction mousePosition;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


        mousePosition = inputActions.FindAction("MousePosition");

    }
    
    private Vector2 moveInput;

    //Updates internal variable when a movement button is pressed.
    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();

    }


    private Vector2 moveDirection = new Vector2();

    void movePlayer()
    {

        /*Controls player movement
         * if there is no input, deccelerated based on set rate
         * when speed is very low, sets speed to 0
         * otherwise accelerates in direction of input
         * finally caps speed at maximum
        */
        if (moveInput.magnitude <= 0.1) 
        {
            moveDirection -= moveDirection.normalized * Time.deltaTime * deccelerationScalar;

            if (moveDirection.magnitude <= maxSpeed / 100)
            {
                moveDirection = Vector2.zero;
            }
        }
        else
        {
            moveDirection += moveInput * Time.deltaTime * accelerationScalar;
        }

        if (moveDirection.magnitude > maxSpeed)
        {
            moveDirection = moveDirection.normalized * maxSpeed;
        }


        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + moveDirection);
        
    }

    private float currDirection = 0f;
    private float currAngularSpeed = 0f;
    void rotatePlayerView()
    {
        Vector2 mouseOffset = getMousePos() - new Vector2(Display.main.renderingWidth / 2, Display.main.renderingHeight / 2);
        float targetDirection = calcTargetDirection(mouseOffset);

        if(Mathf.Abs(targetDirection - currDirection) < 1 || Mathf.Abs(targetDirection - currDirection) > 359)
        {
            Debug.Log("close enough");
            currDirection = targetDirection;
            currAngularSpeed = 0f;
        }
        else
        {

            if((currDirection > targetDirection && currDirection - targetDirection < 180) || (currDirection < targetDirection && targetDirection - currDirection > 180))
            {

                currAngularSpeed -= angularAccelerationScalar;
                Mathf.Clamp(currAngularSpeed, -maxAngularSpeed, maxAngularSpeed);
            }
            else
            {

                currAngularSpeed += angularAccelerationScalar;
                Mathf.Clamp(currAngularSpeed, -maxAngularSpeed, maxAngularSpeed);
            }
            //currDirection += currAngularSpeed;
        }
        currDirection += currAngularSpeed;

    }



    void Update()
    {
        movePlayer();
        fovScript.setOrigin(transform.position);

        rotatePlayerView();
        fovScript.setFacingDirection(currDirection);
        

    }



    public float calcTargetDirection(Vector3 newFacingDirection)
    {
        newFacingDirection = newFacingDirection.normalized;
        float newAngle = Mathf.Atan2(newFacingDirection.y, newFacingDirection.x) * Mathf.Rad2Deg;
        if (newAngle < 0) newAngle += 360;
   
        return newAngle;
    }


    //Getters and Setters for encapsulation
    public Vector2 getMoveDirection()
    {
        return moveDirection;
    }

    public Vector2 getMousePos()
    {
        return mousePosition.ReadValue<Vector2>();
    }
}
