using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TextMeshPro))]
public class Tutorial : MonoBehaviour
{
    private TextMeshPro text;
    private AudioSource audioSource;
    private string lastPlayed = "";
    private Dictionary<string, AudioClip> transpositionTableClips;

    public Transform player;
    public AudioClip lrToMove;
    public AudioClip zjump;
    public AudioClip dash;
    public AudioClip wallJump;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        GetComponent<Renderer>().sortingLayerName = "UI";
        audioSource = GetComponent<AudioSource>();

        transpositionTableClips = new Dictionary<string, AudioClip>()
        {
            {"Press left and right to move.", lrToMove },
            {"Press Z to jump.", zjump },
            {"Press X and any direction to dash in that direction. Dash in the air for a double jump!", dash },
            {"Press jump while touching a wall to wall jump.", wallJump }
        };
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
        PlayIfNew(text.text);
    }

    void PlayIfNew(string clip)
    {
        if (lastPlayed != clip && Player.readTutorial)
        {
            lastPlayed = clip;
            audioSource.clip = transpositionTableClips[clip];
            audioSource.Play();
        }
    }
}
