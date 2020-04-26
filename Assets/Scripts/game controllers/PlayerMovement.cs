using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV; // the photon view
    private Rigidbody2D RB2d; // the rigidbody 2d
    private BoxCollider2D BC2d; // the capsule collider

    public float movementSpeed; // the speed of the user when moving left or right
    public float jumpForce; // the force applied when the user jumps
    public float defaultGravity; // the rate at which the player falls
    public float phaseThroughTimer = 0; // the time the player has been able to phase through objects

    public int collisions = 0; // the number of objects the player is colliding with

    public bool phaseThrough = false; // true when the user is dropping through a platform
    public bool onDroppablePlatform = false; // true when the user is stood on a platform they can drop through
    public bool isOnGround; // true when the user is touching some form of ground or floor
    public bool isFalling; // true when the user is falling in a downward velocity

    //key binds
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;
    KeyCode jumpKey = KeyCode.Space;
    KeyCode crouchKey = KeyCode.S;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        RB2d = GetComponent<Rigidbody2D>();
        BC2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) // if the avatar is the players to control
        {
            Gravity();
            Movement();
            if (RB2d.velocity.y < 0)
            {
                isFalling = true;
            }
            else
                isFalling = false;
        }
    }

    void Movement()
    {
        if (phaseThrough)
        {
            phaseThroughTimer += Time.deltaTime;
            if(phaseThroughTimer > 0.3f)
            {
                phaseThroughTimer = 0;
                phaseThrough = false;
                BC2d.enabled = true;
            }
        }
        if (Input.GetKey(crouchKey))
        {
            if (Input.GetKey(jumpKey))
            {
                if (onDroppablePlatform)
                {
                    BC2d.enabled = false;
                    phaseThrough = true;
                }
            }
        }
        else if (Input.GetKey(jumpKey))
        {
            if (isOnGround)
            {
                RB2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isOnGround = false;
            }
        }
        
        if (Input.GetKey(rightKey))
        {
            RB2d.AddForce(new Vector2(movementSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(leftKey))
        {
            RB2d.AddForce(new Vector2(-movementSpeed * Time.deltaTime, 0));
        }
    }
    void Gravity()
    {
        if(isOnGround)
        {
            RB2d.gravityScale = 0;
        }
        else
        {
            RB2d.gravityScale = defaultGravity;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisions++;
        if((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "GroundDownJump") && isFalling)
        {
            if (collision.gameObject.tag == "GroundDownJump")
                onDroppablePlatform = true;
            isOnGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collisions--;
        if (collisions < 1)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "GroundDownJump")
            {
                if (collision.gameObject.tag == "GroundDownJump")
                    onDroppablePlatform = false;
                isOnGround = false;
            }
        }
        
    }
}
