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
    private bool _collided;

    private void Update()
    {
        if (_collided && _doorAccessed)
        {
            SceneManager.LoadScene( _sceneToLoad );
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.tag == "Player")
        {
            _collided = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            _collided = false;
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _doorAccessed = inputValue.isPressed;
    }
}
