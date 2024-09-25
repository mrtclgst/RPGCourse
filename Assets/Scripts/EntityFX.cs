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

    [Header("Ailment FXs")]
    [SerializeField] private ParticleSystem _igniteFX;
    [SerializeField] private ParticleSystem _chillFX;
    [SerializeField] private ParticleSystem _shockFX;


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
        if (_igniteFX.isPlaying) _igniteFX.Stop();
        else if (_chillFX.isPlaying) _chillFX.Stop();
        else _shockFX.Stop();
    }
    public void IgniteFXFor(float seconds)
    {
        _igniteFX.Play();
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
        _chillFX.Play();
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
        _shockFX.Play();
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