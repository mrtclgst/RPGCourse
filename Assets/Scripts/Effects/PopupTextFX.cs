using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextFX : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _disappearanceSpeed;
    [SerializeField] private float _colorDisappearanceSpeed;
    [SerializeField] private float _lifeTime;

    private TextMeshPro _text;
    private float _textTimer;

    void Start()
    {
        _text = GetComponent<TextMeshPro>();
        _textTimer = _lifeTime;
    }
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        _textTimer -= Time.deltaTime;
        
        float alpha = _text.color.a - _colorDisappearanceSpeed * Time.deltaTime;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);

        if (_textTimer < 0)
        {
            if (_text.color.a < 50)
                _speed = _disappearanceSpeed;

            if (_text.color.a < 0)
                Destroy(gameObject);
        }
    }
}