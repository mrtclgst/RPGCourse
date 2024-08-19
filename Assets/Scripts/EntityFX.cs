using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Header("Flash FX")]
    [SerializeField] private Material _hitMat;
    [SerializeField] private float _flashDuration;
    private Material _originalMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] _chilledColorArray;
    [SerializeField] private Color[] _igniteColorArray;
    [SerializeField] private Color[] _shockedColorArray;


    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
    }

    private IEnumerator IE_FlashFX()
    {
        _spriteRenderer.material = _hitMat;
        Color currentColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.color = currentColor;
        _spriteRenderer.material = _originalMat;
    }

    private void RedColorBlink()
    {
        if (_spriteRenderer.color != Color.white)
        {
            _spriteRenderer.color = Color.white;
        }
        else
        {
            _spriteRenderer.color = Color.yellow;
        }
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        _spriteRenderer.color = Color.white;
    }
    public void IgniteFXFor(float seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, 0.2f);
        Invoke("CancelColorChange", seconds);
    }
    private void IgniteColorFX()
    {
        if (_spriteRenderer.color != _igniteColorArray[0])
        {
            _spriteRenderer.color = _igniteColorArray[0];
        }
        else
        {
            _spriteRenderer.color = _igniteColorArray[1];
        }
    }
    public void ChillFXFor(float seconds)
    {
        InvokeRepeating("ChillColorFX", 0, 0.2f);
        Invoke("CancelColorChange", seconds);
    }
    private void ChillColorFX()
    {
        if (_spriteRenderer.color != _chilledColorArray[0])
        {
            _spriteRenderer.color = _chilledColorArray[0];
        }
        else
        {
            _spriteRenderer.color = _chilledColorArray[1];
        }
    }
    public void ShockFXFor(float seconds)
    {
        InvokeRepeating("ShockColorFX", 0, 0.2f);
        Invoke("CancelColorChange", seconds);
    }
    private void ShockColorFX()
    {
        if (_spriteRenderer.color != _shockedColorArray[0])
        {
            _spriteRenderer.color = _shockedColorArray[0];
        }
        else
        {
            _spriteRenderer.color = _shockedColorArray[1];
        }
    }
}