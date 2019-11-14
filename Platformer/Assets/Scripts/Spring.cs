using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Sprite springSprite;
    private SpriteRenderer sr;

    //0 == up, 1 == right, 2 == down, 3 == left
    //public int direction;
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.name == "Player" && other.GetContact(0).normal[1] < 0)
        {
            Player.thePlayer.SpringJump(2);
        }
    }
}

