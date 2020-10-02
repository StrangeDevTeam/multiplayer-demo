using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    /************************************ Variable Setup  ************************************/

    private PhotonView PV;          // the photon view
    private Rigidbody2D RB2d;       // the rigidbody 2d
    private CapsuleCollider2D CC2d; // the capsule collider

    public AvatarAnimationController anim;

    public float movementSpeed;              // the speed of the user when moving left or right
    public float jumpForce;                  // the force applied when the user jumps
    public float defaultGravity;             // the rate at which the player falls
    public float phaseThroughTimer = 0.0f;   // the time the player has been able to phase through objects
    public float phasetime = 0.5f;           // the time the player will phase through objects when performing a down jump
    public float isFallingLeniancy = 0.2f;   //Value the player can fall wiothout changing to the 'falling' state.
    public int collisions = 0;               // the number of objects the player is colliding with
    public int nearbyCollisions = 0;         //A counter for the platforms that are nearby.
    public bool phaseThrough = false;        // true when the user is dropping through a platform
    public bool onDroppablePlatform = false; // true when the user is stood on a platform they can drop through
    public bool isOnGround;                  // true when the user is touching some form of ground or floor
    public bool isFallingOrStatic;           // false when the user is rising in a upward velocity
    const float climbSpeed = 1.5f;           //The speed at which the player can move on climbable objects.
    int noOfRopesCollidingWith = 0;          //A counter, keeping track of how many climbable objects the player is currently touching.
    public int noOfJumpsUsed = 0;            //A counter, keeping track of the number of jumps a player has performed without touching the ground or a climbable object.
    const int maxNoOfJumps = 2;              //The number of jumps the player can make. 1 is no double jump.
    bool canJumpInMidair = false;            //Prevents holding the jump key to move upwards further.
    float ropeReleaseCooldown = 1.0f;        //A period of time which th eplayer cannot jump off of a rope they have started climbing. Used to prevent spam jumping.
    float timeConnectedToRope = 0.0f;
    bool canJumpOffRope = false;

    //Strings used in the code.
    const string Ground = "Ground";         //Tag applied to a platform that cannot be dropped through.
    const string GroundDownJump = "GroundDownJump"; //Tag applied to a platform that can be dropped through.
    const string climbable = "climbable";      //Tag applied to an object that can be climbed.

    public enum playerState { idle, moving, crouchIdle, crouchMoving, jumping, climbing, falling }; //An enum holding the current state of the player.
    public playerState currentPlayerState = playerState.idle; //The default state is set to idle, though this will change as soon as the player is loaded in.


    void Start()
    {
        // get the components for later use
        PV = GetComponent<PhotonView>();
        RB2d = GetComponent<Rigidbody2D>();
        CC2d = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<AvatarAnimationController>();
    }

    void Update()
    {
        if (PV.IsMine) //If the avatar is the players to control, and not another player on the screen.
        {
            Gravity();
            Movement();
            AnimationHandler();
            // set isFallingOrStatic
            if (RB2d.velocity.y < isFallingLeniancy) 
                isFallingOrStatic = true;
            else isFallingOrStatic = false;


           


            if (nearbyCollisions < 1) //Player must be in the air.
            {
                onDroppablePlatform = false;
                isOnGround = false;
            }
        }
        else
            RB2d.gravityScale = 0;

        
    }


    // moves the player based on inputs
    void Movement()
    {
        if (phaseThrough) //Dropping through platform.
        {
            phaseThroughTimer += Time.deltaTime;
            if (phaseThroughTimer > phasetime)
            {
                phaseThroughTimer = 0;
                phaseThrough = false;
                CC2d.enabled = true;
            }
        }

        /************************************ Player Input  ************************************/

        // only move player if they arent in the middle of an attack animation
        if (!anim.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAvatar_Attack")&&
            !anim.anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAvatar_OneHand_Attack"))
        {
            if (Input.GetKey(KeyBinds.crouchKey)) //Crouching.
            {
                if (currentPlayerState == playerState.climbing)
                    this.transform.Translate(0.0f, -climbSpeed * Time.deltaTime, 0.0f);
                else
                {
                    currentPlayerState = playerState.crouchIdle; //Player state is set to 'idle crouching'. If the player is moving, this is updated to 'crouch moving' when the code checks for movement button input.

                    if (Input.GetKey(KeyBinds.jumpKey)) //Attempts to drop through current platform/ ground.
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
            }
            else if (Input.GetKey(KeyBinds.jumpKey)) //Jumping
            {
                if (isOnGround || (currentPlayerState == playerState.climbing && canJumpOffRope)) //Conditions allowing player to jump.
                    jump(1.0f);
                else if (noOfJumpsUsed < maxNoOfJumps && canJumpInMidair)
                    jump(1.5f);

            }
            else if (Input.GetKeyUp(KeyBinds.jumpKey))
                canJumpInMidair = true;

            // right movement
            if (Input.GetKey(KeyBinds.movementRightKey) && // right key pressed
                currentPlayerState != playerState.climbing && // not climbing
                currentPlayerState != playerState.jumping) // not jumping
            {
                RB2d.AddForce(new Vector2(movementSpeed * Time.deltaTime, 0.0f));
                anim.IsWalkingNormal = true;
                anim.mirrorAnim = true; //The animation for the player avatar faces to the left by default. This flips it.
            }
            // left movement
            else if (Input.GetKey(KeyBinds.movementLeftKey) && // left key pressed
                currentPlayerState != playerState.climbing && // not climbing
                currentPlayerState != playerState.jumping) // not jumping
            {
                RB2d.AddForce(new Vector2(-movementSpeed * Time.deltaTime, 0.0f));
                anim.IsWalkingNormal = true;
                anim.mirrorAnim = false;
            }
            else if (Input.GetKey(KeyBinds.upKey)) //This is only used when climbing an object to move upwards.
            {
                if (currentPlayerState == playerState.climbing)
                {
                    this.transform.Translate(0.0f, climbSpeed * Time.deltaTime, 0.0f);
                }
            }
            else
            {
                anim.IsWalkingNormal = false;
            }


            if ((Input.GetKey(KeyBinds.movementLeftKey) || Input.GetKey(KeyBinds.movementRightKey)) &&
                (RB2d.velocity.y >= 0) && currentPlayerState != playerState.climbing)                   //Changes the players state to either 'moving' or 'crouch moving' depending on whether the player is crouching or not.
                currentPlayerState = (currentPlayerState == playerState.crouchIdle ? currentPlayerState = playerState.crouchMoving : currentPlayerState = playerState.moving);

            if (!Input.GetKey(KeyBinds.movementLeftKey) && !Input.GetKey(KeyBinds.movementRightKey) && !Input.GetKey(KeyBinds.jumpKey) &&
                !Input.GetKey(KeyBinds.crouchKey) && (RB2d.velocity.y >= 0) && currentPlayerState != playerState.climbing) //No controls are being used, and player isn't falling.
            {
                currentPlayerState = playerState.idle;
            }
        }

            if (currentPlayerState == playerState.climbing)
                CC2d.enabled = false;
            else if (!phaseThrough) CC2d.enabled = true;
    }
    // applies gravity to the player when in mid-air
    void Gravity()
    {
        if (isOnGround) RB2d.gravityScale = 0;
        else RB2d.gravityScale = defaultGravity;

        if (currentPlayerState == playerState.climbing) RB2d.gravityScale = 0.0f; //If the player is climbing an object, disable gravity.
    }
    // adds the jump force to the player
    void jump(float jumpMultiplier)
    {
        RB2d.AddForce(new Vector2(0, jumpForce * jumpMultiplier), ForceMode2D.Impulse);
        isOnGround = false;
        currentPlayerState = playerState.jumping;
        noOfJumpsUsed++;
        canJumpInMidair = false;
    }
    // descerns and passes values into the AvatarAnimationController
    void AnimationHandler()
    {
        if (RB2d.velocity.y < 0 && currentPlayerState != playerState.climbing) //Velocity is negative and not on a climbable object, the player must be falling.
            currentPlayerState = playerState.falling;

        if (currentPlayerState == playerState.climbing) // if the player is in the climbing state
        {
            // update the animation controller to display the correct animations
            anim.isClimbing = true;

            // if the user is holding the upKey or Downkey, play animation
            if (Input.GetKey(KeyBinds.upKey) || Input.GetKey(KeyBinds.crouchKey))
            {
                anim.isClimbingSpeed = 1;
            }
            // otherwise freeze animation
            else
            {
                anim.isClimbingSpeed = 0;
            }
        }
        else
        {
            anim.isClimbing = false;
        }
        if (!onDroppablePlatform &&
           !isOnGround &&
           !(currentPlayerState == playerState.climbing))
        {
            anim.showJumpAnimation = true;
        }
        else
        {
            anim.showJumpAnimation = false;
        }
    }


    /************************************ Collision Detection  ************************************/

    private void OnCollisionEnter2D(Collision2D collision) //Enter Regular Collisions.
    {
        if (collision.gameObject.tag == Ground || collision.gameObject.tag == GroundDownJump)
        {
            collisions++;
            noOfJumpsUsed = 0;

            if (isFallingOrStatic)
            {
                if (collision.gameObject.tag == GroundDownJump)
                    onDroppablePlatform = true;
                isOnGround = true;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision) //Exiting Regular Collisions.
    {
        if (collision.gameObject.tag == Ground || collision.gameObject.tag == GroundDownJump)
        {
            collisions--;
            if ((collisions < 1) && (nearbyCollisions < 0))
            {
                if (collision.gameObject.tag == GroundDownJump)
                    onDroppablePlatform = false;
                isOnGround = false;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision) //Enter Trigger Collisions.
    {
        if (collision.gameObject.tag == Ground || collision.gameObject.tag == GroundDownJump)
        {
            nearbyCollisions++;
        }

        if (collision.gameObject.tag == climbable) //Used to increment a counter, keeping track of how many ropes the player is currently touching.
            noOfRopesCollidingWith++;
    }
    private void OnTriggerStay2D(Collider2D collision) //Checks for collision every frame.
    {
        if (collision.gameObject.tag == climbable)
        {
            if (Input.GetKey(KeyBinds.upKey) || Input.GetKey(KeyBinds.crouchKey)) //Setting the player to 'climbing', although the movement up and down the climbable object are done in the player input section.
            {
                currentPlayerState = playerState.climbing;
                noOfJumpsUsed = 0;
                this.RB2d.velocity = new Vector2(0.0f, 0.0f);
                this.transform.position = new Vector2(collision.gameObject.transform.position.x, this.transform.position.y); //Clips player to the centre of the object they are trying to climb.
            }
            timeConnectedToRope += Time.deltaTime;

            if (timeConnectedToRope >= ropeReleaseCooldown) canJumpOffRope = true;
            else
            {
                canJumpOffRope = false;
                timeConnectedToRope = 0.0f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //Exiting Trigger Collisions.
    {
        if (collision.gameObject.tag == Ground || collision.gameObject.tag == GroundDownJump)
            nearbyCollisions--;

        if (collision.gameObject.tag == climbable)
        {
            noOfRopesCollidingWith--;
            if (noOfRopesCollidingWith <= 0)
                currentPlayerState = playerState.falling;
        }
    }

}
