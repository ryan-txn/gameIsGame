using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public IEnumerator FlashCoroutine(float flashDuration, Color flashColor, int numberOfFlashes)
    {
        //we want to change the colour between the start colour and flash colour
        Color startColor = _spriteRenderer.color;

        float elapsedFlashTime = 0;
        float elapsedFlashPercentage = 0;

        //loop while elapsed time is less than duration
        while(elapsedFlashTime < flashDuration)
        {
            elapsedFlashTime += Time.deltaTime;
            elapsedFlashPercentage = elapsedFlashTime / flashDuration;

            if (elapsedFlashPercentage > 1)
            {
                elapsedFlashPercentage = 1;
            }

            float pingPongPercentage = Mathf.PingPong(elapsedFlashPercentage * 2 * numberOfFlashes, 1);

            //color transitions from start to flash
            //but need pingpong to make it flash back and forth. percentage value needs to go back and forth between 0 and 1
            _spriteRenderer.color = Color.Lerp(startColor, flashColor, pingPongPercentage);

            yield return null; //tells coroutine to wait until the next frame
        }

    }
}
