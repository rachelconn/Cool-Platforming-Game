using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody2D body;
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private bool onGround;
    private bool didJump;
    private bool didDash;
    // player can dash again on landing
    private bool canDash;
    // if player has not reached neu
    private bool didWallJump;
    // distance to side bounding box
    private float distToSide;
    // distance to bottom bounding box
    private float distToGround;
    // time since pressing dash
    private float timeDashing;
    //direction wall jumped (0 if reached neutral since then), used to reduce influence on speed
    private float wallJumpDirection;
    // horizontal + vertical axis
    private float timeSinceWallJump;
    private Vector2 inputDirection;
    // direction pressed when the player dashed
    private Vector2 dashDirection;
    // variables for changing physics
    public float terminalVelocity;
    public float moveSpeed;
    public float jumpMultiplier;
    // if not holding jump but moving up, add this velocity per fixedupdate
    public float noHoldMultiplier;
    // if negative velocity, add this (weightier jumps)
    public float fallingMultiplier;
    // force to bounce off walls with
    public float bounceMultiplier;
    public float dashSpeed;
    public float dashTime;
    // speed to wall slide at
    public float maxWallSlideSpeed;
    // momentum to keep from wall jump
    public Sprite player;
    public Sprite playerNoDash;
    public float wallJumpMomentumMultiplier;

    private void HandleMovement() {
        // stop using wall jump physics if trying to go same direction as wall jumping
        if (inputDirection.x != 0 && Mathf.Sign(wallJumpDirection) == Mathf.Sign(inputDirection.x))
            wallJumpDirection = 0;
        // horizontal movement
        float targetVelocityX = Time.fixedDeltaTime * moveSpeed * inputDirection.x;
        // cap vertical speed to terminal velocity
        float newVelocityY = Mathf.Max(body.velocity.y, Time.fixedDeltaTime * terminalVelocity);
        // smooth movement
        body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, targetVelocityX, 0.3f), newVelocityY);
        // lerp hspeed to wall jump direction if recently wall jumped so player can't climb infinitely
        if (wallJumpDirection != 0) {
            // weight velocity towards wall jump direction depending on time since last wall jump
            float tValue = 1 - timeSinceWallJump * wallJumpMomentumMultiplier;
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, getWallJumpVelocity(), tValue), body.velocity.y);
            // if reached neutral velocity or too long has passed, restore original physics
            if (tValue < 0) {//} || Mathf.Sign(wallJumpDirection) != Mathf.Sign(body.velocity.x)) {
                wallJumpDirection = 0;
            }
        }
    }

    private float getJumpVelocity() {
        return jumpMultiplier * Time.fixedDeltaTime;
    }
    // TODO: use deltatime in wall jump
    // horizontal wall jump velocity
    private float getWallJumpVelocity() {
        return wallJumpDirection * moveSpeed * bounceMultiplier * Time.fixedDeltaTime;
    }
    private void HandleJump() {
        // jumping
        if (onGround && didJump) {
            body.velocity += Vector2.up * getJumpVelocity();
            didJump = false;
            // stop dashing if player jumped
            timeDashing = 0;
        }
        // don't change velocity if dashing
        if (timeDashing == 0) {
            // increase y velocity when falling for weightier jump
            if (body.velocity.y < 0)
                body.velocity += Vector2.up * Physics2D.gravity.y * fallingMultiplier * Time.fixedDeltaTime;
            // don't jump as high if not holding jump
            if (body.velocity.y > 0 && !Input.GetButton("Jump")) {
                body.velocity += Vector2.up * Physics2D.gravity.y * noHoldMultiplier * Time.fixedDeltaTime;
            }
        }
    }

    private bool isOnGround() {
        for (int i = -1; i <= 1; i++) {
            Vector2 pos = transform.position + (Vector3.right * distToSide * i);
            if (Physics2D.Raycast(pos, -Vector2.up, distToGround + 0.1f, LayerMask.GetMask("Wall")))
                return true;
        }
        return false;
    }

    private void HandleWallJump() {
        if (onGround)
            wallJumpDirection = 0;
        // see if touching a wall on either side and determine action to take
        for (float direction = -1; direction <= 1; direction += 2) {
            //collide with bottom, middle, and top
            for (int i = -1; i <= 1; i++) {
                Vector2 pos = transform.position + (Vector3.up * distToSide * i);
                if (Physics2D.Raycast(pos, Vector2.right * direction, distToSide + 0.2f, LayerMask.GetMask("Wall"))) {
                    // wall jumping
                    if (!onGround && didJump) {
                        //vertical movement
                        body.velocity = Vector2.up * getJumpVelocity();
                        wallJumpDirection = -direction;
                        //bounce from wall
                        body.velocity += Vector2.right * getWallJumpVelocity();
                        //stop dash after walljumping
                        timeDashing = 0;
                        timeSinceWallJump = 0;
                    }
                    // else slide down if holding towards wall
                    else if (!onGround && direction == inputDirection.x) {
                        float maxFallSpeed = maxWallSlideSpeed * Time.fixedDeltaTime;
                        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxFallSpeed, float.PositiveInfinity));
                    }
                    // once collision has been found, stop checking
                    break;
                }
            }
        }
        timeSinceWallJump += Time.fixedDeltaTime;
    }

    private void HandleDash() {
        // update if player is able to dash
        if (onGround)
            canDash = true;
        // don't use dash if not holding a direction
        if (inputDirection.magnitude != 0 && canDash && didDash) {
            canDash = false;
            dashDirection = inputDirection.normalized;
            timeDashing += Time.fixedDeltaTime;
        }
        // continue dashing
        if (timeDashing != 0 && timeDashing < dashTime) {
            body.velocity = dashSpeed * dashDirection * Time.fixedDeltaTime;
            timeDashing += Time.fixedDeltaTime;
        }
        // finish if dash is over
        if (timeDashing > dashTime) {
            timeDashing = 0;
            // remove some vertical velocity
            body.velocity = new Vector2(body.velocity.x, Mathf.Min(body.velocity.y, dashSpeed / 2 * Time.fixedDeltaTime));
        }
        didDash = false;
    }

    Sprite GetSprite() {
        // determine sprite to render
        return (canDash) ? player : playerNoDash;
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        distToSide = collider.bounds.extents.x;
        distToGround = collider.bounds.extents.y;
        timeSinceWallJump = float.PositiveInfinity;
        onGround = false;
        canDash = true;
        inputManager = GameObject.FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.GetButtonDown("Jump")) {
            didJump = true;
        }
       if (inputManager.GetButtonDown("Dash")) {
            didDash = true;
        }
       if (inputManager.GetButtonDown("Left"))
       {
           inputDirection = new Vector2(-1, Input.GetAxisRaw("Vertical"));
       }
       else if (inputManager.GetButtonUp("Left") || inputManager.GetButtonUp("Right"))
       {
           inputDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
       }
       if (inputManager.GetButtonDown("Right"))
       {
           inputDirection = new Vector2(1, Input.GetAxisRaw("Vertical"));
       }
       else if (inputManager.GetButtonUp("Right"))
       {
           inputDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
       }
    }

    private void FixedUpdate() {
        onGround = isOnGround();
        HandleMovement();
        HandleJump();
        HandleWallJump();
        didJump = false;
        HandleDash();
        spriteRenderer.sprite = GetSprite();
    }
}
