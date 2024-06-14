using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCollectable : MonoBehaviour
{
    private interfaceCollectableBehaviour _collectableBehaviour;
    private Animator _animator;
    private bool _isCollected = false;

    private void Awake()
    {
        _collectableBehaviour = GetComponent<interfaceCollectableBehaviour>();
        _animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        SetAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();

        //if player collides with collectable, use oncollected method, destroy collectable
        if (player !=null)
        {
            _collectableBehaviour.OnCollected(player.gameObject);
            _isCollected = true;
            Destroy(gameObject, (float)0.1);
        }
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsCollected", _isCollected);
    }
}
