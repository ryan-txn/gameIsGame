using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NonAutoCollectable : MonoBehaviour
{
    private interfaceCollectableBehaviour _collectableBehaviour;

    private bool _itemInteracted;

    private void Awake()
    {
        _collectableBehaviour = GetComponent<interfaceCollectableBehaviour>();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.GetComponent<PlayerMovement>();

        if (collider.gameObject.tag == "Player" && _itemInteracted)
        {
            _collectableBehaviour.OnCollected(player.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _itemInteracted = inputValue.isPressed;
    }
}
