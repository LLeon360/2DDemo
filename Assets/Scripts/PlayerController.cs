using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Component references
    // TODO: create variable to store rigidbody of player (2D)
    // TODO: create variable storing the Animator
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 5f;

    //TODO: keep track of current horizontal movement direction
    private float direction;

    //keep track of if the player is on the ground
    private bool isGrounded = false;

    //TODO: keep track of which direction player is facing
    private bool isFacingRight = true;

    // create a list for contact points, reused in OnCollisionStay2D
    private List<ContactPoint2D> contactList;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Get references to the rigidbody and animator attached to the current GameObject
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();

        // initialize contacts list
        contactList = new List<ContactPoint2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Pass in the proper direction that the player should move in
        Move(direction);

        // TODO: check conditions needed to flip player, and if so, flip player
        if((isFacingRight && direction < 0) || 
            (!isFacingRight && direction > 0))
        {
            Flip();
        }
    }

    void OnJump()
    {
        //if player is on the ground, jump
        if (isGrounded)
            Jump();
    }

    private void Jump()
    {
        // TODO: change y velocity of player
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    void OnMove(InputValue moveVal)
    {
        // TODO: store direction input and store it to global variable
        float moveDirection = moveVal.Get<float>();
        direction = moveDirection;
    }

    private void Move(float x)
    {
        // TODO: change x velocity of player
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        // TODO: Here, we can handle animation transitioning logic
        
    }

    private void Flip()
    {
        // TODO: flip local scale of player and change global variable that stores which direction player is facing
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x *= -1f;
        transform.localScale = newLocalScale;

        isFacingRight = !isFacingRight;
    }

    // TODO: Week 3's assignment needs a couple of extra functions here...
    void OnCollisionEnter2D(Collision2D other)
    {

    }

    void OnCollisionStay2D(Collision2D other)
    {
        // check if normal of collision is pointing up, if so grounded is true
        // iterate thru list of contact points bc tilemap has multiple contact points
        if(other.gameObject.CompareTag("Ground")) {
            other.GetContacts(contactList);
            foreach(ContactPoint2D contact in contactList) {
                Vector3 norm = contact.normal;
                if(Vector3.Angle(norm, Vector3.up) < 45) {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // if the player collides with the coin, destroy the coin
        if (col.gameObject.tag == "Collectible")
        {
            Destroy(col.gameObject);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground")) {
            isGrounded = false;
        }
    }
}
