using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(Animator))]

public class Player : MonoBehaviour
{
    //Declaration of Variables for movement
    //Declaration of Variables for movement - Commented code is for old move system
    public float speed = 5f, jumpSpeed = 8f;
    public bool isGrounded;
    public LayerMask layerMask;
    public bool moveWhileJump = true;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Transform playerTrans, groundTrans;

    //Declaraction of Variables for Mechanics
    public int playerHealth = 5;

    // Use this for initialization
    public void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        rigidBody = this.GetComponent<Rigidbody2D>();
        playerTrans = this.GetComponent<Transform>();

        groundTrans = GameObject.Find(this.name + "/tag_ground").transform;
    }

    public void Move(float horizontalInput)
    {
        if (!moveWhileJump && !isGrounded)
            return;
        Vector2 moveVelocity = rigidBody.velocity;
        moveVelocity.x = horizontalInput * speed;
        rigidBody.velocity = moveVelocity;
    }

    public void Jump()
    {
        if (isGrounded)
            rigidBody.velocity = jumpSpeed * Vector2.up;
    }


    public void FixedUpdate()
    {
        isGrounded = Physics2D.Linecast(rigidBody.position, groundTrans.position, layerMask);
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);


        Move(input.x);
        animator.SetFloat("Speed", (float)Mathf.Abs(input.x));

        if (input.x > 0)
            spriteRenderer.flipX = false;
        else if (input.x < 0)
            spriteRenderer.flipX = true;

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    //When the player touches an interactable object (IE: Batteries, maybe, or Flashlight Upgrades
    public void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "PowerUp":
                playerHealth++;
                other.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
