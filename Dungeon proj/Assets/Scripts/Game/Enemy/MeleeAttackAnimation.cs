using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAnimation : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private float _attackAnimationDuration;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void StartAttackAnimation()
    {
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        _animator.SetBool("IsAttacking", true);

        // Wait for the duration of the attack animation
        yield return new WaitForSeconds(_attackAnimationDuration);

        _animator.SetBool("IsAttacking", false);
    }
}
