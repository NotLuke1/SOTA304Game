using UnityEngine;
using UnityEngine.InputSystem;

public class sc_movementController : MonoBehaviour
{
    [HeaderAttribute("Movement Stats")]
    public float maxSpeed;
    public float accelerationScalar;
    public float deccelerationScalar;


    public Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private Vector2 moveInput;
    public void OnMove(InputAction.CallbackContext context)
    {
        

        moveInput = context.ReadValue<Vector2>();

    }


    private Vector2 moveDirection = new Vector2();

    void movePlayer()
    {

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

        Debug.Log(moveDirection.magnitude);

        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + moveDirection);
        
    }

    void Update()
    {
        movePlayer();
    }



    public Vector2 getMoveDirection()
    {
        return moveDirection;
    }
}
