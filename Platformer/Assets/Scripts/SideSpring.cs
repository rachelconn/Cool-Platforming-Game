using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSpring : MonoBehaviour {
    public Sprite springSprite;
    private SpriteRenderer sr;

    //false == left, true == right
    public int direction;
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();

        if (direction == 1) {
            sr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.name == "Player")
            if (direction == 1) {
                if (other.GetContact(0).normal[0] < 0) {
                    Player.thePlayer.SpringJump(direction);
                }
            }

            else {
                if (other.GetContact(0).normal[0] > 0) {
                    Player.thePlayer.SpringJump(direction);
                }
            }
    }
}