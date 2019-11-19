using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablableWall : MonoBehaviour
{
    public COLORSYNC myColor;
    protected int HEURISTIC_colorNotRegisteredFrames = 0;
    protected readonly int HEURISTIC_colorNotRegisteredErrorCutoff = 100;

    public bool activated = true;
    protected Button registeredButton = null;

    public Sprite spr_activated;
    public Sprite spr_deactivated = null;

    // Start is called before the first frame update
    void Start()
    {
        HEURISTIC_colorNotRegisteredFrames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (registeredButton == null)
            {
                if (Button.buttons.ContainsKey(myColor))
                {
                    bool result = Button.buttons.TryGetValue(myColor, out registeredButton);
                    if (!result)
                    {
                        Debug.LogError("Button not in the dict somehow...");
                    }
                }
                else
                {
                    HEURISTIC_colorNotRegisteredFrames++;
                    if (HEURISTIC_colorNotRegisteredFrames > HEURISTIC_colorNotRegisteredErrorCutoff)
                    {
                        Debug.LogError(string.Format("Button of color {0} has not been registered for more than {1} frames.", HEURISTIC_colorNotRegisteredFrames, HEURISTIC_colorNotRegisteredErrorCutoff));
                    }
                }
            }
            
            if (registeredButton != null && registeredButton.isPressed())
            {
                Deactivate();
            }
        }
    }

    public void Deactivate()
    {
        activated = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (spr_deactivated == null)
        {
            sr.sprite = null;
        }
        else
        {
            sr.sprite = spr_deactivated;
        }
    }
}
