using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveMoveState : EntityState
{
    [Space]
    [SerializeField, Min(0)] float _speed = 1; 
    [SerializeField, Min(0)] float _waveSpeed = 1;
    [SerializeField, Min(0)] float _waveAmplitude = 1;
    [SerializeField, Min(0)] float _waveFrequency = 1;
    [SerializeField] Axis _movementAxis = Axis.Horizontal;

    [Space]
    [SerializeField] EntityState _subState;
    [SerializeField, Min(0)] float _subStateFrequency = 2f;
    
    Vector2 _initialPosition;

    enum Axis
    {
        Vertical,
        Horizontal,
    }
    Vector2 _moveAxis;
    Vector2 _waveAxis;

    public override void Enter()
    {
        IsComplete = false;
        _initialPosition = Core.transform.position;
        switch (_movementAxis)
        {
            case Axis.Horizontal:
                _moveAxis = Vector2.right;
                _waveAxis = Vector2.up;
                break;
            case Axis.Vertical:
                _moveAxis = Vector2.up;
                _waveAxis = Vector2.right;
                break;
        }
    }

    public override void Execute()
    {
        float yOffset = Mathf.Sin(Time.time * _waveSpeed * _waveFrequency) * _waveAmplitude;
        Vector3 newPosition = _initialPosition + _waveAxis * yOffset;
        Core.transform.position = newPosition;
        _initialPosition += _moveAxis * _speed * Time.deltaTime;

        SetSubstate();
    }

    Coroutine _subStateCoroutine;
    void SetSubstate()
    {
        if (_subState != null && _subStateCoroutine == null)
            _subStateCoroutine = StartCoroutine(Run());

        IEnumerator Run()
        {
            yield return new WaitForSeconds(_subStateFrequency);
            SetState(_subState, true);
            _subStateCoroutine = null;
        }
    }

    public override void Exit()
    {
        IsComplete = true;
    }

    public override void FixedExecute()
    {
    }
}
