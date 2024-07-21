using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAreaDetection : MonoBehaviour
{
    public GameObject textBubble;
    public TMP_Text textBubbleContent;
    public float goodbyeDuration = 2f;

    void Start()
    {
        textBubble.SetActive(false);
        textBubbleContent.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            textBubbleContent.text = "WELCOME!";
            textBubble.SetActive(true);
            textBubbleContent.enabled = true;
            Debug.Log("Player entered the shop area.");

            //Play sound on enter
            FindObjectOfType<AudioManager>().PlaySFX("Enter shop sfx");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            textBubbleContent.text = "SEE YA";
            Debug.Log("Player exited the shop area.");
            StartCoroutine(DeactivateTextBubbleAfterDelay());
        }
    }

    private IEnumerator DeactivateTextBubbleAfterDelay()
    {
        yield return new WaitForSeconds(goodbyeDuration);
        textBubble.SetActive(false);
        textBubbleContent.enabled = false;
    }

    public void ShowItemPrice(int itemPrice)
    {
        if (itemPrice == 1)
        {
            textBubbleContent.text = "THAT COSTS " + itemPrice + " COIN";
        }
        else
        {
            textBubbleContent.text = "THAT COSTS " + itemPrice + " COINS";
        }
        textBubble.SetActive(true);
        textBubbleContent.enabled = true;
    }

    private List<string> walkAwayDialogues = new List<string> { "TAKE YOUR TIME", "DON'T BE A STRANGER", "NOT BUYING?", "IT'S FREE TO LOOK" };

    public void HideItemPrice()
    {
        int index = Random.Range(0,walkAwayDialogues.Count);
        textBubbleContent.text = walkAwayDialogues[index];
    }
}
