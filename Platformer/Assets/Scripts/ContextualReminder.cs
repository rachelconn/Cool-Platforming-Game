using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContextualReminder : MonoBehaviour
{
    public string MessageToShow;
    public GameObject MessageBox;

    private TMPro.TextMeshProUGUI _textBox;
    private float _displayTime = 0;
    private bool _showing = false;

    private void Start()
    {
        _textBox = MessageBox.GetComponent<TMPro.TextMeshProUGUI>();
        SetMessage(MessageToShow);
    }

    public void SetMessage(string message)
    {
        Debug.Log(string.Format("Message set to: {0}", message));
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _displayTime = 15f;
            MessageBox.SetActive(true);
            _showing = true;
        }
    }
}
