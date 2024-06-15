using System.Collections;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Material _flashMaterial;

    private Material _originalMaterial;

    [SerializeField]
    private float _flashDuration = 0.025f;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flash()
    {
        if (_spriteRenderer != null)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        _spriteRenderer.material = _flashMaterial;

        yield return new WaitForSeconds(_flashDuration);

        _spriteRenderer.material = _originalMaterial;

        Debug.Log("FlashCoroutine finished.");
    }
}
