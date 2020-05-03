using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Packages.Rider.Editor.UnitTesting;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV; // the photon view
    private Rigidbody2D RB2d; // the rigidbody 2d
    private CapsuleCollider2D CC2d; // the capsule collider

    public AvatarAnimationController anim;

    public float movementSpeed; // the speed of the user when moving left or right
    public float jumpForce; // the force applied when the user jumps
    public float defaultGravity; // the rate at which the player falls
    public float phaseThroughTimer = 0; // the time the player has been able to phase through objects
    public float phasetime = 0.5f; // the time the player will phase through objects when performing a down jump
    public float isFallingLeniancy = 0.2f;
    public int collisions = 0; // the number of objects the player is colliding with
    public int nearbyCollisions = 0;
    public bool phaseThrough = false; // true when the user is dropping through a platform
    public bool onDroppablePlatform = false; // true when the user is stood on a platform they can drop through
    public bool isOnGround; // true when the user is touching some form of ground or floor
    public bool isFallingOrStatic; // false when the user is rising in a upward velocity

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
        CC2d = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<AvatarAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) // if the avatar is the players to control
        {
            Gravity();
            Movement();
            if (RB2d.velocity.y < isFallingLeniancy)
            {
                isFallingOrStatic = true;
            }
            else
                isFallingOrStatic = false;

        }
        else
        {
            RB2d.gravityScale = 0;
        }
        if(nearbyCollisions < 1)
        {
            onDroppablePlatform = false;
            isOnGround = false;
        }
    }
    void Movement()
    {
        if (phaseThrough)
        {
            phaseThroughTimer += Time.deltaTime;
            if(phaseThroughTimer > phasetime)
            {
                phaseThroughTimer = 0;
                phaseThrough = false;
                CC2d.enabled = true;
            }
        }
        if (Input.GetKey(crouchKey))
        {
            if (Input.GetKey(jumpKey))
            {
                if (onDroppablePlatform)
                {
                    CC2d.enabled = false;
                    phaseThrough = true;
                    onDroppablePlatform = false;
                    isOnGround = false;
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
            anim.IsWalkingNormal = true;
            anim.mirrorAnim = true;
        }
        else if (Input.GetKey(leftKey))
        {
            RB2d.AddForce(new Vector2(-movementSpeed * Time.deltaTime, 0));
            anim.IsWalkingNormal = true;
            anim.mirrorAnim = false;
        }
        else
        {
            anim.IsWalkingNormal = false;
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
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "GroundDownJump")
        {
            collisions++;
            if (isFallingOrStatic)
            {
                if (collision.gameObject.tag == "GroundDownJump")
                    onDroppablePlatform = true;
                isOnGround = true;
            }
            
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "GroundDownJump")
        {
            collisions--;
            if ((collisions < 1) && (nearbyCollisions < 0))
            {
                if (collision.gameObject.tag == "GroundDownJump")
                    onDroppablePlatform = false;
                isOnGround = false;
                
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "GroundDownJump")
        {
            nearbyCollisions++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "GroundDownJump")
        {
            nearbyCollisions--;
        }
    }
}
