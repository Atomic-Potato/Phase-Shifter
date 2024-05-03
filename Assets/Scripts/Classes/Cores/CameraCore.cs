using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraCore : Core
{
    [SerializeField] Camera _camera;
    public Camera Camera => _camera;

    public CameraState CameraState => (CameraState)State;
}