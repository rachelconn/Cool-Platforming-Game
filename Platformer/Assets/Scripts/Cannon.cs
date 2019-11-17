using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    public double time;
    public Sprite upCannonSprite;
    public Sprite diagCannonSprite;
    public Sprite rightCannonSprite;
    public bool inCannon = false;
    
    private double cannonCoolDown = 0;
    private SpriteRenderer sr;

    private bool launch = false;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = upCannonSprite;

        time = 0;
    }

    void Update() {
        switch (System.Math.Floor(time)) {
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

        time += Time.deltaTime;
        if (cannonCoolDown >= 0)
            cannonCoolDown -= Time.deltaTime;
        if (System.Math.Floor(time) > 7) {
            time = 0;
        }
        
        Rigidbody2D theBody = Player.thePlayer.body;


        
        if (inCannon) {
            
            theBody.position = transform.position;
            theBody.velocity = Vector2.zero;

            if (Input.GetButtonDown("Jump")) {
                Player.thePlayer.didJump = false;
                Player.RechargeDash();
                
                cannonCoolDown = 0.2;
                switch (System.Math.Floor(time)) {
                    case 0:
                        theBody.velocity += (new Vector2(0, 1)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 1:
                        theBody.velocity += (new Vector2(1, 1)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 2:
                        theBody.velocity += (new Vector2(1, 0)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 3:
                        theBody.velocity += (new Vector2(1, -1)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 4:
                        theBody.velocity += (new Vector2(0, -1)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 5:
                        theBody.velocity += (new Vector2(-1, -1)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 6:
                        theBody.velocity += (new Vector2(-1, 0)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                    case 7:
                        theBody.velocity += (new Vector2(-1, 1)) * 3 * Player.thePlayer.getJumpVelocity();
                        break;
                }

                theBody.gravityScale = 1;
                inCannon = false;
                
            }
        }
    
    }

    void OnTriggerEnter2D(Collider2D other) {
        Rigidbody2D theBody = Player.thePlayer.body;

        if (other.gameObject.name == "Player" && cannonCoolDown <= 0) {
            theBody.position = transform.position;
            theBody.velocity = Vector2.zero;
            theBody.gravityScale = 0;

            inCannon = true;
            
        }
    }

}