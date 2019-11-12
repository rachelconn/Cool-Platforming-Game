using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRecovery : MonoBehaviour
{
    public double RechargeTime = 5.0;
    public double cooldown = 0;

    private bool ready = true;

    public Sprite spr_ready;
    public Sprite spr_notReady;

    private SpriteRenderer sr;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!ready)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                ready = true;
                cooldown = 0;
            }
        }
        sr.sprite = ready ? spr_ready : spr_notReady;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (ready)
            {
                ready = false;
                cooldown = RechargeTime;
                Player.RechargeDash();
            }
        }
    }
}
