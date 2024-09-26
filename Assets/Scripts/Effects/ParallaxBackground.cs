using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float _parallaxEffect;
    private GameObject _cam;
    private float _xPosition;
    private float _yPosition;
    private float _length;

    private void Start()
    {
        _cam = Camera.main.gameObject;
        _xPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float distanceToMove = _cam.transform.position.x * _parallaxEffect;
        transform.position = new Vector3(distanceToMove + _xPosition, transform.position.y);

        float distanceMoved = _cam.transform.position.x * (1 - _parallaxEffect);
        if (distanceMoved > _xPosition + _length)
            _xPosition = _xPosition + _length;
        else if (distanceMoved < _xPosition - _length)
            _xPosition = _xPosition - _length;
    }
}