using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    public string _sceneToLoad;

    private bool _doorAccessed;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" /*&& _doorAccessed*/)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _doorAccessed = inputValue.isPressed;
    }
}
