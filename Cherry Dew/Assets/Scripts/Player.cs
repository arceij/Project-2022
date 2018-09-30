using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(Animator))]

public class Player : MonoBehaviour
{
    //Declaration of Variables for movement
    public float speed = 5f;
    public float accel = 3f;
    private Vector2 input;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private Animator animator;

    //Jumping Variables
    public bool isJumping;
    public float jumpSpeed = 8f;
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;
    public float jumpDurationThreshold = 0.05f;
    private float jumpDuration;
    public float airAccel = 1f;

    //Declaraction of Variables for Mechanics
    public int playerHealth = 5;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.01f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }

    // Use this for initialization
    public void Start ()
    {
        playerHealth = 5;
	}

    //Update the movement code of the player
    public void FixedUpdate()
    {
        var acceleration = 0f;
        var xVelocity = 0f;

        if (PlayerIsOnGround())
        {
            acceleration = accel;
        }
        else
        {
            acceleration = airAccel;
        }

        //Check input of Horizontal Movement
        if (PlayerIsOnGround() && input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rigidBody.velocity.x;
        }

        //Check input of Horizontal Movement
        if (input.x ==0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rigidBody.velocity.x;
        }

        //move Player
        rigidBody.AddForce(new Vector2(((input.x * speed) - rigidBody.velocity.x) * acceleration, 0));
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        //jumping!
        if (isJumping && jumpDuration < jumpDurationThreshold)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }
    }

    //Jumping check Function - is the player able to jump?
    public bool PlayerIsOnGround()
    {
        bool groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height), -Vector2.up, rayCastLengthCheck);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height), -Vector2.up, rayCastLengthCheck);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height), -Vector2.up, rayCastLengthCheck);

        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    // Update is called once per frame
    public void Update ()
    {
        //Receive player input
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");

        animator.SetFloat("Speed", Mathf.Abs(input.x));

        //Handle Horizontal Movement
        if (input.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (input.x < 0f)
        {
            spriteRenderer.flipX = true;            
        }

        //Jump mechanics
        if (input.y >= 1f)
        {
            jumpDuration += Time.deltaTime;
        }
        else
        {
            isJumping = false;
            jumpDuration = 0f;
        }

        //checks if the player is on the ground and able to jump
        if (PlayerIsOnGround() && isJumping == false)
        {
            if (input.y > 0f)
            {
                isJumping = true;
            }
        }

        if (jumpDuration > jumpDurationThreshold) input.y = 0f;
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
