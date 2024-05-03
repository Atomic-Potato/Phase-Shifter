using UnityEngine;

[RequireComponent(typeof(CameraCore))]
public class CameraManager : MonoBehaviour 
{
    [SerializeField] CameraCore _cameraCore;

    [Space, Header("States")]
    [SerializeField] CameraSinglePointState _singlePointState;

    Player _player;

    void Start()
    {
        _player = Player.Instance;
        _singlePointState.Target = _player.transform;
        _cameraCore.StateMachine.SetState(_singlePointState);
    }

    void Update()
    {
        SelectState();
        _cameraCore.CameraState.Execute();
    }

    void LateUpdate()
    {
        _cameraCore.CameraState.LateExecute();
    }

    void FixedUpdate()
    {
        _cameraCore.CameraState.FixedExecute();
    }

    void SelectState()
    {
        _cameraCore.StateMachine.SetState(_singlePointState);
    }
}