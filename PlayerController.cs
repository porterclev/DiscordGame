using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    public bool haltMovement = false;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        if (movementInput.y != 0)
        {
            movementInput.x = 0;
        }
        if (movementInput.x != 0)
        {
            movementInput.y = 0;
        }

    
    }

    private void FixedUpdate()
    {

        if (movementInput != Vector2.zero && !haltMovement)
        {
            bool sucess = TryMove(movementInput);

            if (!sucess)
            {
                sucess = TryMove(new Vector2(movementInput.x, 0));
            }
            if (!sucess)
            {
                sucess = TryMove(new Vector2(0, movementInput.y));
            }

            // if (movementInput.y > 0)
            // {
            //     animationClear();
            //     animator.SetBool("isMovingUp", sucess);
            // }
            // if (movementInput.y < 0)
            // {
            //     animationClear();
            //     animator.SetBool("isMovingDown", sucess);
            // }
            // if (movementInput.x < 0)
            // {
            //     animationClear();
            //     animator.SetBool("isMovingLeft", sucess);
            // }
            // if (movementInput.x > 0)
            // {
            //     animationClear();
            //     animator.SetBool("isMoving", sucess);
            // }
        }
        else
        {
            // animationClear();
        }

    }
    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
               direction,
               movementFilter,
               castCollisions,
               moveSpeed * Time.fixedDeltaTime + collisionOffset
           );

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }



    }

    private void animationClear()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isMovingUp", false);
        animator.SetBool("isMovingDown", false);
        animator.SetBool("isMovingLeft", false);
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
