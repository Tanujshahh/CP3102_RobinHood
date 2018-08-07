using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpForce;
    private float moveHorizontal;
    private float moveVertical;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isClimbing = false;
    private bool isHidden = false;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public float distance;



    private bool hasJumped = false;

	void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate()
	{
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        if (hitInfo.collider != null) {
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                isClimbing = true;
            }
        }
        else {
            isClimbing = false;
        }

        if(isClimbing) {
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.position.x, moveVertical * speed);
            rb.gravityScale = 0;
        }
        else {
            rb.gravityScale = 1;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        Debug.Log(moveHorizontal);
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        if (!facingRight && moveHorizontal > 0) {
            Flip();
        }
        else if (facingRight && moveHorizontal < 0 ) {
            Flip();
        }
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("cover")) {
            SpriteRenderer coverSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Color color = coverSprite.color;
            color.a = 0.7f;
            coverSprite.color = color;
            isHidden = true;
        }
	}

	void OnTriggerExit2D(Collider2D collision)
	{
        if(collision.CompareTag("cover")) {
            SpriteRenderer coverSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Color color = coverSprite.color;
            color.a = 1.0f;
            coverSprite.color = color;
            isHidden = false;
        }
	}

	void Update()
	{
        //if (isGrounded) {
        //    hasJumped = false;
        //}

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.velocity = Vector2.up * jumpForce;
            //hasJumped = true;
        }
	}

	void Flip() {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    public bool hidden() {
        return isHidden;
    }
}
