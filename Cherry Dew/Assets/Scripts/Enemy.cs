using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Movement controls
    public float speed;
    public LayerMask enemyMask;
    private Rigidbody2D rb2D;
    private Transform enemyTransform;
    private float enemyWidth, enemyHeight;
    public float forwardCheck = 0.05f;
    
    //Variables for the implimentation of mechanics
    public int playerDamage;

	// Use this for initialization
	public void Start ()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        enemyTransform = this.GetComponent<Transform>();
        enemyWidth = this.GetComponent<PolygonCollider2D>().bounds.extents.x;
        enemyHeight = this.GetComponent<PolygonCollider2D>().bounds.extents.y;
	}

    private Vector2 convertto2D(Vector3 temp)
    {
        return new Vector2(temp.x, temp.y);

    }

    public void FixedUpdate()
    {
        Vector2 lineCastPos = enemyTransform.position - enemyTransform.right * enemyWidth;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down * .5f, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos - convertto2D(enemyTransform.right) * forwardCheck);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - convertto2D(enemyTransform.right) * forwardCheck, enemyMask);

        //If there is no ground or enemy is blocked, turn around!
        if(!isGrounded || isBlocked)
        {
            Vector3 currRot = enemyTransform.eulerAngles;
            currRot.y += 180;
            enemyTransform.eulerAngles = currRot;
        }

        //For now, always move forward
        Vector2 enemyVelocity = rb2D.velocity;
        enemyVelocity.x = -enemyTransform.right.x * speed;
        rb2D.velocity = enemyVelocity;
    }
}
