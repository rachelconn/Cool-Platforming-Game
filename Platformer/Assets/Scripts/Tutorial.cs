using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private TextMeshPro text;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        GetComponent<Renderer>().sortingLayerName = "UI";
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + Vector3.up * 2;
        if (player.position.x < 10)
            text.SetText("Press left and right to move.");
        else if (player.position.x < 22)
            text.SetText("Press Z to jump.");
        else if (player.position.x < 30)
            text.SetText("Press X and any direction to dash in that direction. Dash in the air for a double jump!");
        else
            text.SetText("Press jump while touching a wall to wall jump.");
    }
}
