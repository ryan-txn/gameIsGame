using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    //to be called in OnClick event
    public void PlayButtonSound(string name)
    {
        FindObjectOfType<AudioManager>().PlaySFX(name);
    }
}
