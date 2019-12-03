using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class ContextualReminder : MonoBehaviour
{
    public string MessageToShow;
    public GameObject MessageBox;
    public AudioClip narration;

    private AudioSource sPlayer;
    private TMPro.TextMeshProUGUI _textBox;
    private float _displayTime = 0;
    private bool _showing = false;
    private float cooldownTime = 30f;
    private float curCooldownTime = 0f; // timer that counts down, ready at 0
    private bool playable; // if we have a sound clip even

    private void Start()
    {
        _textBox = MessageBox.GetComponent<TMPro.TextMeshProUGUI>();
        SetMessage(MessageToShow);
        sPlayer = GetComponent<AudioSource>();
        playable = narration != null;
        if (playable)
        {
            sPlayer.clip = narration;
        }
    }

    public void SetMessage(string message)
    {
        _textBox.text = message;
    }

    private void Update()
    {
        if (_displayTime > 0)
        {
            _displayTime -= Time.deltaTime;
            _showing = true;
        }
        else
        {
            if (_showing)
            {
                MessageBox.SetActive(false);
                _showing = false;
            }
        }
        if (curCooldownTime > 0f)
        {
            curCooldownTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _displayTime = 15f;
            MessageBox.SetActive(true);
            _showing = true;

            if (curCooldownTime <= 0 && playable)
            {
                sPlayer.Play();
            }
        }
    }
}
