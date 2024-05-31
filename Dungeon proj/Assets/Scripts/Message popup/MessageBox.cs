using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    private string message; // The message to display when the player is near
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && MessageBoxManager._instance != null)
        {
            MessageBoxManager._instance.ShowMessage(message);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && MessageBoxManager._instance != null)
        {
            MessageBoxManager._instance.HideMessage();
        }
    }
}
