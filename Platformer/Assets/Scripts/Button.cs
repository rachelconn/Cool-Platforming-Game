using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // implement as list elementlet
    public static Dictionary<COLORSYNC, Button> buttons = new Dictionary<COLORSYNC, Button>();

    private bool pressed = false;

    public Sprite spr_unpressed;
    public Sprite spr_pressed;

    public COLORSYNC myColor = COLORSYNC.ORANGE;

    private SpriteRenderer sr;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        buttons.Add(myColor, this);
    }

    void OnDestroy()
    {
        buttons.Remove(myColor);
    }

    void Update()
    {
        sr.sprite = pressed ? spr_pressed : spr_unpressed;
    }

    public bool isPressed()
    {
        return this.pressed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (!pressed)
            {
                pressed = true;
            }
        }
    }

    public static bool IsButtonOfColorPressed(COLORSYNC color)
    {
        Button outval;
        if (!Button.buttons.TryGetValue(color, out outval))
        {
            throw new System.Exception("Button of color " + color.ToString() + " does not exist");
        }
        return outval.isPressed();
    }
}
