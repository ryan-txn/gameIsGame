using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBoxManager : MonoBehaviour
{
    public static MessageBoxManager _instance;

    [SerializeField]
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        // Ensures only one message box manager exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Message box inactive initially
        gameObject.SetActive(false);
    }

    // Show the message box with a specific message and position it at the anchor point
    public void ShowMessage(string message)
    {
        textComponent.text = message;
        gameObject.SetActive(true);
    }

    // Hide the message box
    public void HideMessage()
    {
        gameObject.SetActive(false);
    }

  
}
