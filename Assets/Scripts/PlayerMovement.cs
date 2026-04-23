using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("PlayerMovement requires a Rigidbody2D on the same GameObject.");
      
        }
    }

    void Update()
    {
        if (Keyboard.current == null)
        {
            movementInput = Vector2.zero;
            return;
        }

        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveX = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveX = 1f;
        }

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            moveY = -1f;
        }
        else if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            moveY = 1f;
        }

        // Input 2D.
        movementInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        rb.angularVelocity = 0f;
        rb.linearVelocity = movementInput * moveSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        // Cambia la velocidad actual.
        moveSpeed = newSpeed;
    }
}
