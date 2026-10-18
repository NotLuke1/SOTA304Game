using UnityEngine;
using UnityEngine.InputSystem;

public class sc_movementController : MonoBehaviour
{
    [HeaderAttribute("Movement Stats")]
    public float maxSpeed;
    public float accelerationScalar;
    public float deccelerationScalar;


    private Rigidbody2D rb;

    [SerializeField] private InputActionAsset inputActions;
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

    void Update()
    {
        movePlayer();


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
