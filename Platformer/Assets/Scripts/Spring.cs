using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Sprite spr_ready;
    public Sprite spr_notReady;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.sprite = ready ? spr_ready : spr_notReady;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player")
        {
            Player.SpringJump();
        }
    }
}
