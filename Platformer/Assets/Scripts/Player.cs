using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // implement as singlet
    public static Player thePlayer;

    public GameObject settingsUI;
    private GameObject settingsUIInstance = null;
    public AudioSource oofSound;
    public GameObject oofFlash;
    //direction the player is facing
    private Vector2 facingDirection;
    private bool deathInProgress;
    // if player uses cannon, stop lerping horizontal speed
    private bool usedCannon = false;

    public static float volumeLevel;
    public static bool readTutorial;

    public Rigidbody2D body;
    private BoxCollider2D myCollider;
    private SpriteRenderer spriteRenderer;
    private static bool isDead;
    private bool onGround;
    public bool didJump;
    private bool didDash;
    // player can dash again on landing
    private bool canDash;
    // used to only allow wavedash if player started airborne
    private bool wasGroundedWhenDashing;
    public bool getCanDash()
    {
        return canDash;
    }
    // if player has not reached neu
    private bool didWallJump;
    // distance to side bounding box
    private float distToSide;
    // distance to bottom bounding box
    private float distToGround;
    // time since pressing dash
    private float timeDashing;
    // timeSinceDash used to let player wavedash if they've recently dashed
    private float timeSinceDash;
    // direction wall jumped (0 if reached neutral since then), used to reduce influence on speed
    private float wallJumpDirection;
    // time since touched wall, used to give leniency to walljumps
    private float timeSinceWallTouch;
    // used for walljump leniency, remember direction of last wall hit (-1 or 1 => left or right)
    private float lastCollisionDirection;
    private float timeSinceWallJump;
    // horizontal + vertical axis
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

    private static string currentLevel = "";

    private void HandleMovement()
    {
        // stop using wall jump physics if trying to go same direction as wall jumping
        if (inputDirection.x != 0 && Mathf.Sign(wallJumpDirection) == Mathf.Sign(inputDirection.x))
            wallJumpDirection = 0;
        // horizontal movement
        float targetVelocityX = Time.fixedDeltaTime * moveSpeed * inputDirection.x;
        // cap vertical speed to terminal velocity

        float newVelocityY = Mathf.Max(body.velocity.y, Time.fixedDeltaTime * terminalVelocity);
        // smooth movement: lerp speed to target velocity, keep more momentum if above moveSpeed
        if (!usedCannon && System.Math.Abs(body.velocity.x) > moveSpeed * Time.fixedDeltaTime) {
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, targetVelocityX, 0.03f), newVelocityY);
        }
        // using cannon changes momentum behavior
        else if (usedCannon)
            if (inputDirection.x == 0)
                body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, targetVelocityX, 0.05f), newVelocityY);
            else
                body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, targetVelocityX, 0.15f), newVelocityY);
        else
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, targetVelocityX, 0.3f), newVelocityY);
        // lerp hspeed to wall jump direction if recently wall jumped so player can't climb infinitely
        if (wallJumpDirection != 0)
        {
            // weight velocity towards wall jump direction depending on time since last wall jump
            float tValue = 1 - timeSinceWallJump * wallJumpMomentumMultiplier;
            body.velocity = new Vector2(Mathf.Lerp(body.velocity.x, getWallJumpVelocity(), tValue), body.velocity.y);
            // if reached neutral velocity or too long has passed, restore original physics
            if (tValue < 0)
            {//} || Mathf.Sign(wallJumpDirection) != Mathf.Sign(body.velocity.x)) {
                wallJumpDirection = 0;
            }
        }

    }

    public float getJumpVelocity()
    {
        return jumpMultiplier * Time.fixedDeltaTime;
    }
    // TODO: use deltatime in wall jump
    // horizontal wall jump velocity
    private float getWallJumpVelocity()
    {
        return wallJumpDirection * moveSpeed * bounceMultiplier * Time.fixedDeltaTime;
    }
    private void HandleJump()
    {
        // jumping
        if (onGround && didJump)
        {
            didJump = false;

            // make player jump (set y velocity)
            body.velocity = new Vector2(body.velocity.x, getJumpVelocity());

            // if recently dashed, wavedash
            if (!wasGroundedWhenDashing && timeSinceDash < 0.05f) {
                Debug.Log("Wavedashed!");
                body.velocity = new Vector2(facingDirection.x * dashSpeed * 1.7f * Time.fixedDeltaTime, body.velocity.y);
            }

            // stop dashing if player jumped
            timeDashing = 0;
        }
        // don't change velocity if dashing
        if (timeDashing == 0)
        {
            // increase y velocity when falling for weightier jump
            if (body.velocity.y < 0)
                body.velocity += Vector2.up * Physics2D.gravity.y * fallingMultiplier * Time.fixedDeltaTime;
            // don't jump as high if not holding jump
            if (body.velocity.y > 0 && !InputManager.GetButton("Jump"))
            {
                body.velocity += Vector2.up * Physics2D.gravity.y * noHoldMultiplier * Time.fixedDeltaTime;
            }
        }
    }

    private bool isOnGround()
    {
        for (int i = -1; i <= 1; i++)
        {
            Vector2 pos = transform.position + (Vector3.right * distToSide * i);
            if (Physics2D.Raycast(pos, -Vector2.up, distToGround + 0.1f, LayerMask.GetMask("Wall"))) {
                // reset usedCannon and horizontal speed when landing
                if (usedCannon) {
                    usedCannon = false;
                    body.velocity *= Vector2.up;
                }
                return true;
            }
        }
        return false;
    }

    private void HandleWallJump()
    {
        timeSinceWallTouch += Time.fixedDeltaTime;
        if (onGround)
            wallJumpDirection = 0;
        // see if touching a wall on either side and determine action to take
        for (float direction = -1; direction <= 1; direction += 2)
        {
            //collide with bottom, middle, and top
            for (int i = -1; i <= 1; i++)
            {
                Vector2 pos = transform.position + (Vector3.up * distToSide * i);
                if (Physics2D.Raycast(pos, Vector2.right * direction, distToSide + 0.2f, LayerMask.GetMask("Wall")))
                {
                    // remember that player hit a wall (and the direction of it) to wall jump later
                    timeSinceWallTouch = 0;
                    lastCollisionDirection = direction;
                    // slide down if holding towards wall
                    if (!onGround && direction == inputDirection.x)
                    {
                        float maxFallSpeed = maxWallSlideSpeed * Time.fixedDeltaTime;
                        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -maxFallSpeed, float.PositiveInfinity));
                    }
                    // once collision has been found, stop checking
                    break;
                }
            }
        }
        // wall jumping
        if (!onGround && didJump && timeSinceWallTouch < 0.05f)
        {
            //vertical movement
            body.velocity = Vector2.up * getJumpVelocity();
            wallJumpDirection = -lastCollisionDirection;
            //bounce from wall
            body.velocity += Vector2.right * getWallJumpVelocity();
            //stop dash after walljumping
            timeDashing = 0;
            timeSinceWallJump = 0;
        }
        timeSinceWallJump += Time.fixedDeltaTime;
    }

    private void HandleDash()
    {
        // reset gravity in case dash stops externally
        body.gravityScale = 1;
        // update time since dash - will be reset if player dashes again
        timeSinceDash += Time.fixedDeltaTime;
        // update if player is able to dash
        if (onGround)
            canDash = true;
        // dash direction player is holding, or forward if they araen't holding a direction
        if (canDash && didDash)
        {
            // only let player wavedash if not grounded at beginning of dash
            wasGroundedWhenDashing = onGround;
            if (inputDirection.magnitude == 0 && !onGround)
                dashDirection = Vector2.right * facingDirection;
            else
                dashDirection = inputDirection.normalized;
            canDash = false;
            timeDashing += Time.fixedDeltaTime;
        }
        // continue dashing
        if (timeDashing != 0 && timeDashing < dashTime)
        {
            body.gravityScale = 0;
            body.velocity = dashSpeed * dashDirection * Time.fixedDeltaTime;
            timeDashing += Time.fixedDeltaTime;
            timeSinceDash = 0;
        }
        // finish if dash is over
        if (timeDashing > dashTime)
        {
            timeDashing = 0;
            // remove some vertical velocity, reset horizontal velocity
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * Mathf.Min(Mathf.Abs(body.velocity.x), moveSpeed * Time.fixedDeltaTime),
                                        Mathf.Min(body.velocity.y, dashSpeed / 2 * Time.fixedDeltaTime));
        }
        didDash = false;
    }

    Sprite GetSprite()
    {
        // determine sprite to render
        return (canDash) ? player : playerNoDash;
    }

    void Start()
    {
        facingDirection = Vector2.right;
        oofSound = GetComponent<AudioSource>();
        isDead = false;
        body = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        distToSide = myCollider.bounds.extents.x;
        distToGround = myCollider.bounds.extents.y;
        timeSinceDash = float.PositiveInfinity;
        timeSinceWallJump = float.PositiveInfinity;
        timeSinceWallTouch = float.PositiveInfinity;
        onGround = false;
        canDash = true;
        deathInProgress = false;

        currentLevel = SceneManager.GetActiveScene().name;
        Player.thePlayer = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (gameObject.transform.position.y < -10.0f)
            {
                ReloadLevel();
            }
            if (InputManager.GetButtonDown("Jump"))
            {
                didJump = true;
            }
            if (InputManager.GetButtonDown("Dash"))
            {
                didDash = true;
            }
            inputDirection = new Vector2(InputManager.GetAxisRaw("Horizontal"), InputManager.GetAxisRaw("Vertical"));

        }

        if (InputManager.GetButtonDown("Settings"))
        {
            if (settingsUIInstance == null)
            {
                settingsUIInstance = Instantiate(settingsUI);
                _Pause();
                //SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
            }
            else
            {
                // user may be rebinding keys so don't do anything
            }
        }
    }

    private void FixedUpdate()
    {
        // save if player is onground so it doesn't have to be checked multiple times
        onGround = isOnGround();
        // remember direction player is facing
        if (inputDirection.x != 0)
        facingDirection = Vector2.right * inputDirection.x;
        HandleMovement();
        HandleJump();
        HandleWallJump();
        didJump = false;
        HandleDash();
        spriteRenderer.sprite = GetSprite();
    }

    public void _Pause()
    {
        Time.timeScale = 0f;
    }

    public static bool isPaused()
    {
        if (Time.timeScale == 0f)
        {
            return true;
        }
        return false;
    }

    public void _Unpause()
    {
        Time.timeScale = 1f;
    }

    public static void Pause()
    {
        Player.thePlayer._Pause();
    }

    public static void Unpause()
    {
        if (Player.thePlayer != null)
            Player.thePlayer._Unpause();
    }

    /// <summary>
    /// notify the player through various feedback that they died
    /// </summary>
    public IEnumerator kill()
    {
        isDead = true;
        oofSound.Play();
        StartCoroutine(KillScreenFlash());
        yield return new WaitForSeconds(0.7f);
        // if player has died 10 times, give them the option to skip level
        if (++LevelSkip.numDeaths >= 10 && InputManager.Pref_ShowSkipDialogue)
        {
            Time.timeScale = 0;
            LevelSkip.numDeaths = 0;
            GameObject levelSkipScreen = (GameObject)Instantiate(Resources.Load("LevelSkipUI"));
        }
        else
        {
            SceneManager.LoadScene(currentLevel);
        }
    }

    public IEnumerator KillScreenFlash()
    {
        GameObject go = Instantiate(oofFlash);
        Image im = go.GetComponentInChildren<Image>();
        for (float i = 0f; i < 1f; i += 0.06f)
        {
            Color temp = im.color;
            temp.a = i;
            im.color = temp;
            yield return new WaitForSeconds(0.5f / 15.0f);
        }
        Color temp2 = im.color;
        temp2.a = 1;
        im.color = temp2;
        yield return new WaitForEndOfFrame();
    }

    public void _Kill()
    {
        if (!deathInProgress)
        {
            deathInProgress = true;
            StartCoroutine(kill());
        }
    }

    /// <summary>
    /// public handle for level reloading due to die
    /// </summary>
    public static void ReloadLevel()
    {
        Player.thePlayer._Kill();
    }

    /// <summary>
    /// object handle for dash recharge
    /// </summary>
    public void _rechargeDash()
    {
        this.canDash = true;
    }

    /// <summary>
    /// public handle for dash recharge
    /// </summary>
    public static void RechargeDash()
    {
        Player.thePlayer._rechargeDash();
    }

    public static void SetUsedCannon()
    {
        Player.thePlayer.usedCannon = true;
    }

    public void SpringJump(int direction)
    {
        switch (direction)
        {
            case 0:
                body.velocity = new Vector2(-2 * getJumpVelocity(), body.velocity.y);
                break;
            case 1:
                body.velocity = new Vector2(2 * getJumpVelocity(), body.velocity.y);
                break;
            case 2:
                body.velocity = new Vector2(body.velocity.x, 2 * getJumpVelocity());
                break;
        }

        // stop dashing if player jumped
        timeDashing = 0;
        usedCannon = false;
        RechargeDash();
    }
}
