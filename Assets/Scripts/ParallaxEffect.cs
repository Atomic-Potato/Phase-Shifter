using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField, Min(0)] float _parallaxMultiplier = 1;
    Vector2 _startPosition;
    Transform _camera;

    void Start()
    {
        _camera = Camera.main.transform;    
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(
            _startPosition.x + (_camera.position.x * _parallaxMultiplier),
            _startPosition.y + (_camera.position.y * _parallaxMultiplier),
            transform.position.z
        );
    }
}
