using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public double time;
    public Sprite upCannonSprite;
    public Sprite diagCannonSprite;
    public Sprite rightCannonSprite;
    public bool inCannon = false;

    private double cannonCoolDown = 0;
    private SpriteRenderer sr;


    //constant to change speed that the barrel spins (1 = 8 second full spin; 8 = 1 second full spin)
    public int barrelSpeed;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = upCannonSprite;

        time = 0;
    }

    void Update()
    {
        switch (System.Math.Floor(time))
        {
            case 0:
                sr.sprite = upCannonSprite;
                sr.flipY = false;
                break;
            case 1:
                sr.sprite = diagCannonSprite;
                sr.flipY = false;
                sr.flipX = false;
                break;
            case 2:
                sr.sprite = rightCannonSprite;
                sr.flipX = false;
                break;
            case 3:
                sr.sprite = diagCannonSprite;
                sr.flipY = true;
                sr.flipX = false;
                break;
            case 4:
                sr.sprite = upCannonSprite;
                sr.flipY = true;
                break;
            case 5:
                sr.sprite = diagCannonSprite;
                sr.flipY = true;
                sr.flipX = true;
                break;
            case 6:
                sr.sprite = rightCannonSprite;
                sr.flipX = true;
                break;
            case 7:
                sr.sprite = diagCannonSprite;
                sr.flipY = false;
                sr.flipX = true;
                break;
        }

        time += BarrelSpeed.speed * Time.deltaTime;
        if (cannonCoolDown >= 0)
            cannonCoolDown -= Time.deltaTime;
        if (System.Math.Floor(time) > 7)
        {
            time = 0;
        }

        Rigidbody2D theBody = Player.thePlayer.body;



        if (inCannon)
        {

            theBody.position = transform.position;
            theBody.velocity = Vector2.zero;

            if (InputManager.GetButtonDown("Jump"))
            {
                Player.thePlayer.didJump = false;
                Player.RechargeDash();
                float t = (float) System.Math.Floor(time);
                if (t != 0)
                    Player.SetUsedCannon();
                // calculate angle from time and set velocity
                float angle = Mathf.Deg2Rad * (t * -(360 / 8) + 90);
                // launch with less force if launching vertically
                float multiplier = (Mathf.Abs((angle - (Mathf.PI / 2)) % (Mathf.PI * 2)) < 0.01f) ? 2.0f : 2.5f;
                theBody.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                // don't actually normalize movement because that's how it worked before I changed this
                theBody.velocity *= multiplier * Player.thePlayer.getJumpVelocity();
                theBody.gravityScale = 1;
                inCannon = false;

            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D theBody = Player.thePlayer.body;

        if (other.gameObject.name == "Player") {
            theBody.position = transform.position;
            theBody.velocity = Vector2.zero;
            theBody.gravityScale = 0;

            inCannon = true;

        }
    }

}
