using UnityEngine;
using UnityEngine.InputSystem;

public class sc_movementController : MonoBehaviour
{
    [HeaderAttribute("Movement Stats")]
    public float maxSpeed;
    public float accelerationScalar;
    public float deccelerationScalar;


    private InputAction moveAction;
    void Awake()
    {
        //moveAction = InputSystem.actions.FindAction("Movement");
    }

    
    private Vector2 moveInput;
    public void OnMove(InputAction.CallbackContext context)
    {
        

        moveInput = context.ReadValue<Vector2>();

    }


    private Vector2 moveDirection = new Vector2();

    void movePlayer()
    {
        Debug.Log(moveInput);
        if (moveInput.magnitude == 0) 
        {
            //moveDirection -= moveDirection.normalized * Time.deltaTime * deccelerationScalar;
            moveDirection = Vector2.zero;
        }
        else if (moveDirection.magnitude <= maxSpeed)
        {
            moveDirection += moveInput * Time.deltaTime * accelerationScalar;
        }

        if (moveDirection.magnitude > maxSpeed)
        {
            moveDirection = moveDirection.normalized * maxSpeed;
        }

        Debug.Log(moveDirection);
        transform.Translate(moveDirection);
    }

    void Update()
    {
        movePlayer();
    }
}
