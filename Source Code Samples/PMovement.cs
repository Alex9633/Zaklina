using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;

    Rigidbody2D rb;

    private bool isGrounded = false, stunned = false;
    public bool invincible = false;
    public Transform isGroundedChecker, wallChecker, sprite;
    public LayerMask groundLayer;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public float rotationSpeed = 180f;
    public float rotationTime = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!stunned)
        {
            Move();
            Jump();
            rotate();
        }
        CheckIfGrounded();
        rb.rotation = 0f;
    }
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        if (moveBy != 0) rb.velocity = new Vector2(moveBy, rb.velocity.y);
        else rb.velocity = new Vector2(0, rb.velocity.y);

        //animation idle when against wall
        
        Collider2D collider = Physics2D.OverlapBox(wallChecker.position, new Vector2(1, 0) * 2.7f, 0f, groundLayer);
        if (collider != null)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(moveBy));
        }
        
    }
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            StartCoroutine(JumpRotate());
        }
    }
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapBox(isGroundedChecker.position, new Vector2(1, 0) * 2.4f, 0f, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
            animator.SetBool("IsInAir", false);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("IsInAir", true);
        }
    }
    void rotate()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private IEnumerator JumpRotate()
    {
        Quaternion currentRotation = sprite.transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            float rotationAngle = rotationSpeed * Time.deltaTime;
            sprite.transform.Rotate(Vector3.forward, rotationAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);
        sprite.transform.localRotation = currentRotation;
    }

    public void TakeDamage()
    {
        rb.velocity = new Vector2(-8, 5);
        StartCoroutine(Blink());
        StartCoroutine(Stunned());
        StartCoroutine(Invincibility());
    }

    private IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSecondsRealtime(2);
        invincible = false;
        yield return null;
    }

    private IEnumerator Stunned()
    {
        stunned = true;
        yield return new WaitForSecondsRealtime(1);
        stunned = false;
        yield return null;
    }

    private IEnumerator Blink()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(.5f);
        spriteRenderer.color = Color.white;
        yield return null;
    }
}